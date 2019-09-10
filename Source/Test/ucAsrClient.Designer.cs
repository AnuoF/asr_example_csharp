namespace Test
{
    partial class ucAsrClient
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.rtbInfo = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnText2LanguageType = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtAudioFile = new System.Windows.Forms.TextBox();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnRecog = new System.Windows.Forms.Button();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.cbPCM = new System.Windows.Forms.RadioButton();
            this.cbTK = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(921, 601);
            this.panel1.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.rtbInfo);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 119);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(921, 482);
            this.panel4.TabIndex = 15;
            // 
            // rtbInfo
            // 
            this.rtbInfo.BackColor = System.Drawing.SystemColors.Control;
            this.rtbInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbInfo.ForeColor = System.Drawing.Color.Black;
            this.rtbInfo.Location = new System.Drawing.Point(0, 0);
            this.rtbInfo.Name = "rtbInfo";
            this.rtbInfo.Size = new System.Drawing.Size(921, 482);
            this.rtbInfo.TabIndex = 8;
            this.rtbInfo.Text = "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.btnText2LanguageType);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnSelect);
            this.panel2.Controls.Add(this.txtAudioFile);
            this.panel2.Controls.Add(this.btnPlay);
            this.panel2.Controls.Add(this.btnRecog);
            this.panel2.Controls.Add(this.cmbLanguage);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(921, 119);
            this.panel2.TabIndex = 13;
            // 
            // btnText2LanguageType
            // 
            this.btnText2LanguageType.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnText2LanguageType.Location = new System.Drawing.Point(423, 57);
            this.btnText2LanguageType.Name = "btnText2LanguageType";
            this.btnText2LanguageType.Size = new System.Drawing.Size(124, 20);
            this.btnText2LanguageType.TabIndex = 13;
            this.btnText2LanguageType.Text = "To Language";
            this.btnText2LanguageType.UseVisualStyleBackColor = true;
            this.btnText2LanguageType.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(0, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "音频文件";
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelect.Location = new System.Drawing.Point(818, 4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(42, 23);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "...";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtAudioFile
            // 
            this.txtAudioFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAudioFile.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtAudioFile.Location = new System.Drawing.Point(71, 5);
            this.txtAudioFile.Name = "txtAudioFile";
            this.txtAudioFile.Size = new System.Drawing.Size(741, 26);
            this.txtAudioFile.TabIndex = 1;
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlay.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPlay.Location = new System.Drawing.Point(866, 4);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(57, 23);
            this.btnPlay.TabIndex = 5;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnRecog
            // 
            this.btnRecog.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRecog.Location = new System.Drawing.Point(258, 94);
            this.btnRecog.Name = "btnRecog";
            this.btnRecog.Size = new System.Drawing.Size(121, 20);
            this.btnRecog.TabIndex = 6;
            this.btnRecog.Text = "识别";
            this.btnRecog.UseVisualStyleBackColor = true;
            this.btnRecog.Click += new System.EventHandler(this.btnRecog_Click);
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguage.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.Location = new System.Drawing.Point(258, 57);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(121, 24);
            this.cmbLanguage.TabIndex = 7;
            // 
            // cbPCM
            // 
            this.cbPCM.AutoSize = true;
            this.cbPCM.Checked = true;
            this.cbPCM.Location = new System.Drawing.Point(18, 20);
            this.cbPCM.Name = "cbPCM";
            this.cbPCM.Size = new System.Drawing.Size(191, 16);
            this.cbPCM.TabIndex = 14;
            this.cbPCM.TabStop = true;
            this.cbPCM.Text = "标准音频（pcm16位/16k/单音）";
            this.cbPCM.UseVisualStyleBackColor = true;
            // 
            // cbTK
            // 
            this.cbTK.AutoSize = true;
            this.cbTK.Location = new System.Drawing.Point(18, 42);
            this.cbTK.Name = "cbTK";
            this.cbTK.Size = new System.Drawing.Size(143, 16);
            this.cbTK.TabIndex = 15;
            this.cbTK.Text = "Tracker800保存的音频";
            this.cbTK.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbPCM);
            this.groupBox1.Controls.Add(this.cbTK);
            this.groupBox1.Location = new System.Drawing.Point(13, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(207, 69);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "音频类型";
            // 
            // ucAsr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "ucAsr";
            this.Size = new System.Drawing.Size(921, 601);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox rtbInfo;
        private System.Windows.Forms.ComboBox cmbLanguage;
        private System.Windows.Forms.Button btnRecog;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAudioFile;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnText2LanguageType;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton cbPCM;
        private System.Windows.Forms.RadioButton cbTK;
    }
}
