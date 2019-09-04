/*********************************************************************************************
 *	
 * 文件名称:    Client.cs
 * 
 * 描    述：   TCP 通信客户端
 *
 * 作    者:    王 喜 进
 *	
 * 创作日期:    2019-3-26
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using AsrLibrary.Entity;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Xml;

namespace AsrLibrary.Proxy
{
    /// <summary>
    /// TCP 通信客户端
    /// </summary>
    internal class Client : IDisposable
    {
        /// <summary>
        /// 客户端是否建立连接
        /// </summary>
        public bool IsConnected
        {
            get { return _socket != null ? _socket.Connected : false; }
        }

        /// <summary>
        /// 建立连接事件
        /// </summary>
        public event EventHandler OnConnected;
        /// <summary>
        /// 断开连接事件
        /// </summary>
        public event EventHandler OnDisconnected;
        /// <summary>
        /// 引发错误事件
        /// </summary>
        public event EventHandler<MsgEventArgs> OnError;
        /// <summary>
        /// 数据到达事件
        /// </summary>
        public event EventHandler<RecvDataEventArgs> OnData;

        /// <summary>
        /// TCP连接套接字
        /// </summary>
        private Socket _socket = null;
        /// <summary>
        /// TCP连接和心跳检查线程
        /// </summary>
        private Thread _connHearThd = null;
        /// <summary>
        /// 接收数据线程
        /// </summary>
        private Thread _recvDataThd = null;
        /// <summary>
        /// AsrService IP地址
        /// </summary>
        private string _ip = "127.0.0.1";
        /// <summary>
        /// AsrService 端口
        /// </summary>
        private int _port = 8888;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Client()
        {
            // 读取配置文件
            LoadInfoFromConfig();
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            _connHearThd = new Thread(new ThreadStart(ConnectAndHeartBeat));
            _connHearThd.IsBackground = true;
            _connHearThd.Name = "Tcp connect and heart beat thread";
            _connHearThd.Start();

            _recvDataThd = new Thread(new ThreadStart(RecvData));
            _recvDataThd.IsBackground = true;
            _recvDataThd.Name = "Recive data thread";
            _recvDataThd.Start();
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">数据</param>
        public bool Send(byte[] data)
        {
            if (_socket != null && _socket.Connected)
            {
                try
                {
                    _socket.Send(data);
                    return true;
                }
                catch (Exception ex)
                {
                    if (OnError != null)
                    {
                        OnError.Invoke(this, new MsgEventArgs() { Msg = ex.Message });
                    }

                    return false;
                }
            }
            else
            {
                if (OnError != null)
                {
                    OnError.Invoke(this, new MsgEventArgs() { Msg = "发送数据失败，Tcp 未建立连接。" });
                }

                return false;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Stop();
        }

        /// <summary>
        /// 停止
        /// </summary>
        private void Stop()
        {
            if (_connHearThd != null && _connHearThd.IsAlive)
            {
                try { _connHearThd.Abort(); } catch { }
                _connHearThd = null;
            }

            if (_recvDataThd != null && _recvDataThd.IsAlive)
            {
                try { _recvDataThd.Abort(); } catch { }
                _recvDataThd = null;
            }

            if (_socket != null)
            {
                _socket.Close();
                _socket = null;
            }
        }

        /// <summary>
        /// 接收数据线程
        /// </summary>
        private void RecvData()
        {
            byte[] buffer = new byte[1024 * 1024 * 10];

            while (true)
            {
                if (IsConnected == false)
                {
                    Thread.Sleep(10);
                    continue;
                }

                try
                {
                    _socket.ReceiveBufferSize = buffer.Length;
                    int count = _socket.Receive(buffer, buffer.Length, SocketFlags.None);
                    if (count > 0)
                    {
                        byte[] data = new byte[count];
                        Buffer.BlockCopy(buffer, 0, data, 0, count);
                        if (OnData != null)
                        {
                            OnData.Invoke(this, new RecvDataEventArgs() { Data = data });
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (OnError != null)
                    {
                        OnError.Invoke(this, new MsgEventArgs() { Msg = ex.Message });
                    }
                }
            }
        }

        /// <summary>
        /// TCP 连接和心跳检查线程
        /// </summary>
        private void ConnectAndHeartBeat()
        {
            while (true)
            {
                if (IsConnected == false)
                {
                    if (_socket == null)
                    {
                        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                        byte[] bytes = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0xF4, 0x01, 0x00, 0x00 };
                        _socket.IOControl(IOControlCode.KeepAliveValues, bytes, null);
                        _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                    }

                    try
                    {
                        _socket.Connect(IPAddress.Parse(_ip), _port);

                        // 连接成功，发布事件
                        if (OnConnected != null)
                        {
                            try
                            {
                                OnConnected.Invoke(this, new EventArgs());
                            }
                            catch (Exception ex)
                            {
                                // 此处回调方如果出现异常，可能会影响到我这边的流程，所以将异常捕获。
                                Log.WriteLog("OnConnected 事件引发异常：" + ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (_socket != null)
                        {
                            // 断开套接字连接后，只能通过异步方式再次重新连接，而且只能连接到不同的 EndPoint。  
                            // 在操作完成前，在将不会退出的线程上必须调用 BeginConnect。
                            _socket.Close();
                            _socket = null;
                        }

                        if (OnError != null && !(ex is ThreadAbortException))
                        {
                            OnError.Invoke(this, new MsgEventArgs() { Msg = ex.Message });
                        }
                    }
                }
                else
                {
                    // 检查心跳
                    try
                    {
                        _socket.Send(new byte[0]);
                    }
                    catch (Exception ex)
                    {
                        if (OnDisconnected != null)
                        {
                            OnDisconnected.Invoke(this, new EventArgs());
                        }

                        if (_socket != null)
                        {
                            // 断开套接字连接后，只能通过异步方式再次重新连接，而且只能连接到不同的 EndPoint。  
                            // 在操作完成前，在将不会退出的线程上必须调用 BeginConnect。
                            _socket.Close();
                            _socket = null;
                        }

                        if (OnError != null && !(ex is ThreadAbortException))
                        {
                            OnError.Invoke(this, new MsgEventArgs() { Msg = ex.Message });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        private void LoadInfoFromConfig()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Utils.ConfigName);

                XmlNode node = doc.SelectSingleNode("./configuration/mode/service/ip");
                _ip = node.InnerXml;
                node = doc.SelectSingleNode("./configuration/mode/service//port");
                _port = Convert.ToInt32(node.InnerXml);
            }
            catch (Exception ex)
            {
                Log.WriteLog("读取配置文件异常：" + ex.Message);
            }
        }
    }
}
