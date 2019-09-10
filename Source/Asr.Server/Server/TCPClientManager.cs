/*********************************************************************************************
 *	
 * 文件名称:    TCPClientManager.cs
 * 
 * 描    述：   TCP 连接客户端，用于接收数据处理数据等
 *
 * 作    者:    王 喜 进
 *	
 * 创作日期:    2019-3-27
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AsrServer
{
    /// <summary>
    /// 客户端管理类
    /// </summary>
    internal class TCPClientManager : IDisposable
    {
        /// <summary>
        /// TCP 套接字列表（线程安全的字典）
        /// </summary>
        private ConcurrentDictionary<Guid, TCPClient> _clientDic = new ConcurrentDictionary<Guid, TCPClient>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public TCPClientManager()
        {

        }

        /// <summary>
        /// 新加入客户端连接
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="client">客户端类</param>
        public void Add(Guid id, TCPClient client)
        {
            _clientDic.TryAdd(id, client);
            client.Disconnected += Client_Disconnected;
            UpdateUI();
        }

        /// <summary>
        /// 释放所有资源
        /// </summary>
        public void Dispose()
        {
            // 断开所有连接，更新UI客户端
            foreach (TCPClient c in _clientDic.Values)
            {
                c.Dispose();
            }

            _clientDic.Clear();
            UpdateUI();
        }

        // 端口连接，移除客户端
        private void Client_Disconnected(object sender, DisconnectedEventArgs e)
        {
            TCPClient client;
            _clientDic.TryRemove(e.Id, out client);
            if (client != null)
            {
                client.Disconnected -= Client_Disconnected;
                UpdateUI();
            }
        }

        /// <summary>
        /// 刷新界面客户端连接
        /// </summary>
        private void UpdateUI()
        {
            List<string> ipList = new List<string>();

            foreach (var c in _clientDic.Values)
            {
                ipList.Add(c.RemoteIP);
            }

            Utils.UpdateClientList(this, ipList);
        }
    }
}
