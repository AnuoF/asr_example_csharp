/*********************************************************************************************
 *	
 * 文件名称:    BaiduAsr.cs
 *
 * 描    述：   百度语音识别类
 * 
 * 作    者:    Anuo
 *	
 * 创作日期:    2019-2-28
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using AnuoLibrary.Entity;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Xml;

namespace AnuoLibrary.Asr.Baidu
{
    /// <summary>
    /// 百度语音识别类
    /// </summary>
    internal class BaiduAsr : AsrBase
    {
        /// <summary>
        /// APP_ID
        /// </summary>
        private string _appId = string.Empty;
        /// <summary>
        /// API_KEY
        /// </summary>
        private string _apiKey = string.Empty;
        /// <summary>
        /// SCERET_KEY
        /// </summary>
        private string _sceretKey = string.Empty;
        /// <summary>
        /// 百度语音识别
        /// </summary>
        private global::Baidu.Aip.Speech.Asr _client = null;


        /// <summary>
        /// 构造函数：完成 SDK 的初始化
        /// </summary>
        public BaiduAsr()
        {
            InitSdk();
        }

        /// <summary>
        /// 语音识别
        /// </summary>
        /// <param name="audioData">小于 60s 的音频数据，音频格式要求：pcm/16k/16位/单通道 。</param>
        /// <param name="languageType">音频语种类型</param>
        /// <param name="recogResult">识别成功返回识别结果，识别失败返回错误消息</param>
        /// <returns>识别成功或失败，true-成功；false-失败</returns>
        public override bool AudioRecog(byte[] audioData, LanguageType languageType, out string recogResult)
        {
            if (_isSdkInit == false)
            {
                recogResult = _errMsg;
                return false;
            }

            int pid = GetPID(languageType);
            if (pid == -1)
            {
                recogResult = _errMsg = "百度语音识别不具备该语种的识别能力";
                return false;
            }

            // 可选参数
            var options = new Dictionary<string, object> { { "dev_pid", pid } };
            JObject jsonObj = _client.Recognize(audioData, "pcm", 16000, options);
            int err_no = jsonObj.Value<int>("err_no");
            string err_msg = jsonObj.Value<string>("err_msg");

            if (err_no == 0)
            {
                JArray jList = JArray.Parse(jsonObj["result"].ToString());
                recogResult = jList[0].ToString();
                return true;
            }
            else
            {
                _errMsg = recogResult = err_msg;
                return false;
            }
        }

        /// <summary>
        /// 初始化Baidu SDK
        /// </summary>
        private void InitSdk()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Utils.configName);

                _appId = doc.SelectSingleNode("./configuration/engine/baidu/appId").InnerXml;
                _apiKey = doc.SelectSingleNode("./configuration/engine/baidu/apiKey").InnerXml;
                _sceretKey = doc.SelectSingleNode("./configuration/engine/baidu/secretKey").InnerXml;

                _client = new global::Baidu.Aip.Speech.Asr(_apiKey, _sceretKey);
                _client.Timeout = 60000;   // 超时时间

                _isSdkInit = true;
            }
            catch
            {
                _isSdkInit = false;
                _errMsg = "初始化 Baidu SDK 失败：读取配置信息时报错。";
            }
        }

        /// <summary>
        /// 将语种装换为百度对应的PID
        /// </summary>
        /// <param name="language">语种类型</param>
        /// <returns>PID</returns>
        private int GetPID(LanguageType language)
        {
            // 1536 - 普通话(支持简单的英文识别)；
            // 1537 - 普通话(纯中文识别)；
            // 1737 - 英语；
            // 1637 - 粤语；
            // 1837 - 四川话；
            // 1936 - 普通话远场；
            int pid = -1;

            switch (language)
            {
                case LanguageType.Mandarin:
                    pid = 1536;
                    break;
                case LanguageType.English:
                    pid = 1737;
                    break;
                case LanguageType.Yue:
                    pid = 1637;
                    break;
                case LanguageType.Sichaun:
                    pid = 1837;
                    break;
                default:
                    pid = -1;
                    break;
            }

            return pid;
        }
    }
}
