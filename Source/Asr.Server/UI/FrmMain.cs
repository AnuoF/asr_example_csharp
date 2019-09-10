using System;
using System.IO;
using System.Windows.Forms;

namespace AsrServer
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();

            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
            this.skinEngine1.SkinFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WaveColor2.ssk");

            ucMain mainCtrl = new ucMain() { Dock = DockStyle.Fill };
            this.Controls.Add(mainCtrl);
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (FrmClose f = new FrmClose())
            {
                if (f.ShowDialog() == DialogResult.No)
                {
                    e.Cancel = true;
                    if (f.HideInTaskbar)
                    {
                        this.Hide();
                    }
                }
            }
        }

        #region 托盘

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.ShowInTaskbar = true;
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.ShowInTaskbar = true;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.ShowInTaskbar = false;
            }
        }

        #endregion

    }
}