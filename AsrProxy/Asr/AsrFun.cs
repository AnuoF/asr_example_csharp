/*********************************************************************************************
 *	
 * 文件名称:    AsrFun.cs
 * 
 * 描    述：   AsrFun 类，语音识别功能入口类
 *
 * 作    者:    Anuo.
 *	
 * 创作日期:    2019-3-26
 *
 * 备    注:	
 *              请注意：AsrProxy 与 AsrLibrary 集成相同的接口 IAsr，只是实现不同。        
 *                        
*********************************************************************************************/

using AsrCommon.Asr;

namespace AsrProxy
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
            return Asr.Asr.GetInstance();
        }
    }
}
