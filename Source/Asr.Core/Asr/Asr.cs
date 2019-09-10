/*********************************************************************************************
 *	
 * 文件名称:    Asr.cs
 * 
 * 描    述：   语音识别实现类，用于封装多个 SDK，对外提供统一的接口调用封装。
 *
 * 作    者:    王 喜 进
 *	
 * 创作日期:    2019-2-27
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using Asr.Public;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Asr.Core
{
    /// <summary>
    /// 语音识别实现类
    /// </summary>
    public class Asr : IAsr
    {
        /// <summary>
        /// 获取连接状态
        /// </summary>
        public bool IsConnected
        {
            // 本地调用始终为 true
            get { return true; }
        }

        /// <summary>
        /// 初始化成功实践
        /// </summary>
        public event EventHandler OnInitialized;

        /// <summary>
        /// 百度语音识别
        /// </summary>
        private BaiduAsr _baidu = null;
        /// <summary>
        /// 捷通华声语音识别
        /// </summary>
        private JthsAsr _jths = null;
        /// <summary>
        /// 讯飞语音识别
        /// </summary>
        private iFlyAsr _ifly = null;
        /// <summary>
        /// 上一次处理的音频格式
        /// </summary>
        private WaveFormat _prevWaveFormat = null;
        /// <summary>
        /// 音频转换类，将音频转换成 16K采样率、16位、单音
        /// </summary>
        private AudioConvert _pcm16kConvert = null;
        /// <summary>
        /// 资源互斥锁
        /// </summary>
        private object _lockAudio = new object();
        /// <summary>
        /// 实例
        /// </summary>
        private static Asr _instance = null;
        /// <summary>
        /// 单例锁
        /// </summary>
        private static object _lockAsr = new object();


        /// <summary>
        /// 构造函数
        /// </summary>
        private Asr()
        {
            // 加载配置信息
            LoadLanagueFromConfig();
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static Asr GetInstance()
        {
            // Double-Check Locking （双重锁定）
            if (_instance == null)
            {
                lock (_lockAsr)
                {
                    if (_instance == null)
                    {
                        _instance = new Asr();
                    }
                }
            }

            return _instance;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            // 此方法在 AsrProxy 中使用，这里不需要做任何动作
            if (OnInitialized != null)
            {
                OnInitialized.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// 语音识别。如果是标准音频格式：pcm/16k/16位/单通道，则调用此方法。如果是其他格式的音频，请调用另一个方法并传入音频格式 WaveFormat 参数。
        /// </summary>
        /// <param name="audioData">小于 60s 的音频数据（总长度不超过1920k），音频格式要求：pcm/16k/16位/单通道 。</param>
        /// <param name="languageType">音频语种类型</param>
        /// <param name="recogResult">识别成功返回识别结果，识别失败返回错误消息</param>
        /// <returns>识别成功或失败，true-成功；false-失败</returns>
        public bool AudioRecog(byte[] audioData, LanguageType languageType, out string recogResult)
        {
            if (Utils._languageRecogList == null || Utils._languageRecogList.Count <= 0)
            {
                recogResult = "无可识别的语种，请检查配置文件 Asr.Core.config 是否存在，或者是否正确配置。";
                return false;
            }

            Language language = Utils._languageRecogList.Find(o => o.Name == languageType.ToString());
            if (language == null)
            {
                recogResult = "传入的语种未匹配上对应的能力。";
                return false;
            }

            AsrBase asr = null;

            switch (language.Engine.ToLower())
            {
                case "baidu":
                    if (_baidu == null)
                    {
                        _baidu = BaiduAsr.GetInstance();
                    }
                    asr = _baidu;
                    break;

                case "jths":
                    if (_jths == null)
                    {
                        _jths = JthsAsr.GetInstance();
                    }
                    asr = _jths;
                    break;

                case "ifly":
                    if (_ifly == null)
                    {
                        _ifly = iFlyAsr.GetInstance();
                    }
                    asr = _ifly;
                    break;

                default:
                    break;
            }

            if (asr != null)
            {
                return asr.AudioRecog(audioData, languageType, out recogResult);
            }
            else
            {
                recogResult = "未匹配到对应的音频识别 sdk，请检查配置文件字段“engine”是否正确配置。";
                return false;
            }
        }

        /// <summary>
        /// 语音识别。如果是标准音频格式：pcm/16k/16位/单通道，请调用另一个方法，不需要传入参数 WaveFormat 参数，如果是其他格式，则调用此方法。
        /// </summary>
        /// <param name="audioData">小于 60s 的音频数据</param>
        /// <param name="languageType">音频语种</param>
        /// <param name="recogResult">识别成功返回识别结果，识别失败返回错误消息</param>
        /// <param name="waveFormat">音频格式</param>
        /// <returns>识别成功或失败，true-成功；false-失败</returns>
        public bool AudioRecog(byte[] audioData, LanguageType languageType, out string recogResult, WaveFormat waveFormat)
        {
            if (waveFormat == null)
            {
                recogResult = "传入的音频格式 WaveFormat 不能为 null。";
                return false;
            }

            // 判断上一次处理的音频格式和这次的是否相同，如果相同则不做处理，如果不同，则要释放并从新创建音频转换对象
            if (_prevWaveFormat != null)
            {
                if (_prevWaveFormat.nSamplesPerSec != waveFormat.nSamplesPerSec ||
                    _prevWaveFormat.nChannels != waveFormat.nChannels ||
                    _prevWaveFormat.wBitsPerSample != waveFormat.wBitsPerSample)
                {
                    DisposeAudioConvert();
                }
            }

            // 创建音频转换对象类
            if (_pcm16kConvert == null)
            {
                CreateAudioConvert(waveFormat);

                if (_pcm16kConvert == null)
                {
                    recogResult = "创建音频转换对象失败。";
                    return false;
                }
            }

            // 音频转换
            byte[] data = _pcm16kConvert.Convert(audioData, false);
            if (data == null || data.Length == 0)
            {
                recogResult = "转换音频失败。";
                return false;
            }

            _prevWaveFormat = waveFormat;

            return AudioRecog(data, languageType, out recogResult);
        }

        /// <summary>
        /// 获取可识别的语种列表
        /// </summary>
        /// <returns>语种列表</returns>
        public List<Language> GetLanguageList()
        {
            return Utils._languageRecogList;
        }

        /// <summary>
        /// 将配置文件中 text 字段映射到 LanguageType 枚举值
        /// </summary>
        /// <param name="text">text 字段内容</param>
        /// <returns>LanguageType 枚举值</returns>
        public LanguageType Text2LanguageType(string text)
        {
            return Public.Utils.Text2LanguageType(text);
        }

        /// <summary>
        /// 将配置文件中 name 字段映射到 LanguageType 枚举值
        /// </summary>
        /// <param name="name">name 字段内容</param>
        /// <returns>LanguageType 枚举值</returns>
        public LanguageType Name2LanguageType(string name)
        {
            return Public.Utils.Name2LanguageType(name);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_jths != null)
            {
                _jths.Dispose();
                _jths = null;
            }

            if (_ifly != null)
            {
                _ifly.Dispose();
                _ifly = null;
            }

            if (_baidu != null)
            {
                _baidu.Dispose();
            }

            DisposeAudioConvert();
        }

        /// <summary>
        /// 创建音频转换类
        /// </summary>
        private void CreateAudioConvert(WaveFormat waveFormat)
        {
            lock (_lockAudio)
            {
                _pcm16kConvert = AudioConvert.Create(waveFormat, WaveFormat.PCM_ASR);
            }
        }

        /// <summary>
        /// 释放音频转换非托管资源
        /// </summary>
        private void DisposeAudioConvert()
        {
            if (_pcm16kConvert != null)
            {
                lock (_lockAudio)
                {
                    _pcm16kConvert.Dispose();
                    _pcm16kConvert = null;
                }
            }
        }

        /// <summary>
        /// 从配置文件 Asr.Core.config 读取可识别的语种列表
        /// </summary>
        private void LoadLanagueFromConfig()
        {
            try
            {
                // 防止多次调用
                Utils._languageRecogList.Clear();
                Utils._languageTransList.Clear();

                string configPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Asr.Core.config");
                Utils.ConfigName = configPath;
                XmlDocument doc = new XmlDocument();
                doc.Load(configPath);

                XmlNodeList nodes = doc.SelectSingleNode("./configuration/language").ChildNodes;
                foreach (XmlNode node in nodes)
                {
                    if (node.NodeType == XmlNodeType.Comment)
                        continue;

                    if (node.Attributes["valid"].InnerXml == "true")
                    {
                        Language language = new Language();
                        language.Name = node.Attributes["name"].InnerXml;
                        language.Text = node.Attributes["text"].InnerXml;
                        language.Capacity = node.Attributes["capacity"].InnerXml;
                        language.Engine = node.Attributes["engine"].InnerXml;
                        language.Valid = true;
                        Utils._languageRecogList.Add(language);
                    }
                }

                nodes = doc.SelectSingleNode("./configuration/translate").ChildNodes;
                foreach (XmlNode node in nodes)
                {
                    if (node.NodeType == XmlNodeType.Comment)
                        continue;

                    if (node.Attributes["valid"].InnerXml == "true")
                    {
                        Language language = new Language();
                        language.Name = node.Attributes["name"].InnerXml;
                        language.Text = node.Attributes["text"].InnerXml;
                        language.Capacity = node.Attributes["capacity"].InnerXml;
                        language.Engine = node.Attributes["engine"].InnerXml;
                        language.Valid = true;
                        Utils._languageTransList.Add(language);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("ASR 异常:" + ex.Message);
            }
        }

    }
}
