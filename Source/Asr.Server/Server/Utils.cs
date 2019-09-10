/*********************************************************************************************
 *	
 * 文件名称:    Utils.cs
 * 
 * 描    述：   全局类
 *
 * 作    者:    王 喜 进
 *	
 * 创作日期:    2019-3-27
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using System;
using System.Collections.Generic;

namespace AsrServer
{
    /// <summary>
    /// 全局类
    /// </summary>
    internal class Utils
    {

        /// <summary>
        /// 显示运行信息事件
        /// </summary>
        public static event EventHandler<ShowInfoEventArgs> ShowInfoEvent;

        /// <summary>
        /// 帆布显示运行消息事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        public static void ShowInfo(object sender, string msg)
        {
            if (ShowInfoEvent != null)
            {
                ShowInfoEvent.Invoke(sender, new ShowInfoEventArgs() { Msg = msg });
            }
        }

        /// <summary>
        /// 更新客户端列表事件
        /// </summary>
        public static event EventHandler<UpdateClientListEventArgs> UpdateClientListEvent;

        /// <summary>
        /// 发布更新客户端列表事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="clientList"></param>
        public static void UpdateClientList(object sender, List<string> clientList)
        {
            if (UpdateClientListEvent != null)
            {
                UpdateClientListEvent.Invoke(sender, new UpdateClientListEventArgs() { ClientList = clientList });
            }
        }
    }

    /// <summary>
    /// 显示信息事件结构体
    /// </summary>
    internal class ShowInfoEventArgs : EventArgs
    {
        public string Msg;
    }

    /// <summary>
    /// 更新客户端列表事件结构体
    /// </summary>
    internal class UpdateClientListEventArgs : EventArgs
    {
        public List<string> ClientList;
    }

    /// <summary>
    /// 接收到的数据结构体
    /// </summary>
    internal class RecvData
    {
        public byte[] Data;
    }

    /// <summary>
    /// 断开连接事件结构体
    /// </summary>
    internal class DisconnectedEventArgs : EventArgs
    {
        public Guid Id;
    }
}
