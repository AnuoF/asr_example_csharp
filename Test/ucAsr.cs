using AnuoLibrary;
using AnuoLibrary.Asr;
using AnuoLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Threading;
using System.Windows.Forms;

namespace Test
{
    public partial class ucAsr : UserControl
    {
        private List<Language> _languageList = null;
        private IAsr _asr = null;

        public ucAsr()
        {
            InitializeComponent();

            // 1) 获取 Asr 语音识别功能接口
            _asr = AsrFun.Asr;

            // 2) 获取可识别的语种列表
            _languageList = _asr.GetLanguageList();
            foreach (var item in _languageList)
            {
                cmbLanguage.Items.Add(item.Text);
            }

            cmbLanguage.SelectedIndex = 1;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            dlg.Filter = "pcm files (*.pcm)|*.pcm|All files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtAudioFile.Text = dlg.FileName;
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = txtAudioFile.Text.Trim();
                if (string.IsNullOrEmpty(fileName)) return;
                if (!File.Exists(fileName)) return;

                SoundPlayer player = new SoundPlayer(fileName);
                player.Play();
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }
        }

        private void btnRecog_Click(object sender, EventArgs e)
        {
            string fileName = txtAudioFile.Text.Trim();
            if (string.IsNullOrEmpty(fileName)) return;
            if (!File.Exists(fileName)) return;

            btnRecog.Enabled = false;
            string text = cmbLanguage.Text;

            ThreadPool.QueueUserWorkItem(o =>
            {
                WriteLine("Start ...");

                string result = string.Empty;
                byte[] data = File.ReadAllBytes(fileName);
                LanguageType type = GetLanguageType(text);

                Stopwatch sw = new Stopwatch();
                sw.Start();
                // 3）语音识别
                _asr.AudioRecog(data, type, out result);
                sw.Stop();

                WriteLine(result);
                WriteLine("Finish! 耗时：" + sw.ElapsedMilliseconds + " 毫秒。\n");

                EnableBtn(true);
            });
        }

        private LanguageType GetLanguageType(string text)
        {
            Language language = _languageList.Find(o => o.Text == text);
            return Utils.Text2LanguageType(language.Text);
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

        private void EnableBtn(bool enable)
        {
            if (btnRecog.InvokeRequired)
            {
                MethodInvoker invoker = new MethodInvoker(() => EnableBtn(enable));
                btnRecog.Invoke(invoker);
            }
            else
            {
                btnRecog.Enabled = enable;
            }
        }
    }
}
