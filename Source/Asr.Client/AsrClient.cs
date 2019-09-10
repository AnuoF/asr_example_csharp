using Asr.Public;
using System;
using System.Collections.Generic;

namespace Asr.Client
{
    /// <summary>
    /// 语音识别客户端类，供外部集成和调用
    /// </summary>
    public class AsrClient : IDisposable
    {
        /// <summary>
        /// 建立与服务端的连接事件
        /// </summary>
        public event EventHandler OnInitialized;

        private IAsr _asr = null;
        /// <summary>
        /// 语音识别接口
        /// </summary>
        public IAsr IAsr
        {
            get { return _asr; }
        }

        private ITranslate _translate = null;
        /// <summary>
        /// 翻译接口
        /// </summary>
        public ITranslate ITranslate
        {
            get { return _translate; }
        }

        /// <summary>
        /// 与服务端是否建立连接
        /// </summary>
        public bool IsConnected
        {
            get { return _asr == null ? false : _asr.IsConnected; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">语音识别服务器 IP 地址</param>
        /// <param name="port">语音识别服务器 端口</param>
        public AsrClient(string ip, int port)
        {
            _asr = AsrFun.GetInstance(ip, port);
            _translate = (ITranslate)_asr;
            _asr.OnInitialized += _asr_OnInitialized;
        }

        /// <summary>
        /// 开始建立与服务端的连接（异步的方式），可通过注册 OnInitialized 事件来通知客户端是否已完成初始化
        /// </summary>
        public void ConnectAsync()
        {
            _asr.Initialize();
        }

        /// <summary>
        /// 语音识别。如果是标准音频格式：pcm/16k/16位/单通道，则调用此方法。如果是其他格式的音频，请调用另一个方法并传入音频格式 WaveFormat 参数。
        /// </summary>
        /// <param name="audioData">小于 60s 的音频数据（总长度不超过1920k），音频格式要求：pcm/16k/16位/单通道 。</param>
        /// <param name="languageType">音频语种类型</param>
        /// <param name="recogResult">识别成功返回识别结果，识别失败返回错误消息</param>
        /// <returns>识别成功或失败，true-成功；false-失败</returns>
        public bool AudioRecog(byte[] audioData, LanguageType languageType, out string recogResult)
        {
            if (_asr == null)
            {
                recogResult = "客户端尚未初始化";
                return false;
            }

            return _asr.AudioRecog(audioData, languageType, out recogResult);
        }

        /// <summary>
        /// 语音识别。如果是标准音频格式：pcm/16k/16位/单通道，请调用另一个方法，不需要传入参数 WaveFormat 参数，如果是其他格式，则调用此方法。
        /// </summary>
        /// <param name="audioData">小于 60s 的音频数据</param>
        /// <param name="languageType">音频语种</param>
        /// <param name="recogResult">识别成功返回识别结果，识别失败返回错误消息</param>
        /// <param name="waveFormat">音频格式</param>
        /// <returns>识别成功或失败，true-成功；false-失败</returns>
        public bool AudioRecog(byte[] audioData, LanguageType languageType, out string recogResult, WaveFormat waveFormat)
        {
            if (_asr == null)
            {
                recogResult = "客户端尚未初始化";
                return false;
            }

            return _asr.AudioRecog(audioData, languageType, out recogResult, waveFormat);
        }

        /// <summary>
        /// 翻译
        /// </summary>
        /// <param name="text">待翻译的内容</param>
        /// <param name="from">翻译源语种</param>
        /// <param name="result">成功返回翻译结果，失败返回错误消息</param>
        /// <param name="to">翻译目的语种，默认为中文</param>
        /// <returns>true-成功；false-失败</returns>
        public bool Trans(string text, LanguageType from, out string result, LanguageType to = LanguageType.Mandarin)
        {
            if (_translate == null)
            {
                result = "客户端尚未初始化";
                return false;
            }

            return _translate.Trans(text, from, out result, to);
        }

        /// <summary>
        /// 获取支持的语种
        /// </summary>
        /// <returns>支持的语种列表</returns>
        public List<Language> GetTransLanguages()
        {
            if(_translate == null)
            {
                return new List<Language>();
            }

            return _translate.GetTransLanguages();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_asr != null)
            {
                _asr.Dispose();
                _asr = null;
            }
        }

        private void _asr_OnInitialized(object sender, EventArgs e)
        {
            if (OnInitialized != null)
            {
                try { OnInitialized.Invoke(sender, e); } catch { }   // 防止内部异常影响到这里
            }
        }
    }
}
