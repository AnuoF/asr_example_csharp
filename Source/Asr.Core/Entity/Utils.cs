using Asr.Public;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Asr.Core
{
    internal class Utils
    {
        /// <summary>
        /// 功能函数是否初始化
        /// </summary>
        public static bool IsInitFun = false;

        /// <summary>
        /// Fun的工作模式：true-远程；false-本地
        /// </summary>
        public static bool RemoteMode = false;

        /// <summary>
        /// 配置文件全路径
        /// </summary>
        public static string ConfigName;

        /// <summary>
        /// 可识别的语种列表
        /// </summary>
        public static List<Language> _languageRecogList = new List<Language>();

        /// <summary>
        /// 可翻译的语种列表
        /// </summary>
        public static List<Language> _languageTransList = new List<Language>();


        /// <summary>
        /// 添加环境变量
        /// </summary>
        /// <param name="paths">路径列表</param>
        public static void AddEnvironmentPaths(IEnumerable<string> paths)
        {
            // 参考 https://www.cnblogs.com/fsh001/p/8654790.html
            var path = new[] { Environment.GetEnvironmentVariable("PATH") ?? string.Empty };
            string newPath = string.Join(Path.PathSeparator.ToString(), path.Concat(paths));
            Environment.SetEnvironmentVariable("PATH", newPath);   // 这种方式只会修改当前进程的环境变量
        }
    }
}
