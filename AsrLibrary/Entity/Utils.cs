using AsrCommon.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AsrLibrary
{
    /// <summary>
    /// 全局类：提供公共字段或方法
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// 配置文件全路径
        /// </summary>
        internal static string configName;

        /// <summary>
        /// 可识别的语种列表
        /// </summary>
        internal static List<Language> _languageRecogList = new List<Language>();

        /// <summary>
        /// 可翻译的语种列表
        /// </summary>
        internal static List<Language> _languageTransList = new List<Language>();

        /// <summary>
        /// 添加环境变量
        /// </summary>
        /// <param name="paths">路径列表</param>
        internal static void AddEnvironmentPaths(IEnumerable<string> paths)
        {
            // 参考 https://www.cnblogs.com/fsh001/p/8654790.html
            var path = new[] { Environment.GetEnvironmentVariable("PATH") ?? string.Empty };
            string newPath = string.Join(Path.PathSeparator.ToString(), path.Concat(paths));
            Environment.SetEnvironmentVariable("PATH", newPath);   // 这种方式只会修改当前进程的环境变量
        }

        /// <summary>
        /// 将配置文件中 text 字段映射到 LanguageType 枚举值
        /// </summary>
        /// <param name="text">text 字段内容</param>
        /// <returns>LanguageType 枚举值</returns>
        public static LanguageType Text2LanguageType(string text)
        {
            LanguageType type = LanguageType.Mandarin;

            switch (text)
            {
                case "英语":
                    type = LanguageType.English;
                    break;
                case "普通话":
                    type = LanguageType.Mandarin;
                    break;
                case "粤语":
                    type = LanguageType.Yue;
                    break;
                case "四川话":
                    type = LanguageType.Sichuan;
                    break;
                case "维吾尔语":
                    type = LanguageType.Uygur;
                    break;
                case "国语(台湾)":
                    type = LanguageType.Taiwan;
                    break;
                case "藏语":
                    type = LanguageType.Utsang;
                    break;
                case "藏语安多":
                    type = LanguageType.Amdo;
                    break;
                case "藏语康巴":
                    type = LanguageType.Kham;
                    break;
                case "哈萨克语":
                    type = LanguageType.Kazak;
                    break;
                case "朝鲜语":
                    type = LanguageType.Korean;
                    break;
                case "彝语":
                    type = LanguageType.Yi;
                    break;
                case "蒙文":
                    type = LanguageType.Mongolian;
                    break;
                case "广东阳江话":
                    type = LanguageType.Yangjiang;
                    break;
                case "壮语":
                    type = LanguageType.Zhuang;
                    break;
                case "闽南语":
                    type = LanguageType.Minnan;
                    break;
                case "上海话":
                    type = LanguageType.Shanghai;
                    break;
                case "东北话":
                    type = LanguageType.Dongbei;
                    break;
                case "河南话":
                    type = LanguageType.Henan;
                    break;
                case "天津话":
                    type = LanguageType.Tianjin;
                    break;
                case "山东话":
                    type = LanguageType.Shandong;
                    break;
                case "贵州话":
                    type = LanguageType.Guizhou;
                    break;
                case "宁夏话":
                    type = LanguageType.Ningxia;
                    break;
                case "云南话":
                    type = LanguageType.Yunnan;
                    break;
                case "陕西话":
                    type = LanguageType.Shanxi;
                    break;
                case "甘肃话":
                    type = LanguageType.Gansu;
                    break;
                case "武汉话":
                    type = LanguageType.Wuhan;
                    break;
                case "河北话":
                    type = LanguageType.Hebei;
                    break;
                case "合肥话":
                    type = LanguageType.Hefei;
                    break;
                case "长沙话":
                    type = LanguageType.Changsha;
                    break;
                case "太原话":
                    type = LanguageType.Taiyuan;
                    break;
            }

            return type;
        }

        /// <summary>
        /// 将配置文件中 name 字段映射到 LanguageType 枚举值
        /// </summary>
        /// <param name="name">name 字段内容</param>
        /// <returns>LanguageType 枚举值</returns>
        public static LanguageType Name2LanguageType(string name)
        {
            LanguageType type = LanguageType.Mandarin;

            switch (name)
            {
                case "English":
                    type = LanguageType.English;
                    break;
                case "Mandarin":
                    type = LanguageType.Mandarin;
                    break;
                case "Yue":
                    type = LanguageType.Yue;
                    break;
                case "Sichuan":
                    type = LanguageType.Sichuan;
                    break;
                case "Uygur":
                    type = LanguageType.Uygur;
                    break;
                case "Taiwan":
                    type = LanguageType.Taiwan;
                    break;
                case "Utsang":
                    type = LanguageType.Utsang;
                    break;
                case "Amdo":
                    type = LanguageType.Amdo;
                    break;
                case "Kham":
                    type = LanguageType.Kham;
                    break;
                case "Kazak":
                    type = LanguageType.Kazak;
                    break;
                case "Korean":
                    type = LanguageType.Korean;
                    break;
                case "Yi":
                    type = LanguageType.Yi;
                    break;
                case "Mongolian":
                    type = LanguageType.Mongolian;
                    break;
                case "Yangjiang":
                    type = LanguageType.Yangjiang;
                    break;
                case "Zhuang":
                    type = LanguageType.Zhuang;
                    break;
                case "Minnan":
                    type = LanguageType.Minnan;
                    break;
                case "Shanghai":
                    type = LanguageType.Shanghai;
                    break;
                case "Dongbei":
                    type = LanguageType.Dongbei;
                    break;
                case "Henan":
                    type = LanguageType.Henan;
                    break;
                case "Tianjin":
                    type = LanguageType.Tianjin;
                    break;
                case "Shandong":
                    type = LanguageType.Shandong;
                    break;
                case "Guizhou":
                    type = LanguageType.Guizhou;
                    break;
                case "Ningxia":
                    type = LanguageType.Ningxia;
                    break;
                case "Yunnan":
                    type = LanguageType.Yunnan;
                    break;
                case "Shanxi":
                    type = LanguageType.Shanxi;
                    break;
                case "Gansu":
                    type = LanguageType.Gansu;
                    break;
                case "Wuhan":
                    type = LanguageType.Wuhan;
                    break;
                case "Hebei":
                    type = LanguageType.Hebei;
                    break;
                case "Hefei":
                    type = LanguageType.Hefei;
                    break;
                case "Changsha":
                    type = LanguageType.Changsha;
                    break;
                case "Taiyuan":
                    type = LanguageType.Taiyuan;
                    break;
            }

            return type;
        }
    }
}
