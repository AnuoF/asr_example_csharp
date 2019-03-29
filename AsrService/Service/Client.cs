/*********************************************************************************************
 *	
 * 文件名称:    Client.cs
 * 
 * 描    述：   TCP 连接客户端，用于接收数据、处理数据等
 *
 * 作    者:    Anuo.
 *	
 * 创作日期:    2019-3-27
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using AsrCommon.Entity;
using AsrLibrary;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AsrService
{
    /// <summary>
    /// TCP 连接客户端，用于接收数据、处理数据等
    /// </summary>
    internal class Client : IDisposable
    {
        /// <summary>
        /// 断开连接事件
        /// </summary>
        public event EventHandler<DisconnectedEventArgs> Disconnected;
        /// <summary>
        /// 远程连接的IP
        /// </summary>
        public string RemoteIP { get; private set; }
        /// <summary>
        /// 套接字标识
        /// </summary>
        private Guid _id;
        /// <summary>
        /// tcp 连接套接字
        /// </summary>
        private Socket _socket = null;
        /// <summary>
        /// 接收数据线程
        /// </summary>
        private Thread _recvDataThd = null;
        /// <summary>
        /// 心跳检查线程
        /// </summary>
        private Thread _heartBeatThd = null;
        /// <summary>
        /// 数据处理线程
        /// </summary>
        private Thread _dataDealThd = null;
        /// <summary>
        /// 信号量
        /// </summary>
        private Semaphore _semaphore = new Semaphore(0, int.MaxValue);
        /// <summary>
        /// 数据队列锁
        /// </summary>
        private object _queueLock = new object();
        /// <summary>
        /// 数据缓存队列
        /// </summary>
        private Queue<byte[]> _dataQueue = new Queue<byte[]>();
        /// <summary>
        /// 数据解析类
        /// </summary>
        private DataAnalyzer _analyzer = null;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="socket"></param>
        public Client(Guid guid, Socket socket)
        {
            _id = guid;
            _socket = socket;
            _analyzer = new DataAnalyzer();
            RemoteIP = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();
            Utils.ShowInfo(this, string.Format("[Service] 客户端 {0} 连接成功。", RemoteIP));

            // 建立连接时就开启线程
            _recvDataThd = new Thread(new ThreadStart(RecvData));
            _recvDataThd.IsBackground = true;
            _recvDataThd.Name = "Socket Receive Data Thread: " + _id;
            _recvDataThd.Start();

            _heartBeatThd = new Thread(new ThreadStart(KeepAlive));
            _heartBeatThd.IsBackground = true;
            _heartBeatThd.Name = "Socket Heart Beat Thread: " + _id;
            _heartBeatThd.Start();

            _dataDealThd = new Thread(new ThreadStart(DealData));
            _dataDealThd.IsBackground = true;
            _dataDealThd.Name = "Socket Deal Data Thread: " + _id;
            _dataDealThd.Start();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _dataQueue.Clear();

            if (_socket != null)
            {
                _socket.Close();
                _socket = null;
            }

            if (_recvDataThd != null && _recvDataThd.IsAlive)
            {
                try { _recvDataThd.Abort(); } catch { }
                _recvDataThd = null;
            }

            if (_heartBeatThd != null && _heartBeatThd.IsAlive)
            {
                try { _heartBeatThd.Abort(); } catch { }
                _heartBeatThd = null;
            }

            if (_dataDealThd != null && _dataDealThd.IsAlive)
            {
                try { _dataDealThd.Abort(); } catch { }
                _dataDealThd = null;
            }
        }

        /// <summary>
        /// 接收数据线程
        /// </summary>
        private void RecvData()
        {
            byte[] buffer = new byte[1024 * 1024 * 10];   // 开辟 10M 的缓存

            while (true)
            {
                try
                {
                    int count = _socket.Receive(buffer, buffer.Length, SocketFlags.None);
                    if (count > 0)
                    {
                        byte[] data = new byte[count];
                        Buffer.BlockCopy(buffer, 0, data, 0, count);
                        lock (_queueLock)
                        {
                            _dataQueue.Enqueue(data);
                        }
                        _semaphore.Release();
                    }
                }
                catch
                {
                    //Utils.ShowInfo(this, ex.Message);
                }
            }
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

                AnalyzeData(data);
            }
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="data"></param>
        private void AnalyzeData(byte[] data)
        {
            // 数据类型
            short dataType = BitConverter.ToInt16(data, 5);

            switch (dataType)
            {
                // 语音识别
                case 0x0501:
                    Utils.ShowInfo(this, string.Format("[{0}] 收到 0x0501（语音识别）数据。", RemoteIP));
                    Deal0x0501(data);
                    break;

                // 获取可识别的语种列表
                case 0x0502:
                    Utils.ShowInfo(this, string.Format("[{0}] 收到 0x0502（获取可识别的语种列表）数据。", RemoteIP));
                    Deal0x0502();
                    break;

                // Text 到 LanguaeType 的映射
                case 0x0503:
                    Utils.ShowInfo(this, string.Format("[{0}] 收到 Ox0503（Text 到 LanguaeType 的映射）数据。", RemoteIP));
                    Deal0x0503(data);
                    break;

                // Name 到 LanguaeType 的映射
                case 0x0504:
                    Utils.ShowInfo(this, string.Format("[{0}] 收到 Ox0504（Name 到 LanguaeType 的映射）数据。", RemoteIP));
                    Deal0x0504(data);
                    break;

                // 翻译
                case 0x0505:
                    Utils.ShowInfo(this, string.Format("[{0}] 收到 0x0505（翻译）数据。", RemoteIP));
                    Deal0x0505(data);
                    break;

                // 获取可翻译的语种列表
                case 0x0506:
                    Utils.ShowInfo(this, string.Format("[{0}] 收到 0x0506（获取支持翻译的语种列表）数据。", RemoteIP));
                    Deal0x0506();
                    break;

                // 未处理的数据类型
                default:
                    Utils.ShowInfo(this, string.Format("[{0}] 收到 {1}（获取支持翻译的语种列表）数据。但未做处理。", RemoteIP, string.Format("{0:X}", dataType)));
                    break;
            }
        }

        /// <summary>
        /// 语音识别
        /// </summary>
        /// <param name="data"></param>
        private void Deal0x0501(byte[] data)
        {
            LanguageType languageType;
            byte[] audioData;

            bool ret = _analyzer.Analyze0x0501(data, out languageType, out audioData);
            if (ret == false)
            {
                string msg = "[Service] 解析 0x0501 数据（语音识别）失败。";
                Utils.ShowInfo(this, msg);
                return;
            }

            string recgResult;
            ret = Utils.Asr.AudioRecog(audioData, languageType, out recgResult);

            byte[] sendData = _analyzer.Combine0x0601(recgResult, ret);
            Send(sendData, "0x0601 （识别结果）");
        }

        /// <summary>
        /// 获取可识别的语种列表
        /// </summary>
        private void Deal0x0502()
        {
            List<Language> languageList = Utils.Asr.GetLanguageList();
            if (languageList != null && languageList.Count > 0)
            {
                byte[] data = _analyzer.Combine0x0602(languageList, 0x0602);
                Send(data, " 0x0602 （可识别语种列表）");
            }
        }

        /// <summary>
        /// Text 到 LanguageType 的映射
        /// </summary>
        /// <param name="data"></param>
        private void Deal0x0503(byte[] data)
        {
            string text;
            bool ret = _analyzer.Analyze0x0503_4(data, out text);
            if (ret == false)
            {
                Utils.ShowInfo(this, "[Service] 解析 0x0503 数据失败。");
                return;
            }

            LanguageType type = Utils.Asr.Text2LanguageType(text);

            byte[] sendData = _analyzer.Combine0x0603(type);
            Send(sendData, "0x0603");
        }

        /// <summary>
        /// Name 到 LanguageType 的映射
        /// </summary>
        /// <param name="data"></param>
        private void Deal0x0504(byte[] data)
        {
            string name;
            bool ret = _analyzer.Analyze0x0503_4(data, out name);
            if (ret == false)
            {
                Utils.ShowInfo(this, "[Service] 解析 0x0504 数据失败。");
                return;
            }

            LanguageType type = Utils.Asr.Name2LanguageType(name);

            byte[] sendData = _analyzer.Combine0x0603(type);
            Send(sendData, "0x0603");
        }

        /// <summary>
        /// 翻译
        /// </summary>
        /// <param name="data"></param>
        private void Deal0x0505(byte[] data)
        {
            LanguageType from;
            LanguageType to;
            string text;

            bool ret = _analyzer.Analyze0x0505(data, out text, out from, out to);
            if (ret == false)
            {
                Utils.ShowInfo(this, "[Service] 解析 0x0505 数据失败。");
                return;
            }

            string transResult;
            ret = TranslateFun.GetTranslate().Trans(text, from, out transResult, to);

            byte[] sendData = _analyzer.Combine0x0605(ret, transResult);
            Send(sendData, " 0x0605 (翻译结果)");
        }

        /// <summary>
        /// 获取支持的翻译语种列表
        /// </summary>
        private void Deal0x0506()
        {
            List<Language> languageList = TranslateFun.GetTranslate().GetTransLanguages();
            if (languageList != null && languageList.Count > 0)
            {
                byte[] data = _analyzer.Combine0x0602(languageList, 0x0606);
                Send(data, " 0x0606 (支持翻译的语种列表)");
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="dataType">数据类型</param>
        private void Send(byte[] data, string dataType)
        {
            try
            {
                _socket.Send(data);
                Utils.ShowInfo(this, string.Format("[{0}] 发送数据 {1} 成功。", RemoteIP, dataType));
            }
            catch (Exception ex)
            {
                Utils.ShowInfo(this, string.Format("[{0}] 发送数据 {1} 失败：{2}", RemoteIP, dataType, ex.Message));
            }
        }

        /// <summary>
        /// 心跳检查线程
        /// </summary>
        private void KeepAlive()
        {
            try
            {
                byte[] bytes = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0xF4, 0x01, 0x00, 0x00 };
                _socket.IOControl(IOControlCode.KeepAliveValues, bytes, null);
                _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                byte[] msg = Encoding.Default.GetBytes("");

                while (true)
                {
                    _socket.Send(msg);
                    Thread.Sleep(1000);
                }
            }
            catch
            {
                if (Disconnected != null)
                {
                    Disconnected.Invoke(this, new DisconnectedEventArgs() { Id = _id });
                }

                // 连接断开之后就要释放资源
                Dispose();

                Utils.ShowInfo(this, string.Format("[Service] 客户端 {0} 断开连接。", RemoteIP));
            }
        }
    }
}
