using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ucAsr asr = new ucAsr() { Dock = DockStyle.Fill };
            tabPage1.Controls.Add(asr);
            ucMt mt = new ucMt() { Dock = DockStyle.Fill };
            tabPage2.Controls.Add(mt);
            ucTts tts = new ucTts() { Dock = DockStyle.Fill };
            tabPage3.Controls.Add(tts);
        }
    }
}
