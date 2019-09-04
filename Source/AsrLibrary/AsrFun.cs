/*********************************************************************************************
 *	
 * 文件名称:    AsrFun.cs
 *
 * 描    述：   AsrFun 类，语音识别功能入口类
 *  
 * 作    者:    王 喜 进
 *	
 * 创作日期:    2019-3-6
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

namespace AsrLibrary
{
    /// <summary>
    /// AsrFun 静态类，语音识别功能入口类
    /// </summary>
    public static class AsrFun
    {
        /// <summary>
        /// 获取语音识别接口
        /// </summary>
        public static IAsr GetAsr()
        {
            // 获取调用方式
            bool remoteMode = Utils.GetFunMode();

            if (remoteMode)
            {
                return Proxy.Asr.GetInstance();
            }
            else
            {
                return Asr.Asr.GetInstance();
            }
        }
    }
}
