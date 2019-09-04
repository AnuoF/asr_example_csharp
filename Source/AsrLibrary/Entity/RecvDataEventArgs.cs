/*********************************************************************************************
 *	
 * 文件名称:    RecvDataEventArgs.cs
 * 
 * 描    述：   数据接收事件结构体
 *
 * 作    者:    王 喜 进
 *	
 * 创作日期:    2019-3-27
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using System;

namespace AsrLibrary.Entity
{
    /// <summary>
    /// 数据接收事件结构体
    /// </summary>
    internal class RecvDataEventArgs : EventArgs
    {
        public byte[] Data;
    }
}
