/*********************************************************************************************
 *	
 * 文件名称:    AsrFun.cs
 *
 * 描    述：   AsrFun 类，语音识别功能入口类
 *  
 * 作    者:    Anuo
 *	
 * 创作日期:    2019-3-6
 *
 * 备    注:	
 *                                        
*********************************************************************************************/


namespace AnuoLibrary.Asr
{
    /// <summary>
    /// AsrFun 静态类，语音识别功能入口类
    /// </summary>
    public static class AsrFun
    {
        /// <summary>
        /// 语音识别接口
        /// </summary>
        private static IAsr _asr = null;

        /// <summary>
        /// 对象锁
        /// </summary>
        private static object _lockObj = new object();

        /// <summary>
        /// 获取语音识别接口
        /// </summary>
        public static IAsr Asr
        {
            get
            {
                lock (_lockObj)
                {
                    if (_asr == null)
                    {
                        _asr = new Asr();
                    }
                }

                return _asr;
            }
        }
    }
}
