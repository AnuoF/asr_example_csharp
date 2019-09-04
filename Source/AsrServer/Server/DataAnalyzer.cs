/*********************************************************************************************
 *	
 * 文件名称:    DataAnalyzer.cs
 * 
 * 描    述：   数据解析类，用于解析、封装数据
 *
 * 作    者:    王 喜 进
 *	
 * 创作日期:    2019-3-27
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using AsrLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace AsrServer
{
    /// <summary>
    /// 数据解析类，用于解析、封装数据
    /// </summary>
    internal class DataAnalyzer
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public DataAnalyzer()
        {

        }

        /// <summary>
        /// 构建管理头
        /// </summary>
        /// <param name="dataType">数据类型</param>
        /// <param name="len">包总长</param>
        /// <returns></returns>
        public byte[] CombineDataHeader(short dataType, int len)
        {
            List<byte> byteList = new List<byte>();
            byteList.Add(Convert.ToByte(0x01));
            byteList.AddRange(BitConverter.GetBytes(len));
            byteList.AddRange(BitConverter.GetBytes(dataType));

            return byteList.ToArray();
        }

        /// <summary>
        /// 解析 0x0501 数据（语音识别业务数据）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Analyze0x0501(byte[] data, out LanguageType languageType, out byte[] audioData)
        {
            try
            {
                int totalLen = BitConverter.ToInt32(data, 1);     // 包总长度
                int dataLen = totalLen - 7;                       // 数据区长度

                int type = BitConverter.ToInt32(data, 7);         // 语种类型
                languageType = (LanguageType)type;

                audioData = new byte[totalLen - 11];
                Buffer.BlockCopy(data, 11, audioData, 0, totalLen - 11);
                return true;
            }
            catch
            {
                audioData = null;
                languageType = LanguageType.Mandarin;
                return false;
            }
        }

        /// <summary>
        /// 解析 0x0503 或者 0x0504 数据 （Text/Name 到 LanguageType 的映射）
        /// </summary>
        /// <param name="data"></param>
        /// <param name="textOrName"></param>
        /// <returns></returns>
        public bool Analyze0x0503_4(byte[] data, out string textOrName)
        {
            try
            {
                int totalLen = BitConverter.ToInt32(data, 1);     // 包总长度
                int dataLen = totalLen - 7;                       // 数据区长度

                textOrName = Encoding.UTF8.GetString(data, 7, dataLen);
                return true;
            }
            catch
            {
                textOrName = string.Empty;
                return false;
            }
        }

        /// <summary>
        /// 解析 0x0505 数据（翻译）
        /// </summary>
        /// <param name="data"></param>
        /// <param name="text"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>解析成功或失败</returns>
        public bool Analyze0x0505(byte[] data, out string text, out LanguageType from, out LanguageType to)
        {
            try
            {
                int totalLen = BitConverter.ToInt32(data, 1);
                int f = BitConverter.ToInt32(data, 7);
                int t = BitConverter.ToInt32(data, 11);
                from = (LanguageType)f;
                to = (LanguageType)t;
                text = Encoding.UTF8.GetString(data, 15, totalLen - 15);
                return true;
            }
            catch
            {
                text = string.Empty;
                from = LanguageType.Mandarin;
                to = LanguageType.Mandarin;
                return false;
            }
        }

        /// <summary>
        /// 封装 0x0601 数据（语音识别结果）
        /// </summary>
        /// <param name="recgResult">识别结果</param>
        /// <param name="success">识别成功或失败</param>
        /// <returns>待发送的数据</returns>
        public byte[] Combine0x0601(string recgResult, bool success)
        {
            byte b1 = 1;
            byte b0 = 0;
            byte succ = success ? b1 : b0;
            byte[] audioData = Encoding.UTF8.GetBytes(recgResult);

            List<byte> byteList = new List<byte>();
            byteList.AddRange(CombineDataHeader(0x0601, 7 + 1 + audioData.Length));
            byteList.Add(succ);
            byteList.AddRange(audioData);

            return byteList.ToArray();
        }

        /// <summary>
        /// 组合成 XML 字符串
        /// </summary>
        /// <param name="languageList">语种列表</param>
        /// <param name="dataType">数据类型</param>
        /// <returns></returns>
        public byte[] Combine0x0602(List<Language> languageList, short dataType)
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<list>"
                + "</list>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode list = doc.SelectSingleNode("./list");

            foreach (Language lan in languageList)
            {
                XmlElement languageNode = doc.CreateElement("Language");

                XmlElement nameNode = doc.CreateElement("Name");
                XmlElement textNode = doc.CreateElement("Text");
                XmlElement capacityNode = doc.CreateElement("Capacity");
                XmlElement engineNode = doc.CreateElement("Engine");

                nameNode.InnerXml = lan.Name;
                textNode.InnerXml = lan.Text;
                capacityNode.InnerXml = lan.Capacity;
                engineNode.InnerXml = lan.Engine;

                languageNode.AppendChild(nameNode);
                languageNode.AppendChild(textNode);
                languageNode.AppendChild(capacityNode);
                languageNode.AppendChild(engineNode);

                list.AppendChild(languageNode);
            }

            byte[] data = Encoding.UTF8.GetBytes(doc.InnerXml);

            List<byte> byteList = new List<byte>();
            byteList.Add(Convert.ToByte(0x01));
            byteList.AddRange(BitConverter.GetBytes(7 + data.Length));
            byteList.AddRange(BitConverter.GetBytes(dataType));
            byteList.AddRange(data);

            return byteList.ToArray(); ;
        }

        /// <summary>
        /// 组合 0x0603 数据
        /// </summary>
        /// <param name="languageType">语种类型</param>
        /// <returns></returns>
        public byte[] Combine0x0603(LanguageType languageType)
        {
            int type = (int)languageType;
            List<byte> list = new List<byte>();
            list.AddRange(CombineDataHeader(0x0603, 7 + 4));
            list.AddRange(BitConverter.GetBytes(type));

            return list.ToArray();
        }

        /// <summary>
        /// 组合 0x0605 数据（翻译结果）
        /// </summary>
        /// <param name="succ">翻译成功失败</param>
        /// <param name="transResult">翻译结果</param>
        /// <returns></returns>
        public byte[] Combine0x0605(bool succ, string transResult)
        {
            byte s = 1;
            byte f = 0;
            byte[] resultArr = Encoding.UTF8.GetBytes(transResult);
            List<byte> list = new List<byte>();
            list.AddRange(CombineDataHeader(0x0605, 7 + 1 + resultArr.Length));
            list.Add(succ ? s : f);
            list.AddRange(resultArr);

            return list.ToArray();
        }

    }
}
