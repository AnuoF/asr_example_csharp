/*********************************************************************************************
 *	
 * 文件名称:    JthsTrans.cs
 * 
 * 描    述：   捷通华声翻译实现类。
 *
 * 作    者:    Anuo
 *	
 * 创作日期:    2019-3-4
 *
 * 备    注:	捷通华声目前仅支持 英语和维语
 *                                        
*********************************************************************************************/

using AnuoLibrary.Entity;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;

namespace AnuoLibrary.Mt
{
    /// <summary>
    /// 捷通华声翻译
    /// </summary>
    internal class JthsTrans : TransBase
    {
        private string _cloudUrl = "http://api.hcicloud.com:8880/mt/Translate";
        private string _appKey = "";
        private string _developerKey = "";
        private string _capKey = "mt.cloud.uy2cn"; //"mt.cloud.translate.uy2cn";
        private string _sdkVersion = "8.1";     // 8.1    5.0
        private string _udid = "101:1234567890";
        private string _splitsentence = "yes";
        private string _taskConfig = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        public JthsTrans()
        {
            LoadApiInfo();

            //capkey = mt.cloud.translate，property = uy2cn
            //taskConfig = "capkey=mt.cloud.translate,property=uy2cn";
            _taskConfig = "capkey=" + _capKey + ",splitsentenc" + _splitsentence;
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
            // 捷通华声目前仅支持 英语和维语
            _capKey = string.Empty;
            result = string.Empty;

            switch (from)
            {
                case LanguageType.English:
                    _capKey = "mt.cloud.en2cn";
                    break;

                case LanguageType.Uygur:
                    _capKey = "mt.cloud.uy2cn";
                    break;
            }

            if (string.IsNullOrEmpty(_capKey))
            {
                result = "捷通华声API暂不支持该语种的翻译。";
                return false;
            }

            return Translate(text, out result);
        }

        /// <summary>
        /// 翻译
        /// </summary>
        /// <param name="transText">待翻译的内容</param>
        /// <param name="transResult">成功返回翻译结果，失败返回错误消息</param>
        /// <returns>true-成功；false-失败</returns>
        private bool Translate(string transText, out string transResult)
        {
            try
            {
                string currTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(_cloudUrl);
                request.Method = "POST";
                request.Timeout = 5000;
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";

                request.Headers.Add("x-app-key", _appKey);
                request.Headers.Add("x-sdk-version", _sdkVersion);
                request.Headers.Add("x-request-date", currTime);
                request.Headers.Add("x-task-config", _taskConfig);
                request.Headers.Add("x-session-key", getSessionKey(currTime));
                request.Headers.Add("x-result-format", "xml");
                request.Headers.Add("x-udid", _udid);

                byte[] postData = Encoding.UTF8.GetBytes(transText);
                request.ContentLength = postData.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(postData, 0, postData.Length);
                stream.Close();

                using (WebResponse webResponse = request.GetResponse())
                using (StreamReader responseStream = new StreamReader(webResponse.GetResponseStream()))
                {
                    string retXml = responseStream.ReadToEnd();
                    Trace.WriteLine(DateTime.Now.ToString() + retXml);
                    return getResult(retXml, out transResult);
                }
            }
            catch (Exception ex)
            {
                transResult = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Session + Key  MD5
        /// </summary>
        /// <param name="currTime">当前时间字符串</param>
        /// <returns>md5字符串</returns>
        private string getSessionKey(string currTime)
        {
            return MD5Helper.getXSessionKey(currTime, _developerKey);
        }

        /// <summary>
        /// 提取结果
        /// </summary>
        /// <param name="retXml">返回的XML字符串</param>
        /// <param name="transResult">成功返回翻译结果，失败返回错误消息</param>
        /// <returns>true-翻译成功；false-翻译失败</returns>
        private bool getResult(string retXml, out string transResult)
        {
            transResult = null;
            if (string.IsNullOrEmpty(retXml))
            {
                return false;
            }

            ResponseInfo respInfo = new ResponseInfo(retXml);
            if (respInfo.ErrorNo == "Success")
            {
                transResult = respInfo.ResultText;
                return true;
            }
            else
            {
                transResult = respInfo.ResMessage;
                return false;
            }
        }

        /// <summary>
        /// 加载API信息
        /// </summary>
        private void LoadApiInfo()
        {
            try
            {
                string configPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "AnuoLibrary.config");
                XmlDocument doc = new XmlDocument();
                doc.Load(configPath);

                _appKey = doc.SelectSingleNode("./configuration/engine/jths/appKey").InnerXml;
                _developerKey = doc.SelectSingleNode("./configuration/engine/jths/developerKey").InnerXml;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }
    }

    public class ResponseInfo
    {
        public string ResCode;

        public string ResMessage;

        public string Result_Token;

        public string ResultText;

        public string Score;

        public string ErrorNo = "";


        public ResponseInfo()
        {

        }

        public ResponseInfo(string retXml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(retXml);

            ResCode = doc.SelectSingleNode("./ResponseInfo/ResCode").InnerText;
            if (ResCode == "Success")
            {
                ResMessage = doc.SelectSingleNode("./ResponseInfo/ResMessage").InnerText;
                //Result_Token = doc.SelectSingleNode("./ResponseInfo/Result_Token").InnerText;
                ResultText = doc.SelectSingleNode("./ResponseInfo/ResultText").InnerText;
                Score = doc.SelectSingleNode("./ResponseInfo/Score").InnerText;
            }
            else
            {
                ResMessage = doc.SelectSingleNode("./ResponseInfo/ResMessage").InnerText;
                ErrorNo = doc.SelectSingleNode("./ResponseInfo/ErrorNo").InnerText;
                //Result_Token = doc.SelectSingleNode("./ResponseInfo/Result_Token").InnerText;
            }
        }
    }
}
