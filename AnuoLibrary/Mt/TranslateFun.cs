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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnuoLibrary.Mt
{
    /// <summary>
    /// 静态类，翻译功能入口类
    /// </summary>
    public static class TranslateFun
    {
        /// <summary>
        /// 翻译功能接口
        /// </summary>
        private static ITranslate _translate = null;

        /// <summary>
        /// 对象锁
        /// </summary>
        private static object _lockObj = new object();

        /// <summary>
        /// 获取翻译功能接口
        /// </summary>
        public static ITranslate Translate
        {
            get
            {
                lock (_lockObj)
                {
                    if (_translate == null)
                    {
                        _translate = new Translate();
                    }

                    return _translate;
                }
            }
        }
    }
}
