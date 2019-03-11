/*********************************************************************************************
 *	
 * 文件名称:    Asr.cs
 * 
 * 描    述：   语音识别实现类，用于封装多个 SDK，对外提供统一的接口调用封装。
 *
 * 作    者:    Anuo
 *	
 * 创作日期:    2019-2-27
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using AsrLibrary.Asr.Baidu;
using AsrLibrary.Asr.iFly;
using AsrLibrary.Asr.Jths;
using AsrLibrary.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace AsrLibrary.Asr
{
    /// <summary>
    /// 语音识别实现类
    /// </summary>
    internal class Asr : IAsr
    {
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
        /// 构造函数
        /// </summary>
        public Asr()
        {
            // 加载配置信息
            LoadLanagueFromConfig();
        }

        /// <summary>
        /// 语音识别
        /// </summary>
        /// <param name="audioData">小于 60s 的音频数据（总长度不超过32000），音频格式要求：pcm/16k/16位/单通道 。</param>
        /// <param name="languageType">音频语种类型</param>
        /// <param name="recogResult">识别成功返回识别结果，识别失败返回错误消息</param>
        /// <returns>识别成功或失败，true-成功；false-失败</returns>
        public bool AudioRecog(byte[] audioData, LanguageType languageType, out string recogResult)
        {
            if (Utils._languageRecogList == null || Utils._languageRecogList.Count <= 0)
            {
                recogResult = "无可识别的语种，请检查配置文件 AsrLibrary.config 是否存在，或者是否正确配置。";
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
                recogResult = "未匹配到对应的音频识别 sdk，请检查配置文件字段“engine”是否正确。";
                return false;
            }
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
            return Utils.Text2LanguageType(text);
        }

        /// <summary>
        /// 将配置文件中 name 字段映射到 LanguageType 枚举值
        /// </summary>
        /// <param name="name">name 字段内容</param>
        /// <returns>LanguageType 枚举值</returns>
        public LanguageType Name2LanguageType(string name)
        {
            return Utils.Name2LanguageType(name);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if(_jths != null)
            {
                _jths.Dispose();
                _jths = null;
            }
        }

        /// <summary>
        /// 从配置文件 AsrLibrary.config 读取可识别的语种列表
        /// </summary>
        private void LoadLanagueFromConfig()
        {
            try
            {
                // 识别和翻译都调用了此方法，所以需先 Clear()
                Utils._languageRecogList.Clear();
                Utils._languageTransList.Clear();

                string configPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "AsrLibrary.config");
                Utils.configName = configPath;
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
