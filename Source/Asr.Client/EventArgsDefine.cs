using System;

namespace Asr.Client
{
    /// <summary>
    /// 数据接收事件结构体
    /// </summary>
    internal class RecvDataEventArgs : EventArgs
    {
        public byte[] Data;
    }

    /// <summary>
    /// 消息事件结构体
    /// </summary>
    internal class MsgEventArgs : EventArgs
    {
        public string Msg;
    }
}
