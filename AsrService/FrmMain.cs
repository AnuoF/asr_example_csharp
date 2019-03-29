using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AsrService
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();

            uc_Main mainCtrl = new uc_Main() { Dock = DockStyle.Fill };
            this.Controls.Add(mainCtrl);
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("是否关闭 AsrService 程序？", "关闭提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
