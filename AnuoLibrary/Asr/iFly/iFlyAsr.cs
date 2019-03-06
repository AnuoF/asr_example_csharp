/*********************************************************************************************
 *	
 * 文件名称:    iFlyAsr.cs
 *
 * 描    述：   讯飞语音识别类
 * 
 * 作    者:    Anuo
 *	
 * 创作日期:    2019-2-28
 *
 * 备    注:	已集成英语、普通话、粤语和四川话的识别，由于其他语种的字符没有在官网上查询到，
 *              如果需要集成进来，则要询问讯飞的技术人员。
 *                                        
*********************************************************************************************/

using AnuoLibrary.Entity;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml;

namespace AnuoLibrary.Asr.iFly
{
    /// <summary>
    /// 讯飞语音识别类
    /// </summary>
    internal class iFlyAsr : AsrBase
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public iFlyAsr()
        {
            SdkInit();
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

            // 开启会话
            IntPtr sessionId = IntPtr.Zero;
            bool ret = SessionBegin(languageType, ref sessionId);
            if (ret == false)
            {
                recogResult = _errMsg;
                return false;
            }

            // 识别
            ret = Recog(audioData, sessionId, out recogResult);

            // 结束会话
            SessionEnd(sessionId);

            return ret;
        }

        /// <summary>
        /// 开启会话
        /// </summary>
        /// <param name="languageType">语种</param>
        /// <returns>true-成功；false-失败</returns>
        private bool SessionBegin(LanguageType languageType, ref IntPtr sessionId)
        {
            // 根据语种类型组合会话启动参数
            string sessionBeginParams = CombineSessionParams(languageType);
            if (string.IsNullOrEmpty(sessionBeginParams))
            {
                _errMsg = "讯飞 SDK 不支持该语种的识别。";
                return false;
            }

            int errCode = 0;
            sessionId = msc_api.QISRSessionBegin(null, sessionBeginParams, ref errCode);
            if (errCode != (int)Errors.MSP_SUCCESS)
            {
                _errMsg = string.Format("QISRSessionBegin failed! error code:{0}", errCode);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 结束会话
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        private void SessionEnd(IntPtr sessionId)
        {
            try
            {
                msc_api.QISRSessionEnd(sessionId, "stop session");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 语音识别
        /// </summary>
        /// <param name="audioData">音频数据</param>
        /// <param name="recogResult">成功返回识别结果，失败返回错误消息</param>
        /// <returns>true-成功；false-失败</returns>
        private bool Recog(byte[] audioData, IntPtr sessionId, out string recogResult)
        {
            // 每次写入 200ms 音频(16k，16bit)：1 帧音频20ms，10帧 = 200ms。
            // 16k 采样率的16位音频，一帧的大小为 640Byte
            uint len = 10 * 640;                 // 每次传入的长度
            long totalLen = audioData.Length;    // 音频总长
            long sendLen = 0;                    // 已传入的长度

            var aud_stat = AudioStatus.MSP_AUDIO_SAMPLE_CONTINUE;   // 音频状态
            var ep_stat = EpStatus.MSP_EP_AFTER_SPEECH;             // 端点检测
            var rec_stat = RecogStatus.MSP_REC_STATUS_SUCCESS;      // 识别状态
            int errCode = (int)Errors.MSP_SUCCESS;                  // 错误码
            StringBuilder sb = new StringBuilder();

            while (true)
            {
                if (totalLen - sendLen <= len)
                {
                    len = (uint)(totalLen - sendLen);
                }

                if (sendLen >= totalLen)
                {
                    break;  // 数据已提取完成
                }

                aud_stat = AudioStatus.MSP_AUDIO_SAMPLE_CONTINUE;
                if (sendLen == 0)
                {
                    aud_stat = AudioStatus.MSP_AUDIO_SAMPLE_FIRST;
                }

                byte[] tempData = new byte[len];
                Buffer.BlockCopy(audioData, (int)sendLen, tempData, 0, (int)len);
                errCode = msc_api.QISRAudioWrite(sessionId, tempData, len, aud_stat, ref ep_stat, ref rec_stat);
                if (errCode != (int)Errors.MSP_SUCCESS)
                {
                    recogResult = string.Format("QISRAudioWrite failed! error code:", errCode);
                    return false;
                }

                sendLen += len;

                if (rec_stat == RecogStatus.MSP_REC_STATUS_SUCCESS)   // 已有部分听写结果
                {
                    var ptr = msc_api.QISRGetResult(sessionId, ref rec_stat, 0, ref errCode);
                    if (errCode != (int)Errors.MSP_SUCCESS)
                    {
                        recogResult = string.Format("QISRGetResult failed! error code:", errCode);
                        return false;
                    }

                    string result = Marshal.PtrToStringAnsi(ptr);
                    if (!string.IsNullOrEmpty(result))
                    {
                        sb.Append(result);
                    }
                }

                if (ep_stat == EpStatus.MSP_EP_AFTER_SPEECH)
                {
                    break;
                }

                // TODO:需测试验证，是否需要 Sleep()
                // 模拟人说话时间间隙，200ms对应10帧的音频
                //   Thread.Sleep(200);
            }

            errCode = msc_api.QISRAudioWrite(sessionId, null, 0, AudioStatus.MSP_AUDIO_SAMPLE_LAST, ref ep_stat, ref rec_stat);
            if (errCode != (int)Errors.MSP_SUCCESS)
            {
                recogResult = string.Format("QISRAudioWrite failed! error code:", errCode);
                return false;
            }

            while (rec_stat != RecogStatus.MSP_REC_STATUS_COMPLETE)
            {
                IntPtr ptr = msc_api.QISRGetResult(sessionId, ref rec_stat, 5000, ref errCode);
                if (errCode != (int)Errors.MSP_SUCCESS)
                {
                    recogResult = string.Format("QISRGetResult failed! error code:", errCode);
                    return false;
                }

                string result = Marshal.PtrToStringAnsi(ptr);
                if (!string.IsNullOrEmpty(result))
                {
                    sb.Append(result);
                }

                Thread.Sleep(1);   // 防止频繁占用内存
            }

            recogResult = sb.ToString();
            return true;
        }

        /// <summary>
        /// 初始化 SDK
        /// </summary>
        private void SdkInit()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Utils.configName);

                string appId = doc.SelectSingleNode("./configuration/engine/ifly/appId").InnerXml;
                appId = string.Format("appid = {0}, work_dir = .", appId);
                int errCode = msc_api.MSPLogin(null, null, appId);

                if (errCode == (int)Errors.MSP_SUCCESS)
                {
                    _isSdkInit = true;
                }
                else
                {
                    _errMsg = string.Format("MSPLogin failed! error code:{0}", errCode);
                    _isSdkInit = false;
                }
            }
            catch (Exception ex)
            {
                _errMsg = "初始化讯飞 SDK 异常：" + ex.Message;
                _isSdkInit = false;
            }
        }

        /// <summary>
        /// 将语种转换为讯飞的 Accent
        /// </summary>
        /// <param name="languageType">语种类型</param>
        /// <returns>会话参数，如果参数 null 表示讯飞不具备该语种的识别能力。</returns>
        private string CombineSessionParams(LanguageType languageType)
        {
            /*
            * sub:				请求业务类型
            * domain:			领域
            * language:			语言  zh_cn：简体中文  en_us：英文
            * accent:			方言
            * sample_rate:		音频采样率
            * result_type:		识别结果格式
            * result_encoding:	结果编码格式
            */
            string sessionBeginParams = "sub = iat, domain = iat, language = zh_cn, accent = {0}, sample_rate = 16000, result_type = plain, result_encoding =gb2312";

            switch (languageType)
            {
                case LanguageType.English:
                    sessionBeginParams = "sub = iat, domain = iat, language = en_us, accent = mandarin, sample_rate = 16000, result_type = plain, result_encoding =gb2312";
                    break;
                case LanguageType.Mandarin:
                    sessionBeginParams = string.Format(sessionBeginParams, "mandarin");
                    break;
                case LanguageType.Yue:
                    sessionBeginParams = string.Format(sessionBeginParams, "cantonese");
                    break;
                case LanguageType.Sichaun:
                    sessionBeginParams = string.Format(sessionBeginParams, "lmz");
                    break;

                // TODO:以下情况的具体字段尚未明确，需询问讯飞技术人员。
                //case LanguageType.Dongbei:
                //    break;
                //case LanguageType.Henan:
                //    break;
                //case LanguageType.Tianjin:
                //    break;
                //case LanguageType.Shandong:
                //    break;
                //case LanguageType.Guizhou:
                //    break;
                //case LanguageType.Ningxia:
                //    break;
                //case LanguageType.Yunnan:
                //    break;
                //case LanguageType.Shanxi:
                //    break;
                //case LanguageType.Gansu:
                //    break;
                //case LanguageType.Wuhan:
                //    break;
                //case LanguageType.Hebei:
                //    break;
                //case LanguageType.Hefei:
                //    break;
                //case LanguageType.Changsha:
                //    break;
                //case LanguageType.Shanghai:
                //    break;
                //case LanguageType.Taiyuan:
                //    break;

                default:
                    sessionBeginParams = string.Empty;
                    break;
            }

            return sessionBeginParams;
        }

    }
}
