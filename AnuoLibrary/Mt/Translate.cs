/*********************************************************************************************
 *	
 * 文件名称:    Translate.cs
 * 
 * 描    述：   翻译实现类。
 *
 * 作    者:    Anuo
 *	
 * 创作日期:    2019-3-4
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using AnuoLibrary.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace AnuoLibrary.Mt
{
    /// <summary>
    /// 翻译实现类
    /// </summary>
    public class Translate : ITranslate
    {
        /// <summary>
        /// 捷通华声翻译
        /// </summary>
        private JthsTrans _jths = null;

        /// <summary>
        /// 民族语文翻译
        /// </summary>
        private MzywfyTrans _mzywfy = null;

        /// <summary>
        /// 小牛翻译
        /// </summary>
        private NiuTrans _niu = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Translate()
        {
            LoadLanagueFromConfig();
            _jths = new JthsTrans();
            _mzywfy = new MzywfyTrans();
            _niu = new NiuTrans();
        }

        /// <summary>
        /// 获取支持的语种
        /// </summary>
        /// <returns>支持的语种列表</returns>
        public List<Language> GetTransLanguages()
        {
            return Utils._languageTransList;
        }

        /// <summary>
        /// 翻译
        /// </summary>
        /// <param name="text">待翻译的内容</param>
        /// <param name="from">翻译源语种</param>
        /// <param name="result">成功返回翻译结果，失败返回错误消息</param>
        /// <param name="to">翻译目的语种，默认为中文</param>
        /// <returns>true-成功；false-失败</returns>
        public bool Trans(string text, LanguageType from, out string result, LanguageType to = LanguageType.Mandarin)
        {
            Language language = Utils._languageTransList.Find(o => o.Name == from.ToString());
            if (language == null)
            {
                result = "暂不支持该语种的翻译";
                return false;
            }

            result = string.Empty;
            TransBase trans = null;

            switch (language.Engine.ToLower())
            {
                case "jths":
                    trans = _jths;
                    break;

                case "mzywfy":
                    trans = _mzywfy;
                    break;

                case "niu":
                    trans = _niu;
                    break;
            }

            if (trans == null)
            {
                result = "未匹配上对应的翻译接口，请检查配置文件是否正确配置。";
                return false;
            }
            else
            {
                return trans.Translate(text, from, to, out result);
            }
        }

        /// <summary>
        /// 从配置文件 AnuoLibrary.config 读取可识别的语种列表
        /// </summary>
        private void LoadLanagueFromConfig()
        {
            try
            {
                // 识别和翻译都调用的此方法，所以需先 Clear()
                Utils._languageRecogList.Clear();
                Utils._languageTransList.Clear();

                string configPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "AnuoLibrary.config");
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
