/*********************************************************************************************
 *	
 * 文件名称:    DataHelper.cs
 * 
 * 描    述：   数据帮助类，实现数据的封装，解析等功能。
 *
 * 作    者:    王 喜 进
 *	
 * 创作日期:    2019-3-27
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using Asr.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Asr.Client
{
    /// <summary>
    /// 数据帮助类，实现数据的封装，解析等功能。
    /// </summary>
    internal class DataHelper
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public DataHelper()
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
        /// 封装语音识别业务数据
        /// </summary>
        /// <param name="audioData">语音数据</param>
        /// <param name="languageType">语种类型</param>
        /// <returns></returns>
        public byte[] CombineData0x0501(byte[] audioData, LanguageType languageType)
        {
            List<byte> byteList = new List<byte>();
            byteList.AddRange(CombineDataHeader(0x0501, 7 + 4 + audioData.Length));
            byteList.AddRange(BitConverter.GetBytes((int)languageType));
            byteList.AddRange(audioData);

            return byteList.ToArray();
        }

        /// <summary>
        /// 组合 0x0502 数据（获取可识别的语种列表）
        /// </summary>
        /// <returns></returns>
        public byte[] CombineData0x0502()
        {
            // 0x0502数据类型只有管理头没有数据区
            return CombineDataHeader(0x0502, 7 + 0);
        }

        /// <summary>
        /// 组合 0x0503 数据（Text 到 LanguageType 的映射）
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public byte[] CombineData0x0503(string text)
        {
            List<byte> byteList = new List<byte>();

            byte[] textArr = Encoding.UTF8.GetBytes(text);
            byteList.AddRange(CombineDataHeader(0x0503, 7 + textArr.Length));
            byteList.AddRange(textArr);

            return byteList.ToArray();
        }

        /// <summary>
        /// 组合 0x0504 数据（Name 到 LanguageType 的映射）
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public byte[] CombineData0x0504(string name)
        {
            List<byte> byteList = new List<byte>();

            byte[] textArr = Encoding.UTF8.GetBytes(name);
            byteList.AddRange(CombineDataHeader(0x0504, 7 + textArr.Length));
            byteList.AddRange(textArr);

            return byteList.ToArray();
        }

        /// <summary>
        /// 组合 0x0505 数据（翻译）
        /// </summary>
        /// <param name="text">待翻译的内容</param>
        /// <param name="from">From</param>
        /// <param name="to">To</param>
        /// <returns></returns>
        public byte[] CombineData0x0505(string text, LanguageType from, LanguageType to)
        {
            List<byte> list = new List<byte>();

            byte[] textArr = Encoding.UTF8.GetBytes(text);

            list.AddRange(CombineDataHeader(0x0505, 7 + 4 + 4 + textArr.Length));
            list.AddRange(BitConverter.GetBytes((int)from));
            list.AddRange(BitConverter.GetBytes((int)to));
            list.AddRange(textArr);

            return list.ToArray();
        }

        /// <summary>
        /// 组合 0x0506 数据（支持翻译的语种列表）
        /// </summary>
        /// <returns></returns>
        public byte[] CombineData0x0506()
        {
            // 只有包头
            return CombineDataHeader(0x0506, 7);
        }

        /// <summary>
        /// 解析 0x0601 数据（语音识别结果）
        /// </summary>
        /// <param name="data"></param>
        /// <param name="recgResult">成功，语音识别结果；false-错误消息</param>
        /// <returns>true-成功；false-失败</returns>
        public bool Analyze0x0601(byte[] data, out string recgResult)
        {
            try
            {
                int totalLen = BitConverter.ToInt32(data, 1);  // 包总长度
                int dataLen = totalLen - 7;                    // 数据区长度

                byte flag = Buffer.GetByte(data, 7);           // 成功失败 flag：1-成功，其他-失败
                recgResult = Encoding.UTF8.GetString(data, 8, dataLen - 1);

                return flag == 1 ? true : false;
            }
            catch (Exception ex)
            {
                recgResult = "解析业务数据 0x0601 失败。";
                Log.WriteLog(recgResult + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 解析 0x0602 数据（可识别的语种列表）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<Language> Analyze0x0602(byte[] data)
        {
            try
            {
                int totalLen = BitConverter.ToInt32(data, 1);  // 包总长度
                int dataLen = totalLen - 7;                    // 数据区长度
                byte[] newData = new byte[dataLen];
                Buffer.BlockCopy(data, 7, newData, 0, dataLen);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Encoding.UTF8.GetString(newData));
                XmlNodeList nodes = doc.SelectSingleNode("./list").ChildNodes;
                List<Language> list = new List<Language>();

                foreach (XmlNode node in nodes)
                {
                    Language lan = new Language();
                    lan.Name = node.SelectSingleNode("./Name").InnerXml;
                    lan.Text = node.SelectSingleNode("./Text").InnerXml;
                    lan.Capacity = node.SelectSingleNode("./Capacity").InnerXml;
                    lan.Engine = node.SelectSingleNode("./Engine").InnerXml;
                    list.Add(lan);
                }

                return list;
            }
            catch (Exception ex)
            {
                Log.WriteLog("解析 0x0602 数据异常：" + ex.Message);
                return new List<Language>();
            }
        }

        /// <summary>
        /// 解析语种类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public LanguageType Analyze0x0603(byte[] data)
        {
            try
            {
                int type = BitConverter.ToInt32(data, 7);

                return (LanguageType)type;
            }
            catch (Exception ex)
            {
                Log.WriteLog("解析语种类型异常：" + ex.Message);
                return LanguageType.Mandarin;
            }
        }

        /// <summary>
        /// 解析 0x0605 数据（翻译结果）
        /// </summary>
        /// <param name="data"></param>
        /// <param name="recgResult">成功，翻译结果；false-错误消息</param>
        /// <returns>true-成功；false-失败</returns>
        public bool Analyze0x0605(byte[] data, out string transResult)
        {
            try
            {
                int totalLen = BitConverter.ToInt32(data, 1);  // 包总长度
                int dataLen = totalLen - 7;                    // 数据区长度

                byte flag = Buffer.GetByte(data, 7);           // 成功失败 flag：1-成功，其他-失败
                transResult = Encoding.UTF8.GetString(data, 8, dataLen - 1);

                return flag == 1 ? true : false;
            }
            catch (Exception ex)
            {
                transResult = "解析业务数据 0x0605 失败。";
                Log.WriteLog(transResult + ex.Message);
                return false;
            }
        }

    }
}
