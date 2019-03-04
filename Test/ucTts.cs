using AnuoLibrary.Tts;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Test
{
    public partial class ucTts : UserControl
    {
        private TtsBaidu _tts = null;

        public ucTts()
        {
            InitializeComponent();

            _tts = new TtsBaidu("cFSLHg4rW9N6x4Sc9NZ4dSvc", "301ORzSxeLoagyrNb4qElEMLzeGbMbG6");
        }

        private void btnChooseTxt_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = dlg.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.AddExtension = true;
            dlg.CheckPathExists = true;
            dlg.Title = "保存音频文件";
            dlg.Filter = "mp3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            dlg.DefaultExt = ".mp3";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtSave.Text = dlg.FileName;
            }
        }

        private void btnTts_Click(object sender, EventArgs e)
        {
            string fileName = txtFile.Text.Trim();
            if (string.IsNullOrEmpty(fileName)) return;
            if (!File.Exists(fileName)) return;

            string saveName = txtSave.Text.Trim();
            if (string.IsNullOrEmpty(saveName)) return;

            ThreadPool.QueueUserWorkItem(o =>
            {
                TtsFile(fileName, saveName);
            });
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

        private void TtsFile(string fileName, string saveName)
        {
            EnableBtn(false);
            WriteLine("开始合成...");

            byte[] data = File.ReadAllBytes(fileName);
            int len = 1024;    // 每次传1024长度
            int total_length = data.Length;    // 总长度
            int send_length = 0;               // 已发送的长度

            while (true)
            {
                if (send_length >= total_length)
                {
                    break;
                }

                if (total_length - send_length <= len)
                {
                    len = total_length - send_length;
                }

                byte[] tempData = new byte[len];
                Buffer.BlockCopy(data, send_length, tempData, 0, len);

                string errMsg;
                byte[] ttsData;
                string text = System.Text.Encoding.UTF8.GetString(tempData);
                bool ret = _tts.Tts(text, out ttsData, out errMsg);
                if (ret)
                {
                    WriteFile(saveName, ttsData);
                }
                else
                {
                    WriteLine("合成：" + text + " 失败：" + errMsg);
                }

                send_length += len;
            }

            EnableBtn(true);
            WriteLine("合成结束，查看文件 " + saveName);
        }

        private void WriteFile(string fileName, byte[] data)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
            {
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
            }
        }

        private void EnableBtn(bool enable)
        {
            if (btnTts.InvokeRequired)
            {
                MethodInvoker invoker = new MethodInvoker(() => EnableBtn(enable));
                btnTts.Invoke(invoker);
            }
            else
            {
                btnTts.Enabled = enable;
            }
        }
    }
}