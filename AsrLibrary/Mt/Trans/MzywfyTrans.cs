/*********************************************************************************************
 *	
 * 文件名称:    MzywfyTrans.cs
 * 
 * 描    述：   民族语文翻译实现类。
 *
 * 作    者:    Anuo
 *	
 * 创作日期:    2019-3-4
 *
 * 备    注:	民族语文翻译局支持语种：汉文、蒙古文、藏文、维吾尔文、哈萨克文、朝鲜文、彝文、壮文。
 *                                        
*********************************************************************************************/

using AsrLibrary.Entity;

namespace AsrLibrary.Mt
{
    /// <summary>
    /// 民族语文翻译的实现类
    /// </summary>
    internal class MzywfyTrans : TransBase
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public MzywfyTrans()
        {

        }

        /// <summary>
        /// 翻译
        /// </summary>
        /// <param name="text">待翻译的内容</param>
        /// <param name="from">翻译源语种</param>
        /// <param name="to">翻译目的语种</param>
        /// <param name="result">成功返回翻译结果，失败返回错误消息</param>
        /// <returns>true-成功；false-失败</returns>
        public override bool Translate(string text, LanguageType from, LanguageType to, out string result)
        {
            result = string.Empty;
            string fromStr = string.Empty;
            string url = string.Empty;

            switch (from)
            {
                case LanguageType.Mongolian:
                    fromStr = "mo";
                    url = "5";
                    break;

                case LanguageType.Utsang:
                case LanguageType.Kham:
                case LanguageType.Amdo:
                    fromStr = "ti";
                    url = "6";
                    break;

                case LanguageType.Uygur:
                    fromStr = "uy";
                    url = "7";
                    break;

                case LanguageType.Kazak:
                    fromStr = "ka";
                    url = "8";
                    break;

                case LanguageType.Korean:
                    fromStr = "ko";
                    url = "9";
                    break;

                case LanguageType.Yi:
                    fromStr = "yi";
                    url = "12";
                    break;

                case LanguageType.Zhuang:
                    fromStr = "za";
                    url = "10";
                    break;
            }

            if (string.IsNullOrEmpty(fromStr))
            {
                result = "民族语文翻译 API 暂不支持该语种的翻译。";
                return false;
            }
            else
            {
                return MZYWFY_API.Translate(text, fromStr, "zh", url, out result);
            }
        }
    }
}
