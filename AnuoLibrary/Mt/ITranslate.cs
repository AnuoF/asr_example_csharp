/*********************************************************************************************
 *	
 * 文件名称:    ITranslate.cs
 * 
 * 描    述：   翻译接口。
 *
 * 作    者:    Anuo
 *	
 * 创作日期:    2019-3-4
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using AnuoLibrary.Entity;
using System.Collections.Generic;

namespace AnuoLibrary.Mt
{
    /// <summary>
    /// 翻译接口：对外提供
    /// </summary>
    public interface ITranslate
    {
        /// <summary>
        /// 翻译
        /// </summary>
        /// <param name="text">待翻译的内容</param>
        /// <param name="from">翻译源语种</param>
        /// <param name="result">成功返回翻译结果，失败返回错误消息</param>
        /// <param name="to">翻译目的语种，默认为中文</param>
        /// <returns>true-成功；false-失败</returns>
        bool Trans(string text, LanguageType from, out string result, LanguageType to = LanguageType.Mandarin);

        /// <summary>
        /// 获取支持的语种
        /// </summary>
        /// <returns>支持的语种列表</returns>
        List<Language> GetTransLanguages();
    }
}
