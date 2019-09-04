namespace Test
{
    partial class ucTts
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
            this.btnSave = new System.Windows.Forms.Button();
            this.txtSave = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rtbInfo = new System.Windows.Forms.RichTextBox();
            this.btnTts = new System.Windows.Forms.Button();
            this.btnChooseTxt = new System.Windows.Forms.Button();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Location = new System.Drawing.Point(504, 44);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(43, 24);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "...";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtSave
            // 
            this.txtSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSave.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSave.Location = new System.Drawing.Point(78, 46);
            this.txtSave.Name = "txtSave";
            this.txtSave.Size = new System.Drawing.Size(420, 26);
            this.txtSave.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(0, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 21;
            this.label2.Text = "保存路径：";
            // 
            // rtbInfo
            // 
            this.rtbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbInfo.Location = new System.Drawing.Point(3, 111);
            this.rtbInfo.Name = "rtbInfo";
            this.rtbInfo.Size = new System.Drawing.Size(544, 485);
            this.rtbInfo.TabIndex = 20;
            this.rtbInfo.Text = "";
            // 
            // btnTts
            // 
            this.btnTts.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTts.Location = new System.Drawing.Point(78, 78);
            this.btnTts.Name = "btnTts";
            this.btnTts.Size = new System.Drawing.Size(75, 27);
            this.btnTts.TabIndex = 19;
            this.btnTts.Text = "合成";
            this.btnTts.UseVisualStyleBackColor = true;
            this.btnTts.Click += new System.EventHandler(this.btnTts_Click);
            // 
            // btnChooseTxt
            // 
            this.btnChooseTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChooseTxt.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnChooseTxt.Location = new System.Drawing.Point(504, 5);
            this.btnChooseTxt.Name = "btnChooseTxt";
            this.btnChooseTxt.Size = new System.Drawing.Size(43, 24);
            this.btnChooseTxt.TabIndex = 18;
            this.btnChooseTxt.Text = "...";
            this.btnChooseTxt.UseVisualStyleBackColor = true;
            this.btnChooseTxt.Click += new System.EventHandler(this.btnChooseTxt_Click);
            // 
            // txtFile
            // 
            this.txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFile.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFile.Location = new System.Drawing.Point(78, 7);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(420, 26);
            this.txtFile.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(0, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "文本文件：";
            // 
            // ucTts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rtbInfo);
            this.Controls.Add(this.btnTts);
            this.Controls.Add(this.btnChooseTxt);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.label1);
            this.Name = "ucTts";
            this.Size = new System.Drawing.Size(550, 599);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox rtbInfo;
        private System.Windows.Forms.Button btnTts;
        private System.Windows.Forms.Button btnChooseTxt;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Label label1;
    }
}
