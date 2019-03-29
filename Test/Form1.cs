using AsrCommon.Asr;
using System;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        private ucAsr asr;
        private ucMt mt;

        public Form1()
        {
            InitializeComponent();

            asr = new ucAsr() { Dock = DockStyle.Fill };
            tabPage1.Controls.Add(asr);
            mt = new ucMt() { Dock = DockStyle.Fill };
            tabPage2.Controls.Add(mt);
            ucTts tts = new ucTts() { Dock = DockStyle.Fill };
            tabPage3.Controls.Add(tts);


            IAsr _asr = AsrProxy.AsrFun.GetAsr();

            _asr.OnInitialized += _asr_OnInitialized;
            _asr.Initialize();
        }

        private void _asr_OnInitialized(object sender, EventArgs e)
        {
            asr.Load();
            mt.Load();
        }
    }
}
