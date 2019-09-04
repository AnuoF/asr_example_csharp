/*********************************************************************************************
 *	
 * 文件名称:    BuzProcess.cs
 * 
 * 描    述：   业务逻辑处理类，尽量将业务逻辑也界面类分离开，便于后期维护集成。
 *
 * 作    者:    王 喜 进
 *	
 * 创作日期:    2019-3-27
 *
 * 备    注:	
 *                                        
*********************************************************************************************/

using System;

namespace AsrServer
{
    /// <summary>
    /// 业务逻辑处理类
    /// </summary>
    internal class BuzProcess : IDisposable
    {
        /// <summary>
        /// 服务端
        /// </summary>
        private Server _server = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BuzProcess()
        {
            _server = new Server();
        }

        /// <summary>
        /// 启动
        /// </summary>
        public bool Start()
        {
            return _server.Start();
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            _server.Stop();
        }

        // 释放资源
        public void Dispose()
        {
            _server.Dispose();
        }

        /// <summary>
        /// 设置配置信息
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void SetInfo(string ip, int port)
        {
            _server.SetInfo(ip, port);
        }

    }
}
