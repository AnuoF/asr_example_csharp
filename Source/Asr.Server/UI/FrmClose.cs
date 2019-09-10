using System;
using System.IO;
using System.Windows.Forms;

namespace AsrServer
{
    public partial class FrmClose : Form
    {
        public FrmClose()
        {
            InitializeComponent();

            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
            this.skinEngine1.SkinFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WaveColor2.ssk");
        }

        public bool HideInTaskbar
        {
            get { return cbMin.Checked; }
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
    }
}
