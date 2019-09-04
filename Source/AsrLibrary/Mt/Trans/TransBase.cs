/*********************************************************************************************
 *	
 * 文件名称:    TransBase.cs
 * 
 * 描    述：   翻译抽象基类。
 *
 * 作    者:    王 喜 进
 *	
 * 创作日期:    2019-3-4
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using AsrLibrary.Entity;

namespace AsrLibrary.Mt
{
    /// <summary>
    /// 翻译抽象基类
    /// </summary>
    internal abstract class TransBase
    {
        /// <summary>
        /// 翻译
        /// </summary>
        /// <param name="text">待翻译的内容</param>
        /// <param name="from">翻译源语种</param>
        /// <param name="to">翻译目的语种</param>
        /// <param name="result">成功返回翻译结果，失败返回错误消息</param>
        /// <returns>true-成功；false-失败</returns>
        public abstract bool Translate(string text, LanguageType from, LanguageType to, out string result);
    }
}
