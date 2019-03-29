/*********************************************************************************************
 *	
 * 文件名称:    DisconnectedEventArgs.cs
 * 
 * 描    述：   断开连接事件结构体
 *
 * 作    者:    Anuo.
 *	
 * 创作日期:    2019-3-27
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using System;

namespace AsrService
{
    /// <summary>
    /// 断开连接事件结构体
    /// </summary>
    internal class DisconnectedEventArgs : EventArgs
    {
        public Guid Id;
    }
}
