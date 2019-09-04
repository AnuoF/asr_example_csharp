/*********************************************************************************************
 *	
 * 文件名称:    LogManager.cs
 * 
 * 描    述：   日志类
 *
 * 作    者:    王 喜 进
 *	
 * 创作日期:    2019-4-1
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using System;
using System.Diagnostics;
using System.IO;

namespace AsrServer
{
    /// <summary>
    /// 日志类
    /// </summary>
    internal static class LogManager
    {
        private static string _logPath = string.Empty;
        /// <summary>
        /// 获取设置日志路径（默认为应用程序根目录）
        /// </summary>
        public static string LogPath
        {
            get
            {
                if (string.IsNullOrEmpty(_logPath))
                {
                    _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
                }

                if (!Directory.Exists(_logPath))
                {
                    Directory.CreateDirectory(_logPath);
                }

                return _logPath;
            }
            set { _logPath = value; }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteLog(string msg)
        {
            try
            {
                StreamWriter sw = File.AppendText(LogPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log");
                sw.WriteLine(msg);
                sw.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }
    }
}
