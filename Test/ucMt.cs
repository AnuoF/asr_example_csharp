using AnuoLibrary;
using AnuoLibrary.Entity;
using AnuoLibrary.Mt;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Test
{
    public partial class ucMt : UserControl
    {
        private ITranslate _trans = null;

        public ucMt()
        {
            InitializeComponent();

            _trans = new Translate();

            LoadInfo();
            cmbTo.SelectedIndex = 0;
        }

        private void btnTranslate_Click(object sender, EventArgs e)
        {
            string text = txtSrc.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            string from = cmbFrom.Text;
            if (string.IsNullOrEmpty(from)) return;

            LanguageType languageType = Utils.Text2LanguageType(from);
            string result = string.Empty;

            bool ret = _trans.Trans(text, languageType, out result);
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
        private void LoadInfo()
        {
            List<Language> list = _trans.GetTransLanguages();
            if (list != null && list.Count > 0)
            {
                foreach (Language lan in list)
                {
                    string text = lan.Text;
                    cmbFrom.Items.Add(text);
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
