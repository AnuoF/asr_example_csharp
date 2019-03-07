/*********************************************************************************************
 *	
 * 文件名称:    hci_api.cs
 *
 * 描    述：   捷通华声语音识别 API
 * 
 * 作    者:    Anuo
 *	
 * 创作日期:    2019-2-28
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using System;
using System.Runtime.InteropServices;

namespace AsrLibrary.Asr.Jths
{
    /// <summary>
    /// 捷通华声的 API
    /// </summary>
    internal class hci_api
    {
        [DllImport("hci_sys.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int hci_init(string initConfig);

        [DllImport("hci_sys.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern string hci_get_error_info(int errorCode);

        [DllImport("hci_sys.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int hci_release();

        [DllImport("hci_asr.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int hci_asr_init(string asrInitConfig);

        [DllImport("hci_asr.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void hci_asr_release();

        [DllImport("hci_asr.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int hci_asr_session_start(string config, ref int sessionId);

        [DllImport("hci_asr.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int hci_asr_recog(int sessionId, byte[] data, uint dataLen, string congfig, string grammarData, ref ASR_RECOG_RESULT recogResult);

        [DllImport("hci_asr.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int hci_asr_free_recog_result(ref ASR_RECOG_RESULT recogResult);

        [DllImport("hci_asr.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int hci_asr_session_stop(int sessionId);


        [DllImport("ASRCommonTool.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int test_sum(int a, int b);

        [DllImport("ASRCommonTool.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int UTF8ToGBK(IntPtr utfStr, ref IntPtr gbkStr);

        [DllImport("ASRCommonTool.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int GBKToUTF8(IntPtr pGBKStr, ref IntPtr pUTF8Str);

        [DllImport("ASRCommonTool.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int UTF8Length(IntPtr utfStr);

        [DllImport("ASRCommonTool.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr U8ToUnicode(IntPtr szU8);

        [DllImport("ASRCommonTool.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Unicode2Utf8(IntPtr unicode, IntPtr utf8, int nBuffSize);

        [DllImport("hci_mt.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int hci_get_auth_expire_time(ref long expire_time);

        [DllImport("hci_mt.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int hci_check_auth();

        [DllImport("hci_mt.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int hci_mt_init(string initConfig);

        [DllImport("hci_mt.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int hci_mt_session_start(string pszSessionConfig, ref int sessionId);

        [DllImport("hci_mt.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int hci_mt_session_stop(int sessionId);

        //[DllImport("hci_mt.dll", CallingConvention = CallingConvention.StdCall)]
        //public static extern int hci_mt_trans(int sessionId, string transText, string pszRecogConfig, ref MT_TRANS_RESULT mtResult);

        //[DllImport("hci_mt.dll", CallingConvention = CallingConvention.StdCall)]
        //public static extern int hci_mt_free_trans_result(ref MT_TRANS_RESULT mrResult);
    }

    /// <summary>
    /// 返回错误码定义
    /// </summary>
    public enum HCI_ERR_CODE
    {
        HCI_ERR_UNKNOWN = -1,                   ///< -1: 未知错误，通常不会出现
        HCI_ERR_NONE = 0,                       ///< 0: 正确
        HCI_ERR_PARAM_INVALID,                  ///< 1: 函数的传入参数错误
        HCI_ERR_OUT_OF_MEMORY,                  ///< 2: 申请内存失败
        HCI_ERR_CONFIG_INVALID,                 ///< 3: 配置串参数错误
        HCI_ERR_CONFIG_CAPKEY_MISSING,          ///< 4: 缺少必需的capKey配置项
        HCI_ERR_CONFIG_CAPKEY_NOT_MATCH,        ///< 5: CAPKEY与当前引擎不匹配
        HCI_ERR_CONFIG_DATAPATH_MISSING,        ///< 6: 缺少必需的dataPath配置项
        HCI_ERR_CONFIG_UNSUPPORT,               ///< 7: 配置项不支持
        HCI_ERR_SERVICE_CONNECT_FAILED,         ///< 8: 连接服务器失败，服务器无响应
        HCI_ERR_SERVICE_TIMEOUT,                ///< 9: 服务器访问超时
        HCI_ERR_SERVICE_DATA_INVALID,           ///< 10: 服务器返回的数据格式不正确
        HCI_ERR_SERVICE_RESPONSE_FAILED,        ///< 11: 服务器返回操作失败
        HCI_ERR_CAPKEY_NOT_FOUND,               ///< 12: 没有找到指定的能力
        HCI_ERR_NOT_LOCAL_CAPKEY,               ///< 13: 不是本地能力的KEY
        HCI_ERR_LOCAL_LIB_MISSING,              ///< 14: 本地能力引擎缺失必要的库资源
        HCI_ERR_URL_MISSING,                    ///< 15: 找不到对应的网络服务地址（可能是HCI能力服务地址，下载资源库地址等）
        HCI_ERR_SESSION_INVALID,                ///< 16: 无效的会话
        HCI_ERR_TOO_MANY_SESSION,               ///< 17: 开启会话过多(目前每种HCI能力的最大会话数为256)
        HCI_ERR_ACTIVE_SESSION_EXIST,           ///< 18: 还有会话没有停止
        HCI_ERR_START_LOG_FAILED,               ///< 19: 启动日志错误, 可能是日志配置参数错误，路径不存在或者没有写权限等造成
        HCI_ERR_DATA_SIZE_TOO_LARGE,            ///< 20: 传入的数据量超过可处理的上限
        HCI_ERR_LOAD_CODEC_DLL,                 ///< 21: 加载codec编码库失败
        HCI_ERR_UNSUPPORT,                      ///< 22: 暂不支持
        HCI_ERR_LOAD_FUNCTION_FROM_DLL,         ///< 23: 加载库失败
        HCI_ERR_TXACCOUNT_NOT_FOUND,            ///< 24: 天行账号获取失败

        //HCI_SYS 
        HCI_ERR_SYS_NOT_INIT = 100,             ///< 100: HCI_SYS未初始化
        HCI_ERR_SYS_ALREADY_INIT,               ///< 101: HCI_SYS多次初始化
        HCI_ERR_SYS_CONFIG_AUTHPATH_MISSING,    ///< 102: 缺少必需的authPath配置项
        HCI_ERR_SYS_CONFIG_CLOUDURL_MISSING,    ///< 103: 缺少必需的cloudUrl配置项
        HCI_ERR_SYS_CONFIG_USERID_MISSING,      ///< 104: 缺少必需的userId配置项
        HCI_ERR_SYS_CONFIG_PASSWORD_MISSING,    ///< 105: 缺少必需的password配置项
        HCI_ERR_SYS_CONFIG_PLATFORMID_MISSING,  ///< 106: 缺少必需的platformId配置项
        HCI_ERR_SYS_CONFIG_DEVELOPERID_MISSING, ///< 107: 缺少必需的developerId配置项
        HCI_ERR_SYS_CONFIG_DEVELOPERKEY_MISSING,///< 108: 缺少必需的developerKey配置项
        HCI_ERR_SYS_CONFIG_APPNO_MISSING,       ///< 109: 缺少必需的appNo配置项
        HCI_ERR_SYS_USERINFO_INVALID,           ///< 110: 读写用户信息文件错误
        HCI_ERR_SYS_AUTHFILE_INVALID,           ///< 111: 读取授权文件错误
        HCI_ERR_SYS_CHECKAUTH_RESPONSE_FAILED,  ///< 112: 服务器返回获取云端授权失败
        HCI_ERR_SYS_REGISTER_RESPONSE_FAILED,   ///< 113: 服务器返回注册用户失败（已废弃，废弃版本3.0）
        HCI_ERR_SYS_USING,                      ///< 114: 仍然有能力在使用（尚未反初始化）
        HCI_ERR_SYS_CONFIG_APPKEY_MISSING,      ///< 115: 缺少必需的appkey配置项

        /* HCI_ASR */
        HCI_ERR_ASR_NOT_INIT = 200,             ///< 200: HCI_ASR没有初始化
        HCI_ERR_ASR_ALREADY_INIT,               ///< 201: HCI_ASR多次初始化
        HCI_ERR_ASR_CONFIRM_NO_TASK,            ///< 202: 使用confirm，但没有确认任务
        HCI_ERR_ASR_PARAM_CHECK_FLAG_INVALID,   ///< 203: 错误的CheckFlag项，例如已经检测到端点仍然再发送数据，或尚未开启端点检测就发送flag为（CHECK_FLAG_END，CHECK_FLAG_PROGRESS）
        HCI_ERR_ASR_GRAMMAR_DATA_TOO_LARGE,     ///< 204: 语法数据太大
        HCI_ERR_ASR_ENGINE_NOT_INIT,            ///< 205: ASR本地引擎尚未初始化(已废弃，废弃版本3.8)
        HCI_ERR_ASR_ENGINE_INIT_FAILED,         ///< 206: ASR本地引擎初始化失败
        HCI_ERR_ASR_OPEN_GRAMMAR_FILE,          ///< 207: 读取语法文件失败
        HCI_ERR_ASR_LOAD_GRAMMAR_FAILED,        ///< 208: 加载语法文件失败
        HCI_ERR_ASR_ENGINE_FAILED,              ///< 209: ASR本地引擎识别失败
        HCI_ERR_ASR_GRAMMAR_ID_INVALID,         ///< 210: 语法ID无效
        HCI_ERR_ASR_REALTIME_WAITING,           ///< 211: 实时识别时未检测到音频末端，继续等待数据
        HCI_ERR_ASR_GRAMMAR_OVERLOAD,           ///< 212: 加载语法数量已达上限
        HCI_ERR_ASR_GRAMMAR_USING,              ///< 213: 该语法正在使用中
        HCI_ERR_ASR_REALTIME_END,               ///< 214: 实时识别时检测到末端，或者缓冲区满，需要使用NULL获取结果
        HCI_ERR_ASR_UPLOAD_NO_DATA,             ///< 215: 上传本地数据时，无用于上传的数据 
        HCI_ERR_ASR_REALTIME_NO_VOICE_INPUT,    ///< 216: 实时识别时未检测语音
        HCI_ERR_ASR_VOICE_DATA_TOO_LARGE,       ///< 217: 音频片段太长，应在(0,32K)

        /* HCI_HWR */
        HCI_ERR_HWR_NOT_INIT = 300,             ///< 300: HCI_HWR没有初始化
        HCI_ERR_HWR_ALREADY_INIT,               ///< 301: HCI_HWR多次初始化
        HCI_ERR_HWR_CONFIRM_NO_TASK,            ///< 302: 使用confirm，但没有确认任务
        HCI_ERR_HWR_ENGINE_INIT_FAILED,         ///< 303: HWR本地引擎初始化失败
        HCI_ERR_HWR_ENGINE_FAILED,              ///< 304: HWR本地引擎操作（识别、获取拼音、获取联想结果）失败
        HCI_ERR_HWR_UPLOAD_NO_DATA,             ///< 305: 没有可用于上传的数据
        HCI_ERR_HWR_ENGINE_SESSION_START_FAILED,///< 306: HWR本地引擎开启会话失败
        HCI_ERR_HWR_ENGINE_NOT_INIT,            ///< 307: SDK初始化时未传入本地能力却在创建会话时使用了本地能力(已废弃，废弃版本3.8)
        HCI_ERR_HWR_CONFIG_SUBLANG_MISSING,     ///< 308: 单字能力、多语种字典时，未传入sublang
        HCI_ERR_HWR_TOO_MANY_DOMAIN,            ///< 309: 传入了领域数目超过了4个

        /* HCI_OCR */
        HCI_ERR_OCR_NOT_INIT = 400,             ///< 400: HCI_OCR没有初始化
        HCI_ERR_OCR_ALREADY_INIT,               ///< 401: HCI_OCR多次初始化
        HCI_ERR_OCR_ENGINE_INIT_FAILED,         ///< 402: OCR本地引擎初始化失败
        HCI_ERR_OCR_ENGINE_FAILED,              ///< 403: OCR本地引擎操作（倾斜校正、版面分析、识别、预处理、压缩）失败
        HCI_ERR_OCR_ENGINE_NOT_INIT,            ///< 404: SDK初始化时未传入本地能力却在创建会话时使用了本地能力(已废弃，废弃版本3.8)
        HCI_ERR_OCR_LOAD_IMAGE,                 ///< 405: 载入本地文件或者本地图片缓冲失败
        HCI_ERR_OCR_SAVE_IMAGE,                 ///< 406: 保存OCR_IMAGE到本地文件失败
        HCI_ERR_OCR_IMAGE_NOT_SET,              ///< 407: 未设置要处理的图片就进行了倾斜校正、版面分析、识别等操作
        HCI_ERR_OCR_LOAD_TEMPLATE_FAILED,       ///< 408: 加载模板文件失败
        HCI_ERR_OCR_TEMPLATE_OVERLOAD,          ///< 409: 加载模板数量已达上限（1024）
        HCI_ERR_OCR_TEMPLATE_ID_INVALID,        ///< 410: 传入的模板ID不在加载的模板列表中
        HCI_ERR_OCR_TEMPLATE_USING,             ///< 411: 还有会话在使用要卸载的模板
        HCI_ERR_OCR_DETECT_CORNER_FAILED,       ///< 412: OCR获取切边信息失败
        HCI_ERR_OCR_NORMALIZE_FAILED,           ///< 413: OCR切边失败
        HCI_ERR_OCR_RECOGNIZABLE_FAILED,        ///< 414: OCR识别判断返回失败
        HCI_ERR_OCR_IMAGE_NOT_CLEAR,            ///< 415: OCR识别图片不清晰
        HCI_ERR_OCR_CONFIG_TEMPLATE_ID_MISSING, ///< 416: OCR本地模板 templateid参数缺失
        HCI_ERR_OCR_CONFIG_TEMPLATE_INDEX_MISSING,  ///< 417: OCR模板 templateindex参数缺失
        HCI_ERR_OCR_CONFIG_TEMPLATE_PAGE_INDEX_MISSING, ///< 418: OCR模板 templatepageindex参数缺失
        HCI_ERR_OCR_CONFIG_PROPERTY_MISSING,    ///< 419: OCR云端模板 property参数缺失
        HCI_ERR_OCR_EDGE_IS_NOT_EXIST,          ///< 420: OCR识别图片边缘不存在

        /* HCI_TTS */
        HCI_ERR_TTS_NOT_INIT = 500,             ///< 500: HCI_TTS没有初始化
        HCI_ERR_TTS_ALREADY_INIT,               ///< 501: HCI_TTS多次初始化
        HCI_ERR_TTS_SESSION_BUSY,               ///< 502: TTS会话正忙，例如在合成回调函数中又调用了合成
        HCI_ERR_TTS_ENGINE_SESSION_START_FAILED,///< 503: TTS本地引擎开启会话失败
        HCI_ERR_TTS_ENGINE_FAILED,              ///< 504: TTS本地引擎合成失败
        HCI_ERR_TTS_ENGINE_INIT_FAILED,         ///< 505: TTS(NU)本地引擎初始化失败
        HCI_ERR_TTS_ENGINE_NOT_INIT,            ///< 506: TTS(NU)本地引擎尚未初始化
        HCI_ERR_TTS_CONFIG_PROPERTY_MISSING,    ///< 507: TTS 云端property参数缺失

        /* HCI_MT */
        HCI_ERR_MT_NOT_INIT = 600,              ///< 600: HCI_MT没有初始化
        HCI_ERR_MT_ALREADY_INIT,                ///< 601: HCI_MT多次初始化

        /*HCI_NLU*/
        HCI_ERR_NLU_NOT_INIT = 700,             ///< 700: HCI_NLU没有初始化
        HCI_ERR_NLU_ALREADY_INIT,               ///< 701: HCI_NLU多次初始化
        HCI_ERR_NLU_ENGINE_SESSION_START_FAILED,///< 702: NLU本地引擎开启会话失败
        HCI_ERR_NLU_ENGINE_FAILED,              ///< 703: NLU本地引擎识别失败

        /*HCI_KB*/
        HCI_ERR_KB_NOT_INIT = 800,              ///< 800: HCI_KB没有初始化
        HCI_ERR_KB_ALREADY_INIT,                ///< 801: HCI_KB多次初始化
        HCI_ERR_KB_ENGINE_SESSION_START_FAILED, ///< 802: KB本地引擎开启会话失败
        HCI_ERR_KB_ENGINE_FAILED,               ///< 803: KB本地引擎识别失败
        HCI_ERR_KB_SYLLABLE_INVALID,            ///< 804: 容错音节无法判断类型
        HCI_ERR_KB_UDB_WORD_EXIST,              ///< 805: 已经在用户词库中存在
        HCI_ERR_KB_CONFIRM_NO_TASK,             ///< 806: 使用confirm，但没有确认任务

        /*HCI_VPR*/
        HCI_ERR_VPR_NOT_INIT = 900,             ///< 900: HCI_VPR没有初始化
        HCI_ERR_VPR_ALREADY_INIT,               ///< 901: HCI_VPR多次初始化
        HCI_ERR_VPR_ENGINE_INIT_FAILED,         ///< 902: VPR本地引擎初始化失败
        HCI_ERR_VPR_ENGINE_FAILED,              ///< 903: VPR本地引擎处理失败
        HCI_ERR_VPR_USERID_NOT_EXIST,           ///< 904: VPR用户不存在
        HCI_ERR_VPR_ENGINE_SESSION_START_FAILED,///< 905: VPR本地引擎开启会话失败
        HCI_ERR_VPR_CONFIG_USER_ID_MISSING,     ///< 906: VPR缺少userId配置项
        HCI_ERR_VPR_CONFIG_GROUP_ID_MISSING,    ///< 907: VPR缺少groupId配置项
        HCI_ERR_VPR_REALTIME_WAITING,           ///< 908: 实时识别检测到末端，等待
        HCI_ERR_VPR_VOICE_SHORT,                ///< 909: 语音太短
        HCI_ERR_VPR_VOLUMN_HIGH,                ///< 910: 音量太高
        HCI_ERR_VPR_VOLUMN_LOW,                 ///< 911: 音量太低
        HCI_ERR_VPR_NOISE_HIGH,                 ///< 912: 噪音太大
        HCI_ERR_VPR_VAD_DETECT_RESULT_IS_EMPTY, ///< 913: 端点检测结果为空


        /*HCI_FPR*/
        HCI_ERR_FPR_NOT_INIT = 1000,            ///< 1000: HCI_FPR没有初始化
        HCI_ERR_FPR_ALREADY_INIT,               ///< 1001: HCI_FPR多次初始化
        HCI_ERR_FPR_ENGINE_INIT_FAILED,         ///< 1002: FPR本地引擎初始化失败
        HCI_ERR_FPR_ENGINE_FAILED,              ///< 1003: FPR本地引擎处理失败
        HCI_ERR_FPR_USERID_NOT_EXIST,           ///< 1004: FPR用户不存在
        HCI_ERR_FPR_ENGINE_SESSION_START_FAILED,///< 1005: FPR本地引擎开启会话失败
        HCI_ERR_FPR_CONFIG_USER_ID_MISSING,     ///< 1006: FPR缺少userId配置项
        HCI_ERR_FPR_CONFIG_DPI_MISSING,         ///< 1007: FPR缺少dpi配置项
        HCI_ERR_FPR_CONFIG_BGCOLOR_MISSING,     ///< 1008: FPR缺少bgColor配置项
        HCI_ERR_FPR_CONFIG_WIDTH_MISSING,       ///< 1009: FPR缺少width配置项
        HCI_ERR_FPR_CONFIG_HEIGHT_MISSING,      ///< 1010: FPR缺少height配置项
        HCI_ERR_FPR_CONFIG_GROUP_ID_MISSING,    ///< 1011: FPR缺少groupId配置项

        /*HCI_AFR*/
        HCI_ERR_AFR_NOT_INIT = 1100,                ///< 1100: HCI_AFR没有初始化
        HCI_ERR_AFR_ALREADY_INIT,                   ///< 1101: HCI_AFR多次初始化
        HCI_ERR_AFR_ENGINE_INIT_FAILED,             ///< 1102: AFR本地引擎初始化失败
        HCI_ERR_AFR_ENGINE_FAILED,                  ///< 1103: AFR本地引擎处理失败
        HCI_ERR_AFR_USERID_NOT_EXIST,               ///< 1104: AFR用户不存在
        HCI_ERR_AFR_ENGINE_SESSION_START_FAILED,    ///< 1105: AFR本地引擎开启会话失败
        HCI_ERR_AFR_CONFIG_USER_ID_MISSING,         ///< 1106: AFR缺少userId配置项
        HCI_ERR_AFR_CONFIG_FACE_ID_MISSING,         ///< 1107: AFR缺少faceid配置项
        HCI_ERR_AFR_CONFIG_GROUP_ID_MISSING,        ///< 1108: AFR缺少groupId配置项
        HCI_ERR_AFR_ENGINE_SESSION_IMAGE_MISSING,   ///< 1109: AFR探测未设置图像信息
        HCI_ERR_AFR_LOAD_IMAGE,                     ///< 1110: AFR无法打开文件
        HCI_ERR_AFR_IMAGE_INVALID,                  ///< 1111: AFR输入图像非法
        HCI_ERR_AFR_MODEL_INVALID,                  ///< 1112: AFR输入的人脸模型非法
        HCI_ERR_AFR_CONFIG_INVALID,                 ///< 1113: AFR输入的配置串不合法
        HCI_ERR_AFR_PARAM_KEY,                      ///< 1114: AFR不正确的参数Key
        HCI_ERR_AFR_SAVE_MODEL,                     ///< 1115: 保存用户模型错误
        HCI_ERR_AFR_ENGINE,                         ///< 1116: 引擎错误, 一般不会发生
        HCI_ERR_AFR_FACEID_NOT_EXIST,               ///< 1117: AFR人脸不存在
        HCI_ERR_AFR_MODEL_NOT_EXIST                 ///< 1118: AFR人脸模型不存在
    }

    /// <summary>
    /// ASR识别函数的返回结果
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ASR_RECOG_RESULT
    {
        /// <summary>
        /// 识别候选结果列表
        /// </summary>
        public IntPtr psResultItemList;

        /// <summary>
        /// 识别候选结果的数目
        /// </summary>
        public uint uiResultItemCount;
    }

    /// <summary>
    /// ASR识别候选结果条目
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ASR_RECOG_RESULT_ITEM
    {
        /// <summary>
        /// 候选结果分值, 分值越高，越可信
        /// </summary>
        public uint uiScore;

        /// <summary>
        /// 候选结果字符串，UTF-8编码，以'\0'结束
        /// </summary>
        public IntPtr pszResult;
    }
}
