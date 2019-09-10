/*********************************************************************************************
 *	
 * 文件名称:    AsrBase.cs
 *
 * 描    述：   语音识别抽象基类，所有的 sdk 都要集成此抽象类。
 * 
 * 作    者:    王 喜 进
 *	
 * 创作日期:    2019-2-27
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using Asr.Public;

namespace Asr.Core
{
    /// <summary>
    /// 语音识别抽象类
    /// </summary>
    internal abstract class AsrBase
    {
        /// <summary>
        /// SDK 是否初始化成功
        /// </summary>
        public bool _isSdkInit = false;
        /// <summary>
        /// 最近一次的错误信息
        /// </summary>
        public string _errMsg = string.Empty;
        /// <summary>
        /// 同步锁
        /// </summary>
        public static object _lockObj = new object();

        /// <summary>
        /// 语音识别
        /// </summary>
        /// <param name="audioData">小于 60s 的音频数据，音频格式要求：pcm/16k/16位/单通道 。</param>
        /// <param name="languageType">音频语种类型</param>
        /// <param name="recogResult">识别成功返回识别结果，识别失败返回错误消息</param>
        /// <returns>识别成功或失败，true-成功；false-失败</returns>
        public abstract bool AudioRecog(byte[] audioData, LanguageType languageType, out string recogResult);
    }
}
