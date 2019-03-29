/*********************************************************************************************
 *	
 * 文件名称:    Log.cs
 * 
 * 描    述：   系统日志类，封装成类，方便后面扩展或修改。
 *
 * 作    者:    Anuo.
 *	
 * 创作日期:    2019-3-27
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using System.Diagnostics;

namespace AsrProxy
{
    /// <summary>
    /// 系统日志类，封装成类，方便后面扩展或修改。
    /// </summary>
    internal class Log
    {
        /// <summary>
        /// 打印日志
        /// </summary>
        /// <param name="log"></param>
        public static void WriteLog(string log)
        {
            Trace.WriteLine(log);
        }
    }
}
