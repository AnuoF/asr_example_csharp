/*********************************************************************************************
 *	
 * 文件名称:    Asr.cs
 * 
 * 描    述：   语音识别实现类，在本 dll 中作为代理功能类，配合 AsrService 实现语音识别分布式
 *              部署调用。
 *
 * 作    者:    王 喜 进
 *	
 * 创作日期:    2019-3-26
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using System.Collections.Generic;
using System;
using AsrLibrary.Entity;

namespace AsrLibrary.Proxy
{
    /// <summary>
    /// /语音识别实现类，在本 dll 中作为代理功能类，配合 AsrService 实现语音识别分布式部署调用。
    /// </summary>
    internal class Asr : IAsr, ITranslate
    {
        /// <summary>
        /// 业务逻辑处理类
        /// </summary>
        private BuzProcess _buzProcess = null;
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
        private object _lockObj = new object();
        /// <summary>
        /// 类实例
        /// </summary>
        private static Asr _instance = null;
        /// <summary>
        /// Asr 类锁，因识别和翻译都共用这个类
        /// </summary>
        private static object _lockAsr = new object();

        /// <summary>
        /// 获取与服务端的连接状态
        /// </summary>
        public bool IsConnected
        {
            get { return _buzProcess.IsConnected; }
        }

        /// <summary>
        /// 建立与服务端的连接事件
        /// </summary>
        public event EventHandler OnInitialized;

        /// <summary>
        /// 构造函数
        /// </summary>
        private Asr()
        {
            _buzProcess = new BuzProcess();
            _buzProcess.OnInitialized += _buzProcess_OnConnected;
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

        #region IAsr

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public void Initialize()
        {
            _buzProcess.Initialize();
        }

        /// <summary>
        /// 语音识别。如果是标准音频格式：pcm/16k/16位/单通道，则调用此方法。如果是其他格式的音频，请调用另一个方法并传入音频格式 WaveFormat 参数。
        /// </summary>
        /// <param name="audioData">小于 60s 的音频数据（总长度不超过32000），音频格式要求：pcm/16k/16位/单通道 。</param>
        /// <param name="languageType">音频语种类型</param>
        /// <param name="recogResult">识别成功返回识别结果，识别失败返回错误消息</param>
        /// <returns>识别成功或失败，true-成功；false-失败</returns>
        public bool AudioRecog(byte[] audioData, LanguageType languageType, out string recogResult)
        {
            return _buzProcess.AudioRecog(audioData, languageType, out recogResult);
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
                    if (_pcm16kConvert != null)
                    {
                        DisposeAudioConvert();
                    }
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
            return _buzProcess.GetLanguageList();
        }

        /// <summary>
        /// 将配置文件中 name 字段映射到 LanguageType 枚举值
        /// </summary>
        /// <param name="name">name 字段内容</param>
        /// <returns>LanguageType 枚举值</returns>
        public LanguageType Name2LanguageType(string name)
        {
            return _buzProcess.Name2LanguageType(name);
        }

        /// <summary>
        /// 将配置文件中 text 字段映射到 LanguageType 枚举值
        /// </summary>
        /// <param name="text">text 字段内容</param>
        /// <returns>LanguageType 枚举值</returns>
        public LanguageType Text2LanguageType(string text)
        {
            return _buzProcess.Text2LanguageType(text);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            DisposeAudioConvert();

            if (_buzProcess != null)
            {
                _buzProcess.Dispose();
            }
        }
        #endregion

        #region ITranslate

        /// <summary>
        /// 翻译
        /// </summary>
        /// <param name="text"></param>
        /// <param name="from"></param>
        /// <param name="result"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public bool Trans(string text, LanguageType from, out string result, LanguageType to = LanguageType.Mandarin)
        {
            return _buzProcess.Trans(text, from, out result, to);
        }

        /// <summary>
        /// 获取支持的翻译列表
        /// </summary>
        /// <returns></returns>
        public List<Language> GetTransLanguages()
        {
            return _buzProcess.GetTransLanguages();
        }
        #endregion

        // 连接建立事件
        private void _buzProcess_OnConnected(object sender, EventArgs e)
        {
            if (OnInitialized != null)
            {
                OnInitialized.Invoke(sender, e);
            }
        }

        /// <summary>
        /// 创建音频转换类
        /// </summary>
        private void CreateAudioConvert(WaveFormat waveFormat)
        {
            lock (_lockObj)
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
                lock (_lockObj)
                {
                    _pcm16kConvert.Dispose();
                    _pcm16kConvert = null;
                }
            }
        }

    }
}
