/*********************************************************************************************
 *	
 * 文件名称:    BuzProcess.cs
 * 
 * 描    述：   业务处理类，逻辑处理由此类单独提供，不与 Asr 类搅在一起，使程序结构更清晰。
 *              集成相同的接口 IAsr，方便从 Asr 类直接映射过来。
 *
 * 作    者:    王 喜 进
 *	
 * 创作日期:    2019-3-26
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using Asr.Public;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Asr.Client
{
    /// <summary>
    /// 业务逻辑处理类
    /// </summary>
    internal class BuzProcess : IAsr, ITranslate
    {
        /// <summary>
        /// 初始化成功事件
        /// </summary>
        public event EventHandler OnInitialized;
        /// <summary>
        /// 客户端
        /// </summary>
        private TCPClient _client = null;
        /// <summary>
        /// 数据封装、解析类
        /// </summary>
        private DataHelper _dataHelper = null;
        /// <summary>
        /// 数据缓存队列
        /// </summary>
        private Queue<byte[]> _dataQueue = null;
        /// <summary>
        /// 数据队列锁
        /// </summary>
        private object _queueLock = null;
        /// <summary>
        /// 处理数据线程
        /// </summary>
        private Thread _dealDataThd = null;
        /// <summary>
        /// 数据到达信号量
        /// </summary>
        private Semaphore _semaphore = null;

        /// <summary>
        /// 0x0602 数据接收标识
        /// </summary>
        private bool _0x0602flag = false;
        /// <summary>
        /// 0x0601 数据接收标识
        /// </summary>
        private bool _0x0601flag = false;
        /// <summary>
        /// 0x0603 数据接收标识
        /// </summary>
        private bool _0x0603flag = false;
        /// <summary>
        /// 0x0605 数据接收标识
        /// </summary>
        private bool _0x0605flag = false;
        /// <summary>
        /// 0x0606 数据接收标识
        /// </summary>
        private bool _0x0606flag = false;
        /// <summary>
        /// 接收到的语种列表（可识别的语种列表和可翻译的语种列表共用一个变量）
        /// </summary>
        private List<Language> _languageList = null;
        /// <summary>
        /// 解析出来的语音识别结果
        /// </summary>
        private string _recgResult = string.Empty;
        /// <summary>
        /// 识别成功或失败，由服务端返回
        /// </summary>
        private bool _recgSuccess = false;
        /// <summary>
        /// 解析出来的语种类型
        /// </summary>
        private LanguageType _languageType = LanguageType.Mandarin;
        /// <summary>
        /// 解析回来的翻译结果
        /// </summary>
        private bool _transSuccess = false;
        /// <summary>
        /// 解析出来的翻译结果
        /// </summary>
        private string _transResult = string.Empty;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BuzProcess(string ip, int port)
        {
            _client = new TCPClient(ip, port);
            _client.OnConnected += _client_OnConnected;
            _client.OnData += _client_OnData;

            _dataHelper = new DataHelper();
            _dataQueue = new Queue<byte[]>();
            _semaphore = new Semaphore(0, int.MaxValue);
            _queueLock = new object();
        }

        /// <summary>
        /// 获取与服务端的连接状态
        /// </summary>
        public bool IsConnected
        {
            get { return _client == null ? false : _client.IsConnected; }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            Dispose();   // 防止外部多次调用，每次调用之前，先清除释放资源

            _client.Start();

            _dealDataThd = new Thread(new ThreadStart(DealData));
            _dealDataThd.IsBackground = true;
            _dealDataThd.Name = "Deal data thread";
            _dealDataThd.Start();
        }

        public bool AudioRecog(byte[] audioData, LanguageType languageType, out string recogResult)
        {
            if (IsConnected == false)
            {
                recogResult = "与服务端尚未建立连接或连接已断开。";
                return false;
            }

            byte[] data = _dataHelper.CombineData0x0501(audioData, languageType);
            _0x0601flag = false;
            bool ret = _client.Send(data);
            if (ret == false)
            {
                recogResult = "发送业务数据失败。";
                return false;
            }

            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (_0x0601flag || (DateTime.Now - startTime).TotalMilliseconds >= 30000)
                    break;  // 超时时间设置为 30s，需多调试，确定一个最佳的超时时间

                Thread.Sleep(1);
            }

            if (_0x0601flag)
            {
                recogResult = _recgResult;
                return _recgSuccess;
            }
            else
            {
                recogResult = "接收语音识别结果超时。";
                return false;
            }
        }

        public bool AudioRecog(byte[] audioData, LanguageType languageType, out string recogResult, WaveFormat waveFormat)
        {
            // 此方法不会调用，不需实现
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取可识别的语种列表
        /// </summary>
        /// <returns></returns>
        public List<Language> GetLanguageList()
        {
            if (IsConnected == false)
            {
                return new List<Language>();
            }

            // 流程：打包数据，发送数据，等待接收数据，返回结果，中间会有超时时间
            byte[] data = _dataHelper.CombineData0x0502();
            _0x0602flag = false;
            bool ret = _client.Send(data);
            if (ret == false)
                return new List<Language>();

            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (_0x0602flag || (DateTime.Now - startTime).TotalMilliseconds >= 5000)
                    break;                     // 如果已接收到数据 或 超过超时时间（5s），则退出循环

                Thread.Sleep(1);
            }

            return _0x0602flag ? _languageList : new List<Language>();
        }

        /// <summary>
        /// Text 到 LangugaeType 的映射
        /// </summary>
        /// <param name="text">Text 字段，如“普通话”</param>
        /// <returns></returns>
        public LanguageType Text2LanguageType(string text)
        {
            if (IsConnected == false)
            {
                return LanguageType.Mandarin;
            }

            byte[] data = _dataHelper.CombineData0x0503(text);
            _0x0603flag = false;
            bool ret = _client.Send(data);
            if (ret == false)
            {
                return LanguageType.Mandarin;
            }

            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (_0x0603flag || (DateTime.Now - startTime).TotalMilliseconds >= 5000)    // 超时时间 5s
                    break;

                Thread.Sleep(1);
            }

            return _0x0603flag ? _languageType : LanguageType.Mandarin;
        }

        /// <summary>
        /// Name 到 LangugaeType 的映射
        /// </summary>
        /// <param name="text">Name 字段，如“Sichuan”</param>
        /// <returns></returns>
        public LanguageType Name2LanguageType(string name)
        {
            if (IsConnected == false)
            {
                return LanguageType.Mandarin;
            }

            byte[] data = _dataHelper.CombineData0x0504(name);
            _0x0603flag = false;
            bool ret = _client.Send(data);
            if (ret == false)
            {
                return LanguageType.Mandarin;
            }

            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (_0x0603flag || (DateTime.Now - startTime).TotalMilliseconds >= 5000)  // 超时时间 5s
                    break;

                Thread.Sleep(1);
            }

            return _0x0603flag ? _languageType : LanguageType.Mandarin;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_client != null)
            {
                _client.Dispose();
            }

            if (_dataQueue != null)
            {
                _dataQueue.Clear();
            }

            if (_dealDataThd != null && _dealDataThd.IsAlive)
            {
                try { _dealDataThd.Abort(); } catch { }
                _dealDataThd = null;
            }
        }

        /// <summary>
        /// 翻译
        /// </summary>
        /// <param name="text"></param>
        /// <param name="from"></param>
        /// <param name="result"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public bool Trans(string text, LanguageType from, out string result, LanguageType to = LanguageType.Mandarin)
        {
            if (IsConnected == false)
            {
                result = "与服务端尚未建立连接或连接已断开。";
                return false;
            }

            byte[] data = _dataHelper.CombineData0x0505(text, from, to);
            _0x0605flag = false;
            bool ret = _client.Send(data);
            if (ret == false)
            {
                result = "发送翻译请求业务数据失败。";
                return false;
            }

            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (_0x0605flag || (DateTime.Now - startTime).TotalMilliseconds >= 10000)
                    break;   // 超时时间 10s

                Thread.Sleep(1);
            }

            if (_0x0605flag)
            {
                result = _transResult;
                return _transSuccess;
            }
            else
            {
                result = "接收翻译结果数据超时。";
                return false;
            }
        }

        /// <summary>
        /// 获取可翻译的列表
        /// </summary>
        /// <returns></returns>
        public List<Language> GetTransLanguages()
        {
            if (IsConnected == false)
            {
                return new List<Language>();
            }

            byte[] data = _dataHelper.CombineData0x0506();
            _0x0606flag = false;
            bool ret = _client.Send(data);
            if (ret == false)
            {
                return new List<Language>();
            }

            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (_0x0606flag || (DateTime.Now - startTime).TotalMilliseconds >= 5000)  // 5s
                    break;

                Thread.Sleep(1);
            }

            // 可识别的语种列表和可翻译的语种列表共用一个变量
            return _0x0606flag ? _languageList : new List<Language>();
        }

        // 建立连接事件
        private void _client_OnConnected(object sender, EventArgs e)
        {
            if (OnInitialized != null)
            {
                OnInitialized.Invoke(sender, e);
            }
        }

        // 数据到达
        private void _client_OnData(object sender, RecvDataEventArgs e)
        {
            lock (_queueLock)
            {
                _dataQueue.Enqueue(e.Data);
            }

            _semaphore.Release();
        }

        /// <summary>
        /// 处理数据线程
        /// </summary>
        private void DealData()
        {
            while (true)
            {
                _semaphore.WaitOne();

                byte[] data = null;
                lock (_queueLock)
                {
                    data = _dataQueue.Dequeue();
                }

                if (data == null)
                    continue;

                try
                {
                    AnalyzeData(data);
                }
                catch (Exception ex)
                {
                    if (ex is ThreadAbortException)
                        return;

                    //Log.WriteLog("解析数据异常：" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="data"></param>
        private void AnalyzeData(byte[] data)
        {
            // 提取数据类型
            short dataType = BitConverter.ToInt16(data, 5);

            switch (dataType)
            {
                // 语音识别结果
                case 0x0601:
                    _recgSuccess = _dataHelper.Analyze0x0601(data, out _recgResult);
                    _0x0601flag = true;
                    break;

                // 可识别的语种列表
                case 0x0602:
                    _languageList = _dataHelper.Analyze0x0602(data);
                    _0x0602flag = true;
                    break;

                // LanguagetType 映射
                case 0x0603:
                    _languageType = _dataHelper.Analyze0x0603(data);
                    _0x0603flag = true;
                    break;

                // 0x0604 保留，与 0x0603 一样

                // 翻译结果
                case 0x0605:
                    _transSuccess = _dataHelper.Analyze0x0605(data, out _transResult);
                    _0x0605flag = true;
                    break;

                // 可翻译的语种列表，与 0x0602 数据协议一致
                case 0x0606:
                    _languageList = _dataHelper.Analyze0x0602(data);
                    _0x0606flag = true;
                    break;

                // 心跳包，不做处理
                case 0x0555:
                    break;

                // 未处理的数据类型
                default:
                    //Log.WriteLog(string.Format("收到数据类型 {0} ，未做处理。", string.Format("{0:X}", dataType)));
                    break;
            }
        }

    }
}
