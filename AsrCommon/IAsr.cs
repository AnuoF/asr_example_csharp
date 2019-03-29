/*********************************************************************************************
 *	
 * 文件名称:    IAsr.cs
 *
 * 描    述：   语音识别对外提供的接口
 *  
 * 作    者:    Anuo.
 *	
 * 创作日期:    2019-2-27
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using AsrCommon.Entity;
using System;
using System.Collections.Generic;

namespace AsrCommon.Asr
{
    /// <summary>
    /// 语音识别接口。
    /// </summary>
    public interface IAsr : IDisposable
    {
        /// <summary>
        /// 语音识别。如果是标准音频格式：pcm/16k/16位/单通道，则调用此方法。如果是其他格式的音频，请调用另一个方法并传入音频格式 WaveFormat 参数。
        /// </summary>
        /// <param name="audioData">小于 60s 的音频数据（总长度不超过32000），音频格式要求：pcm/16k/16位/单通道 。</param>
        /// <param name="languageType">音频语种</param>
        /// <param name="recogResult">识别成功返回识别结果，识别失败返回错误消息</param>
        /// <returns>识别成功或失败，true-成功；false-失败</returns>
        bool AudioRecog(byte[] audioData, LanguageType languageType, out string recogResult);

        /// <summary>
        /// 语音识别。如果是标准音频格式：pcm/16k/16位/单通道，请调用另一个方法，不需要传入参数 WaveFormat 参数，如果是其他格式，则调用此方法。
        /// </summary>
        /// <param name="audioData">小于 60s 的音频数据</param>
        /// <param name="languageType">音频语种</param>
        /// <param name="recogResult">识别成功返回识别结果，识别失败返回错误消息</param>
        /// <param name="waveFormat">音频格式。请注意 WaveFormat 与 Tracker800 中的定义和使用完全一致，但为了形成独立的功能库，尽量避免与其他库耦合在一起，所以单独移植过来的。</param>
        /// <returns>识别成功或失败，true-成功；false-失败</returns>
        bool AudioRecog(byte[] audioData, LanguageType languageType, out string recogResult, WaveFormat waveFormat);

        /// <summary>
        /// 获取可识别的语种列表
        /// </summary>
        /// <returns>语种列表</returns>
        List<Language> GetLanguageList();

        /// <summary>
        /// 将配置文件中 text 字段映射到 LanguageType 枚举值
        /// </summary>
        /// <param name="text">text 字段内容</param>
        /// <returns>LanguageType 枚举值</returns>
        LanguageType Text2LanguageType(string text);

        /// <summary>
        /// 将配置文件中 name 字段映射到 LanguageType 枚举值
        /// </summary>
        /// <param name="name">name 字段内容</param>
        /// <returns>LanguageType 枚举值</returns>
        LanguageType Name2LanguageType(string name);

        #region 此部分接口是为扩展分布式部署时添加
        /// <summary>
        /// 初始化成功事件
        /// </summary>
        event EventHandler OnInitialized;

        /// <summary>
        /// 初始化
        /// </summary>
        void Initialize();

        /// <summary>
        /// 获取与服务端的连接状态（根据需要调用）
        /// </summary>
        bool IsConnected { get; }
        #endregion
    }
}
