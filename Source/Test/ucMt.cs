using Asr.Client;
using Asr.Public;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Test
{
    public partial class ucMt : UserControl
    {
        private AsrClient _client = null;

        public ucMt(AsrClient client)
        {
            InitializeComponent();

            _client = client;

            cmbTo.SelectedIndex = 0;
        }

        private void btnTranslate_Click(object sender, EventArgs e)
        {
            string text = txtSrc.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            string from = cmbFrom.Text;
            if (string.IsNullOrEmpty(from)) return;

            LanguageType languageType = _client.IAsr.Text2LanguageType(from);
            string result = string.Empty;

            // 2）翻译
            bool ret = _client.ITranslate.Trans(text, languageType, out result);
            if (ret)
            {
                txtResult.Text = result;
            }
            else
            {
                WriteLine(result);
            }
        }

        /// <summary>
        /// 加载基础信息
        /// </summary>
        public void LoadInfo()
        {
            if (cmbFrom.InvokeRequired)
            {
                MethodInvoker invoker = new MethodInvoker(() => LoadInfo());
                cmbFrom.Invoke(invoker);
            }
            else
            {
                cmbFrom.Items.Clear();
                List<Language> list = _client.ITranslate.GetTransLanguages();

                if (list != null && list.Count > 0)
                {
                    foreach (Language lan in list)
                    {
                        string text = lan.Text;
                        cmbFrom.Items.Add(text);
                    }
                }
            }
        }

        // show info
        private void WriteLine(string msg)
        {
            if (rtbInfo.InvokeRequired)
            {
                MethodInvoker invoker = new MethodInvoker(() => WriteLine(msg));
                rtbInfo.Invoke(invoker);
            }
            else
            {
                msg = "\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " >>> " + msg;
                rtbInfo.AppendText(msg);
                rtbInfo.ScrollToCaret();
            }
        }
    }
}
