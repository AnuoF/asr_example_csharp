namespace Test
{
    partial class ucMt
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnTranslate = new System.Windows.Forms.Button();
            this.cmbTo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbFrom = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtSrc = new System.Windows.Forms.TextBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rtbInfo = new System.Windows.Forms.RichTextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnTranslate);
            this.panel1.Controls.Add(this.cmbTo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cmbFrom);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(519, 73);
            this.panel1.TabIndex = 0;
            // 
            // btnTranslate
            // 
            this.btnTranslate.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTranslate.Location = new System.Drawing.Point(368, 25);
            this.btnTranslate.Name = "btnTranslate";
            this.btnTranslate.Size = new System.Drawing.Size(93, 23);
            this.btnTranslate.TabIndex = 4;
            this.btnTranslate.Text = "Translate";
            this.btnTranslate.UseVisualStyleBackColor = true;
            this.btnTranslate.Click += new System.EventHandler(this.btnTranslate_Click);
            // 
            // cmbTo
            // 
            this.cmbTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTo.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbTo.FormattingEnabled = true;
            this.cmbTo.Items.AddRange(new object[] {
            "汉语"});
            this.cmbTo.Location = new System.Drawing.Point(226, 27);
            this.cmbTo.Name = "cmbTo";
            this.cmbTo.Size = new System.Drawing.Size(121, 24);
            this.cmbTo.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(202, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "To";
            // 
            // cmbFrom
            // 
            this.cmbFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFrom.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbFrom.FormattingEnabled = true;
            this.cmbFrom.Location = new System.Drawing.Point(51, 28);
            this.cmbFrom.Name = "cmbFrom";
            this.cmbFrom.Size = new System.Drawing.Size(121, 24);
            this.cmbFrom.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(4, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "From";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtSrc);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtResult);
            this.splitContainer1.Size = new System.Drawing.Size(519, 366);
            this.splitContainer1.SplitterDistance = 268;
            this.splitContainer1.TabIndex = 1;
            // 
            // txtSrc
            // 
            this.txtSrc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSrc.Location = new System.Drawing.Point(0, 0);
            this.txtSrc.Multiline = true;
            this.txtSrc.Name = "txtSrc";
            this.txtSrc.Size = new System.Drawing.Size(268, 366);
            this.txtSrc.TabIndex = 0;
            // 
            // txtResult
            // 
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Location = new System.Drawing.Point(0, 0);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(247, 366);
            this.txtResult.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rtbInfo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 439);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(519, 100);
            this.panel2.TabIndex = 2;
            // 
            // rtbInfo
            // 
            this.rtbInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbInfo.Location = new System.Drawing.Point(0, 0);
            this.rtbInfo.Name = "rtbInfo";
            this.rtbInfo.Size = new System.Drawing.Size(519, 100);
            this.rtbInfo.TabIndex = 0;
            this.rtbInfo.Text = "";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.splitContainer1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 73);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(519, 366);
            this.panel3.TabIndex = 3;
            // 
            // ucMt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ucMt";
            this.Size = new System.Drawing.Size(519, 539);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnTranslate;
        private System.Windows.Forms.ComboBox cmbTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbFrom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtSrc;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox rtbInfo;
        private System.Windows.Forms.Panel panel3;
    }
}
