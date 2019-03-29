/*********************************************************************************************
 *	
 * 文件名称:    TranslateFun.cs
 *
 * 描    述：   TranslateFun 类，翻译功能入口类
 *  
 * 作    者:    Anuo
 *	
 * 创作日期:    2019-3-7
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using AsrCommon.Mt;
using AsrLibrary.Mt;

namespace AsrLibrary
{
    /// <summary>
    /// TranslateFun 静态类，翻译功能入口类
    /// </summary>
    public static class TranslateFun
    {
        /// <summary>
        /// 获取翻译功能接口
        /// </summary>
        public static ITranslate GetTranslate()
        {
            return Translate.GetInstance();
        }
    }
}
