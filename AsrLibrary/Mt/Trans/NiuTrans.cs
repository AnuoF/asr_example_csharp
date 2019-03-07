/*********************************************************************************************
 *	
 * 文件名称:    NiuTrans.cs
 * 
 * 描    述：   小牛翻译实现类。
 *
 * 作    者:    Anuo
 *	
 * 创作日期:    2019-3-4
 *
 * 备    注:	小牛翻译支持语种：英语、粤语、藏语、蒙语、韩语等，以及大部分国外语种。
 *              目前仅集成了英语、藏语、蒙语、韩语，其他语言需要时再集成进来。 
 *                                        
*********************************************************************************************/

using AsrLibrary.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace AsrLibrary.Mt
{
    /// <summary>
    /// 小牛翻译实现类
    /// </summary>
    internal class NiuTrans : TransBase
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public NiuTrans()
        {

        }

        /// <summary>
        /// 翻译
        /// </summary>
        /// <param name="text">待翻译的内容</param>
        /// <param name="from">翻译源语种</param>
        /// <param name="to">翻译目的语种</param>
        /// <param name="result">成功返回翻译结果，失败返回错误消息</param>
        /// <returns>true-成功；false-失败</returns>
        public override bool Translate(string text, LanguageType from, LanguageType to, out string result)
        {
            try
            {
                result = string.Empty;
                string fromStr = GetCharFromLanguageType(from);

                if (string.IsNullOrEmpty(fromStr))
                {
                    result = "小牛翻译，暂未集成该语种的翻译。";
                    return false;
                }

                string url = "https://test.niutrans.vip/NiuTransServer/testtrans?&from=en&to=zh&m=0.7651222916705216&src_text=hello&url=";

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = 5000;
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.119 Safari/537.36";
                request.Referer = "https://niutrans.vip/";

                string sendText = string.Format("from={0}&to={1}&m={2}&src_text={3}&url=", fromStr, "zh", "0.9837005488627406", text);
                byte[] data = Encoding.UTF8.GetBytes(sendText);

                request.ContentLength = data.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

                bool success = false;
                using (WebResponse webResponse = request.GetResponse())
                using (StreamReader responseStream = new StreamReader(webResponse.GetResponseStream()))
                {
                    success = true;
                    string retXml = responseStream.ReadToEnd();     // {"from":"en","to":"zh","tgt_text":"你好 \n"}
                    JObject jo = (JObject)JsonConvert.DeserializeObject(retXml);
                    result = jo["tgt_text"].ToString().Trim();
                }

                return success;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 语种映射到字符串
        /// </summary>
        /// <param name="type">语种类型</param>
        /// <returns>字符串</returns>
        private string GetCharFromLanguageType(LanguageType type)
        {
            // 暂时集成：英语、藏语、蒙语、韩语，其他语言需要时再集成进来。
            string ch = string.Empty;

            switch (type)
            {
                case LanguageType.English:
                    ch = "en";
                    break;

                case LanguageType.Utsang:
                    ch = "ti";
                    break;

                case LanguageType.Mongolian:
                    ch = "mn";
                    break;

                case LanguageType.Korean:
                    ch = "ko";
                    break;
            }

            return ch;
        }
    }
}
