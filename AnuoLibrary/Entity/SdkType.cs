using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnuoLibrary.Entity
{
    /// <summary>
    /// 语音识别引擎类型
    /// </summary>
    internal enum SdkType
    {
        /// <summary>
        /// 百度语音识别
        /// </summary>
        Baidu,

        /// <summary>
        /// 捷通华声语音识别
        /// </summary>
        Jths,

        /// <summary>
        /// 讯飞语音识别
        /// </summary>
        iFly
    }
}
