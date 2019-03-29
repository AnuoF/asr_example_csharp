/*********************************************************************************************
 *	
 * 文件名称:    Utils.cs
 * 
 * 描    述：   全局类
 *
 * 作    者:    Anuo.
 *	
 * 创作日期:    2019-3-27
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using AsrCommon.Asr;
using System;
using System.Collections.Generic;

namespace AsrService
{
    /// <summary>
    /// 全局类
    /// </summary>
    internal class Utils
    {

        /// <summary>
        /// 语音识别接口
        /// </summary>
        public static IAsr Asr = null;

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
}
