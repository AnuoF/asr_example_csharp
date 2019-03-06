/*********************************************************************************************
 *	
 * 文件名称:    IAsr.cs
 *
 * 描    述：   语音识别对外提供的接口
 *  
 * 作    者:    Anuo
 *	
 * 创作日期:    2019-2-27
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using AnuoLibrary.Entity;
using System;
using System.Collections.Generic;

namespace AnuoLibrary.Asr
{
    /// <summary>
    /// 语音识别接口。
    /// 编写此接口的目的是让调用者方便使用，统一接口，不用关心使用的是什么SDK。
    /// 要达到这样的要求，内部实现就必须要灵活，扩展性好。
    /// </summary>
    public interface IAsr : IDisposable
    {
        /// <summary>
        /// 语音识别
        /// </summary>
        /// <param name="audioData">小于 60s 的音频数据（总长度不超过32000），音频格式要求：pcm/16k/16位/单通道 。</param>
        /// <param name="languageType">音频语种</param>
        /// <param name="recogResult">识别成功返回识别结果，识别失败返回错误消息</param>
        /// <returns>识别成功或失败，true-成功；false-失败</returns>
        bool AudioRecog(byte[] audioData, LanguageType languageType, out string recogResult);

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
    }
}
