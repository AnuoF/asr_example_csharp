/*********************************************************************************************
 *	
 * 文件名称:    JthsAsr.cs
 *
 * 描    述：   捷通华声语音识别类
 * 
 * 作    者:    Anuo
 *	
 * 创作日期:    2019-2-28
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using AsrLibrary.Entity;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace AsrLibrary.Asr.Jths
{
    /// <summary>
    /// 捷通华声语音识别类
    /// </summary>
    internal class JthsAsr : AsrBase, IDisposable
    {
        /// <summary>
        /// 识别结果
        /// </summary>
        private StringBuilder _sbResult = new StringBuilder();

        /// <summary>
        /// 构造函数
        /// </summary>
        public JthsAsr()
        {
            InitSdk();
        }

        #region AsrBase Implement

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

            _sbResult.Clear();
            string capKey = GetCapKey(languageType);
            if (string.IsNullOrEmpty(capKey))
            {
                recogResult = _errMsg = "捷通华声语音识别不具备该语种的识别能力";
                return false;
            }

            // 初始化 ASR
            bool ret = AsrInit(capKey, null);
            if (ret == false)
            {
                recogResult = _errMsg;
                return false;
            }

            // 启动会话
            string recogConfig = "audioformat=pcm16k16bit,encode=speex";
            int sessionId = -1;
            ret = SessionStart(capKey, recogConfig, ref sessionId);
            if (ret == false)
            {
                recogResult = _errMsg;
                return false;
            }

            // 语音识别
            ret = Recog(audioData, sessionId);
            if (ret)
            {
                recogResult = _sbResult.ToString();
                // 翻译提供独立的接口，不与识别放在一起
            }
            else
            {
                recogResult = _errMsg;
            }

            // 终止 ASR 会话
            hci_api.hci_asr_session_stop(sessionId);
            // ASR 反初始化
            hci_api.hci_asr_release();

            return ret;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                int errCode = hci_api.hci_release();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 初始化 SDK
        /// </summary>
        private void InitSdk()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Utils.configName);

                string appKey = doc.SelectSingleNode("./configuration/engine/jths/appKey").InnerXml;
                string developerKey = doc.SelectSingleNode("./configuration/engine/jths/developerKey").InnerXml;
                string cloudUrl = doc.SelectSingleNode("./configuration/engine/jths/cloudUrl").InnerXml;
                string authPath = doc.SelectSingleNode("./configuration/engine/jths/authPath").InnerXml;
                string logFilePath = doc.SelectSingleNode("./configuration/engine/jths/logFilePath").InnerXml;
                string logLevel = doc.SelectSingleNode("./configuration/engine/jths/logLevel").InnerXml;
                string logFileSize = doc.SelectSingleNode("./configuration/engine/jths/logFileSize").InnerXml;

                string appRootPath = AppDomain.CurrentDomain.BaseDirectory;

                // 如果日志文件路和授权文件路径不存在，则初始化将失败。
                if (!string.IsNullOrEmpty(authPath))
                {
                    authPath = Path.Combine(appRootPath, authPath);
                    if (!Directory.Exists(authPath))
                    {
                        Directory.CreateDirectory(authPath);
                    }
                }
                else
                {
                    authPath = appRootPath;
                }

                if (!string.IsNullOrEmpty(logFilePath))
                {
                    logFilePath = Path.Combine(appRootPath, logFilePath);
                    if (!Directory.Exists(logFilePath))
                    {
                        Directory.CreateDirectory(logFilePath);
                    }
                }
                else
                {
                    logFilePath = appRootPath;
                }

                StringBuilder initConfig = new StringBuilder();
                initConfig.Append("appKey=" + appKey);
                initConfig.Append(",developerKey=" + developerKey);
                initConfig.Append(",cloudUrl=" + cloudUrl);
                initConfig.Append(",authpath=" + authPath);
                initConfig.Append(",logfilepath=" + logFilePath);
                initConfig.Append(",loglevel=" + logLevel);
                initConfig.Append(",logfilesize=" + logFileSize);

                int errCode = hci_api.hci_init(initConfig.ToString());
                if (errCode == (int)HCI_ERR_CODE.HCI_ERR_NONE)
                {
                    _isSdkInit = true;
                }
                else
                {
                    _errMsg = string.Format("hci_init return {0}:{1}", errCode, hci_api.hci_get_error_info(errCode));
                    _isSdkInit = false;
                }
            }
            catch (Exception ex)
            {
                _errMsg = "初始化 Jths SDK 失败。" + ex.Message;
                _isSdkInit = false;
            }
        }

        /// <summary>
        /// ASR 初始化
        /// </summary>
        /// <param name="capKey">能力值</param>
        /// <param name="dataPath">本地资源路径，使用云端能力则传 null</param>
        /// <returns>true-成功；false-失败</returns>
        private bool AsrInit(string capKey, string dataPath)
        {
            string asrInitConfig = "initcapkeys=" + capKey;
            if (!string.IsNullOrEmpty(dataPath))
            {
                asrInitConfig += ",datapath=" + dataPath;
            }

            try
            {
                int errCode = hci_api.hci_asr_init(asrInitConfig);
                if (errCode == (int)HCI_ERR_CODE.HCI_ERR_NONE)
                {
                    return true;
                }
                else
                {
                    _errMsg = string.Format("hci_asr_init return {0}:{1}", errCode, hci_api.hci_get_error_info(errCode));
                    return false;
                }
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 开始 ASR 会话
        /// </summary>
        /// <param name="capKey">能力值</param>
        /// <param name="recogConfig">识别配置</param>
        /// <param name="sessionId">会话ID</param>
        /// <returns>true-成功；false-失败</returns>
        private bool SessionStart(string capKey, string recogConfig, ref int sessionId)
        {
            string sessionConfig = "realtime=yes,capkey=" + capKey + "," + recogConfig;

            try
            {
                int errCode = hci_api.hci_asr_session_start(sessionConfig, ref sessionId);
                if (errCode == (int)HCI_ERR_CODE.HCI_ERR_NONE)
                {
                    return true;
                }
                else
                {
                    _errMsg = string.Format("hci_asr_session_start return {0}:{1}", errCode, hci_api.hci_get_error_info(errCode));
                    return false;
                }
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 语音识别
        /// </summary>
        /// <param name="audioData">音频数据</param>
        /// <param name="sessionId">会话ID</param>
        /// <returns>true-成功；false-失败</returns>
        private bool Recog(byte[] audioData, int sessionId)
        {
            int errCode = 0;
            bool success = false;
            int perLen = 6400;  // 0.2s
            int len = 0;        // 已传入的长度
            ASR_RECOG_RESULT result = new ASR_RECOG_RESULT();

            // 实时识别过程模拟，将待识别的音频数据分为多段，逐段传入识别接口
            // 若某次传输音频数据检测到末端，则跳出循环，终止音频数据传入，以准备获取识别结果
            while (len < audioData.Length)
            {
                // 本次要传入的参与识别的数据长度
                // 剩余的多余 perLen 则传入 perLen 个字节，若不足则传入剩余数据
                int currLen = 0;
                if (audioData.Length - len >= perLen)
                {
                    currLen = perLen;
                }
                else
                {
                    currLen = audioData.Length - len;
                }

                byte[] currData = new byte[currLen];
                Buffer.BlockCopy(audioData, len, currData, 0, currLen);
                errCode = hci_api.hci_asr_recog(sessionId, currData, (uint)currLen, null, null, ref result);
                if (result.uiResultItemCount > 0)
                {
                    GetAsrResult(result);
                    hci_api.hci_asr_free_recog_result(ref result);
                }

                if (errCode == (int)HCI_ERR_CODE.HCI_ERR_ASR_REALTIME_END)
                {
                    errCode = hci_api.hci_asr_recog(sessionId, null, 0, null, null, ref result);
                    if (errCode == (int)HCI_ERR_CODE.HCI_ERR_NONE)
                    {
                        GetAsrResult(result);
                        hci_api.hci_asr_free_recog_result(ref result);
                    }
                    else
                    {
                        _errMsg = string.Format("hci_asr_recog return {0}:{1}", errCode, hci_api.hci_get_error_info(errCode));
                        success = false;
                        break;
                    }
                }
                else if (errCode == (int)HCI_ERR_CODE.HCI_ERR_ASR_REALTIME_WAITING ||
                    errCode == (int)HCI_ERR_CODE.HCI_ERR_ASR_REALTIME_NO_VOICE_INPUT)
                {
                    // 在连续识别的场景，忽略这两个情况，继续识别后面的音频
                    len += currLen;
                }
                else
                {
                    // 识别失败
                    _errMsg = string.Format("hci_asr_recog return {0}:{1}", errCode, hci_api.hci_get_error_info(errCode));
                    success = false;
                    break;
                }

                // TODO:需测试是否需要 Sleep() 方法以提高识别速度
                //  Thread.Sleep(200);   // 模拟真实说话人语速，发送200ms数据后需等待200ms
            }

            // 若检测到端点，但数据已经传入完毕，则需要告诉引擎数据输入完毕
            // 或者检测到末端了，也需要告诉引擎，获取结果
            if (errCode == (int)HCI_ERR_CODE.HCI_ERR_ASR_REALTIME_WAITING ||
                errCode == (int)HCI_ERR_CODE.HCI_ERR_ASR_REALTIME_END)
            {
                errCode = hci_api.hci_asr_recog(sessionId, null, 0, null, null, ref result);
                if (errCode == (int)HCI_ERR_CODE.HCI_ERR_NONE)
                {
                    success = true;
                    GetAsrResult(result);
                    hci_api.hci_asr_free_recog_result(ref result);
                }
                else
                {
                    success = false;
                    _errMsg = string.Format("hci_asr_recog return {0}:{1}", errCode, hci_api.hci_get_error_info(errCode));
                }
            }

            return success;
        }

        /// <summary>
        /// 提取识别结果
        /// </summary>
        /// <param name="result">识别结果结构体</param>
        private void GetAsrResult(ASR_RECOG_RESULT result)
        {
            int sizeOfItem = Marshal.SizeOf(typeof(ASR_RECOG_RESULT_ITEM));
            for (int i = 0; i < (int)result.uiResultItemCount; ++i)
            {
                ASR_RECOG_RESULT_ITEM item = (ASR_RECOG_RESULT_ITEM)Marshal.PtrToStructure((IntPtr)(result.psResultItemList.ToInt32() + i * sizeOfItem), typeof(ASR_RECOG_RESULT_ITEM));
                int len = FindNullIndex(item.pszResult, 100);
                byte[] data = new byte[len];
                Marshal.Copy(item.pszResult, data, 0, len);
                _sbResult.Append(Encoding.UTF8.GetString(data));
            }
        }

        /// <summary>
        /// 找到第一个为空的字符串索引
        /// </summary>
        /// <param name="ptr">指针</param>
        /// <param name="length">起始长度</param>
        /// <returns>索引</returns>
        private int FindNullIndex(IntPtr ptr, int length)
        {
            byte[] data = new byte[length];
            Marshal.Copy(ptr, data, 0, length);
            int currLen = 0;

            for (int i = 0; i < data.Length; i++)
            {
                currLen = i;
                if (data[i] == 0)
                {
                    break;
                }
            }

            if (currLen == length - 1)
            {
                return FindNullIndex(ptr, length + 50);
            }
            else
            {
                return currLen;
            }
        }

        /// <summary>
        /// 从语种转到对应的能力值 CapKey
        /// </summary>
        /// <param name="language">语种</param>
        /// <returns>CapKey，如果为空表示为匹配上</returns>
        private string GetCapKey(LanguageType language)
        {
            string capKey = string.Empty;

            switch (language)
            {
                case LanguageType.Amdo:
                    capKey = "asr.cloud.freetalk.amdo";
                    break;
                case LanguageType.Yue:
                    capKey = "asr.cloud.freetalk.cantonese";
                    break;
                case LanguageType.English:
                    capKey = "asr.cloud.freetalk.english";
                    break;
                case LanguageType.Kazak:
                    capKey = "asr.cloud.freetalk.kazak";
                    break;
                case LanguageType.Kham:
                    capKey = "asr.cloud.freetalk.kham";
                    break;
                case LanguageType.Korean:
                    capKey = "asr.cloud.freetalk.korean";
                    break;
                case LanguageType.Minnan:
                    capKey = "asr.cloud.freetalk.minnan";
                    break;
                case LanguageType.Mongolian:
                    capKey = "asr.cloud.freetalk.mongolian";
                    break;
                case LanguageType.Shanghai:
                    capKey = "asr.cloud.freetalk.shanghaihua";
                    break;
                case LanguageType.Sichaun:
                    capKey = "asr.cloud.freetalk.sichuan";
                    break;
                case LanguageType.Taiwan:
                    capKey = "asr.cloud.freetalk.taiwan";
                    break;
                case LanguageType.Utsang:
                    capKey = "asr.cloud.freetalk.utsang";
                    break;
                case LanguageType.Uygur:
                    capKey = "asr.cloud.freetalk.uyghur";
                    break;
                case LanguageType.Yangjiang:
                    capKey = "asr.cloud.freetalk.yangjiang";
                    break;
                case LanguageType.Yi:
                    capKey = "asr.cloud.freetalk.yi";
                    break;
                case LanguageType.Mandarin:
                    capKey = "asr.cloud.freetalk";
                    break;
                case LanguageType.Zhuang:
                    capKey = "asr.cloud.zhuang";
                    break;
                default:
                    capKey = string.Empty;
                    break;
            }

            return capKey;
        }
    }
}
