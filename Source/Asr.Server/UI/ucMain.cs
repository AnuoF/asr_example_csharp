using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace AsrServer
{
    public partial class ucMain : UserControl
    {
        /// <summary>
        /// tcp 服务端类
        /// </summary>
        private TCPServer _server = null;

        public ucMain()
        {
            InitializeComponent();

            LoadInfoFromConfig();

            Utils.ShowInfoEvent += Utils_ShowInfoEvent;
            Utils.UpdateClientListEvent += Utils_UpdateClientListEvent;
            _server = new TCPServer();
        }


        // 启动或停止
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Tag.ToString() == "start")
            {
                bool ret = VerifyInput();
                if (!ret) return;

                SetSaveInfo();

                // 启动
                ret = _server.Start();
                if (ret)
                {
                    btnStart.Text = "停  止";
                    btnStart.Tag = "stop";
                    gbServieInfo.Enabled = false;
                    ShowInfo("[Server] 开始监听客户端连接...");
                }
            }
            else
            {
                // 停止
                _server.Stop();

                btnStart.Text = "启  动";
                btnStart.Tag = "start";
                gbServieInfo.Enabled = true;
                ShowInfo("[Server] 停止监听客户端连接。");
            }
        }

        /// <summary>
        /// 从配置文件加载信息
        /// </summary>
        private void LoadInfoFromConfig()
        {
            try
            {
                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config\\Server.xml");
                XmlDocument doc = new XmlDocument();
                doc.Load(configPath);

                string ip = doc.SelectSingleNode("./config/serviceinfo/ip").InnerXml;
                int port = Convert.ToInt32(doc.SelectSingleNode("./config/serviceinfo/port").InnerXml);

                txtIP.Text = ip;
                txtPort.Text = port.ToString();
            }
            catch (Exception ex)
            {
                ShowInfo(ex.Message);
            }
        }

        /// <summary>
        /// 显示运行日志
        /// </summary>
        /// <param name="msg"></param>
        private void ShowInfo(string msg)
        {
            if (rtbInfo.InvokeRequired)
            {
                MethodInvoker invoker = new MethodInvoker(() => ShowInfo(msg));
                rtbInfo.Invoke(invoker);
            }
            else
            {
                if (rtbInfo.Lines.Length > 100)
                    rtbInfo.Clear();

                msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " >>> " + msg + "\n";
                rtbInfo.AppendText(msg);
                rtbInfo.ScrollToCaret();

                LogManager.WriteLog(msg);
            }
        }


        private void UpdateClient(List<string> ipList)
        {
            if (lbClient.InvokeRequired)
            {
                MethodInvoker invoker = new MethodInvoker(() => UpdateClient(ipList));
                lbClient.Invoke(invoker);
            }
            else
            {
                lbClient.Items.Clear();
                lbClient.Items.AddRange(ipList.ToArray());
            }
        }

        /// <summary>
        /// 验证数据的正确性
        /// </summary>
        /// <returns></returns>
        private bool VerifyInput()
        {
            try
            {
                if (string.IsNullOrEmpty(txtIP.Text))
                    return false;

                if (string.IsNullOrEmpty(txtIP.Text.Trim()))
                    return false;

                if (string.IsNullOrEmpty(txtPort.Text))
                    return false;

                if (string.IsNullOrEmpty(txtPort.Text.Trim()))
                    return false;

                int port = Convert.ToInt32(txtPort.Text);

                return true;
            }
            catch (Exception ex)
            {
                ShowInfo(ex.Message);
                return true;
            }
        }

        /// <summary>
        /// 设置和保存信息
        /// </summary>
        private void SetSaveInfo()
        {
            string ip = txtIP.Text.Trim();
            int port = Convert.ToInt32(txtPort.Text);

            _server.SetInfo(ip, port);

            try
            {
                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config\\Server.xml");
                XmlDocument doc = new XmlDocument();
                doc.Load(configPath);

                doc.SelectSingleNode("./config/serviceinfo/ip").InnerXml = ip;
                doc.SelectSingleNode("./config/serviceinfo/port").InnerXml = txtPort.Text;

                doc.Save(configPath);
            }
            catch (Exception ex)
            {
                ShowInfo(ex.Message);
            }
        }

        // 刷新客户端列表
        private void Utils_UpdateClientListEvent(object sender, UpdateClientListEventArgs e)
        {
            UpdateClient(e.ClientList);
        }

        private void Utils_ShowInfoEvent(object sender, ShowInfoEventArgs e)
        {
            ShowInfo(e.Msg);
        }

    }
}
