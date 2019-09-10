/*********************************************************************************************
 *	
 * 文件名称:    TtsBaidu.cs
 * 
 * 描    述：   百度语音合成类。
 *
 * 作    者:    王 喜 进
 *	
 * 创作日期:    2019-3-5
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using System.Collections.Generic;

namespace Asr.Core.Tts
{
    /// <summary>
    /// 百度语音合成类
    /// </summary>
    public class TtsBaidu
    {
        /// <summary>
        /// API_KEY
        /// </summary>
        private string _apiKey;
        /// <summary>
        /// SECRET_KEY
        /// </summary>
        private string _secretKey;
        /// <summary>
        /// tts
        /// </summary>
        private Baidu.Aip.Speech.Tts _tts = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="apiKey">API_KEY</param>
        /// <param name="secretKey">SECRET_KEY</param>
        public TtsBaidu(string apiKey, string secretKey)
        {
            _apiKey = apiKey;
            _secretKey = secretKey;

            _tts = new Baidu.Aip.Speech.Tts(_apiKey, _secretKey);
            _tts.Timeout = 60000;
        }

        /// <summary>
        /// 语音合成
        /// </summary>
        /// <param name="text"></param>
        /// <param name="data"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Tts(string text, out byte[] data, out string errMsg)
        {
            //tex String  合成的文本，使用UTF - 8编码，
            //请注意文本长度必须小于1024字节 是
            //cuid String  用户唯一标识，用来区分用户，
            //填写机器 MAC 地址或 IMEI 码，长度为60以内 否
            //spd String  语速，取值0 - 9，默认为5中语速 否
            //pit String  音调，取值0 - 9，默认为5中语调 否
            //vol String  音量，取值0 - 15，默认为5中音量 否
            //per String  发音人选择, 0为女声，1为男声，
            //3为情感合成 - 度逍遥，4为情感合成 - 度丫丫，默认为普通女 否

            data = null;
            errMsg = "";
            // 可选参数
            var option = new Dictionary<string, object>()
            {
                {"spd", 5}, // 语速
                {"vol", 7}, // 音量
                {"per", 0}  // 发音人
            };

            var result = _tts.Synthesis(text, option);
            if (result.ErrorCode == 0)
            {
                data = result.Data;
                return true;
            }
            else
            {
                errMsg = result.ErrorMsg;
                return false;
            }
        }
    }
}
