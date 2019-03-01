using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace AnuoLibrary.Mt
{
    /// <summary>
    /// 中国民族语文翻译局
    /// </summary>
    public class MzywfyjMT
    {
        /// <summary>
        /// 翻译
        /// </summary>
        /// <param name="transText">需要翻译的内容</param>
        /// <param name="from">翻译原语种</param>
        /// <param name="to">翻译后语种</param>
        /// <param name="url">类型</param>
        /// <param name="transResult">翻译成功返回翻译结果，翻译失败返回错误信息</param>
        /// <returns>true-成功；false-失败</returns>
        public static bool Translate(string transText, string from, string to, string url, out string transResult)
        {
            transResult = "";
            string requestUrl = "http://www.mzywfy.org.cn/ajaxservlet";

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
                request.Timeout = 5000;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.109 Safari/537.36";
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.Referer = "http://www.mzywfy.org.cn/translate.jsp";

                string send = string.Format("src_text={0}&from={1}&to={2}&url={3}", transText, from, to, url);
                byte[] postData = Encoding.UTF8.GetBytes(send);
                request.ContentLength = postData.Length;

                Stream stream = request.GetRequestStream();
                stream.Write(postData, 0, postData.Length);
                stream.Close();
                stream.Dispose();

                bool success = false;
                using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                using (StreamReader responseStream = new StreamReader(webResponse.GetResponseStream()))
                {
                    if (webResponse.StatusCode == HttpStatusCode.OK)
                    {
                        success = true;
                        string retJson = responseStream.ReadToEnd();
                        JObject jo = (JObject)JsonConvert.DeserializeObject(retJson);
                        transResult = jo["tgt_text"].ToString().Trim();
                    }
                }

                return success;
            }
            catch (Exception ex)
            {
                transResult = ex.ToString();
                return false;
            }
        }

        /// <summary>
        /// 维语到汉文
        /// </summary>
        /// <param name="transText">待翻译的内容</param>
        /// <param name="transResult">翻译成功返回翻译结果，翻译失败返回错误信息</param>
        /// <returns>true-成功；false-失败</returns>
        public static bool TranslateUY2ZH(string transText, out string transResult)
        {
            return Translate(transText, "uy", "zh", "7", out transResult);
        }

        /// <summary>
        /// 藏文到汉文
        /// </summary>
        /// <param name="transText">待翻译的内容</param>
        /// <param name="transResult">翻译成功返回翻译结果，翻译失败返回错误信息</param>
        /// <returns>true-成功；false-失败</returns>
        public static bool TranslateTI2ZH(string transText, out string transResult)
        {
            return Translate(transText, "ti", "zh", "6", out transResult);
        }

        /// <summary>
        /// 蒙文到汉文
        /// </summary>
        /// <param name="transText">待翻译的内容</param>
        /// <param name="transResult">翻译成功返回翻译结果，翻译失败返回错误信息</param>
        /// <returns>true-成功；false-失败</returns>
        public static bool TranslateMO2ZH(string transText, out string transResult)
        {
            return Translate(transText, "mo", "zh", "5", out transResult);
        }

    }
}
