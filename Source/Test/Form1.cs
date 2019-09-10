using Asr.Client;
using System;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        private ucAsrClient asr;
        private ucMt mt;
        private AsrClient _client;

        public Form1()
        {
            InitializeComponent();

            // 实例化客户端
            _client = new AsrClient("127.0.0.1", 8888);
            _client.OnInitialized += _asr_OnConnected;

            asr = new ucAsrClient(_client) { Dock = DockStyle.Fill };
            tabPage1.Controls.Add(asr);
            mt = new ucMt(_client) { Dock = DockStyle.Fill };
            tabPage2.Controls.Add(mt);

            // 建立连接
            _client.ConnectAsync();
        }

        private void _asr_OnConnected(object sender, EventArgs e)
        {
            asr.LoadInfo();
            mt.LoadInfo();
        }
    }
}
