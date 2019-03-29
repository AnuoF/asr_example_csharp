using AsrCommon.Asr;
using AsrCommon.Entity;
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

            panel2.Enabled = false;
            // 1) 获取 Asr 语音识别功能接口
            //_asr = AsrLibrary.AsrFun.GetAsr();
            _asr = AsrProxy.AsrFun.GetAsr();            
        }

        public void Load()
        {
            _asr_OnConnected(null, null);
        }

        // 建立
        private void _asr_OnConnected(object sender, EventArgs e)
        {
            // 2) 获取可识别的语种列表
            _languageList = _asr.GetLanguageList();
            ShowLanguageList();
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
                string pcmFileName = txtAudioFile.Text.Trim();
                if (string.IsNullOrEmpty(pcmFileName)) return;
                if (!File.Exists(pcmFileName)) return;

                string mp3FileName = pcmFileName.Replace("pcm", "wav");  // @"C:\Users\Administrator\Desktop\语音识别音频文件\武汉话 - Rec 0002.wav";//
                if (!File.Exists(mp3FileName)) return;

                SoundPlayer player = new SoundPlayer(mp3FileName);
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

                //WaveFormat waveFormat = WaveFormat.PCM_MONO;
                //waveFormat.nSamplesPerSec = 16000;        
                //waveFormat.nAvgBytesPerSec = 16000 * 2;
                //_asr.AudioRecog(data, type, out result, waveFormat);    // 两个接口都可以调

                sw.Stop();

                WriteLine(result);
                WriteLine("Finish! 耗时：" + sw.ElapsedMilliseconds + " 毫秒。\n");

                EnableBtn(true);
            });
        }

        private LanguageType GetLanguageType(string text)
        {
            Language language = _languageList.Find(o => o.Text == text);
            return _asr.Text2LanguageType(language.Text);
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

        private void ShowLanguageList()
        {
            if (_languageList == null) return;

            if (this.InvokeRequired)
            {
                MethodInvoker invoker = new MethodInvoker(() => ShowLanguageList());
                this.Invoke(invoker);
            }
            else
            {
                cmbLanguage.Items.Clear();

                foreach (var item in _languageList)
                {
                    cmbLanguage.Items.Add(item.Text);
                }

                if(cmbLanguage.Items.Count>=2)
                {
                    cmbLanguage.SelectedIndex = 1;
                }

                panel2.Enabled = true;
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

        #region Tracker800 的音频文件测试

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "请选择 Tracker800 保存的音频文件";
            dlg.Multiselect = false;
            dlg.Filter = "wav files (*.wav)|*.wav|All files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtTracker.Text = dlg.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string wavFileName = txtTracker.Text.Trim();
                if (string.IsNullOrEmpty(wavFileName)) return;
                if (!File.Exists(wavFileName)) return;

                SoundPlayer player = new SoundPlayer(wavFileName);
                player.Play();
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            string fileName = txtTracker.Text.Trim();
            if (!File.Exists(fileName)) return;

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
                WaveFormat waveFormat = WaveFormat.PCM_MONO;
                //waveFormat.nSamplesPerSec = 22050;        // 默认值就是这样的
                //waveFormat.nAvgBytesPerSec = 22050 * 2;

                _asr.AudioRecog(data, type, out result, waveFormat);
                sw.Stop();

                WriteLine(result);
                WriteLine("Finish! 耗时：" + sw.ElapsedMilliseconds + " 毫秒。\n");

                EnableBtn(true);
            });
        }

        private void btnText2LanguageType_Click(object sender, EventArgs e)
        {
            string text = cmbLanguage.Text;
            if (string.IsNullOrEmpty(text))
                return;

            LanguageType languageType = _asr.Text2LanguageType(text);

            languageType = _asr.Name2LanguageType("Sichuan");

            languageType = _asr.Name2LanguageType("English");

            int i = 0;
        }

        #endregion

        //private void btnStart_Click(object sender, EventArgs e)
        //{
        //    if (btnStart.Tag.ToString() == "start")
        //    {
        //        _asr.Start();

        //        btnStart.Tag = "stop";
        //        btnStart.Text = "Stop";
        //    }
        //    else
        //    {
        //        _asr.Stop();

        //        btnStart.Tag = "start";
        //        btnStart.Text = "Start";
        //    }
        //}
    }
}
