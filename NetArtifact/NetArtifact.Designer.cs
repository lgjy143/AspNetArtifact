namespace NetArtifact
{
    partial class NetArtifact
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.参数 = new System.Windows.Forms.GroupBox();
            this.btnBackUpDir = new System.Windows.Forms.Button();
            this.txtSln = new System.Windows.Forms.TextBox();
            this.btnPublishProfiles = new System.Windows.Forms.Button();
            this.txtPublishProfiles = new System.Windows.Forms.TextBox();
            this.btnSln = new System.Windows.Forms.Button();
            this.txtBackUpDir = new System.Windows.Forms.TextBox();
            this.btnRelease = new System.Windows.Forms.Button();
            this.rtbMessage = new System.Windows.Forms.RichTextBox();
            this.cbbSystemType = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.参数.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // 参数
            // 
            this.参数.Controls.Add(this.btnBackUpDir);
            this.参数.Controls.Add(this.txtSln);
            this.参数.Controls.Add(this.btnPublishProfiles);
            this.参数.Controls.Add(this.txtPublishProfiles);
            this.参数.Controls.Add(this.btnSln);
            this.参数.Controls.Add(this.txtBackUpDir);
            this.参数.Location = new System.Drawing.Point(12, 66);
            this.参数.Name = "参数";
            this.参数.Size = new System.Drawing.Size(390, 114);
            this.参数.TabIndex = 8;
            this.参数.TabStop = false;
            this.参数.Text = "参数";
            // 
            // btnBackUpDir
            // 
            this.btnBackUpDir.Location = new System.Drawing.Point(309, 78);
            this.btnBackUpDir.Name = "btnBackUpDir";
            this.btnBackUpDir.Size = new System.Drawing.Size(75, 23);
            this.btnBackUpDir.TabIndex = 6;
            this.btnBackUpDir.Text = "备份目录";
            this.btnBackUpDir.UseVisualStyleBackColor = true;
            this.btnBackUpDir.Click += new System.EventHandler(this.btnBackUpDir_Click);
            // 
            // txtSln
            // 
            this.txtSln.Location = new System.Drawing.Point(6, 20);
            this.txtSln.Name = "txtSln";
            this.txtSln.Size = new System.Drawing.Size(297, 21);
            this.txtSln.TabIndex = 1;
            // 
            // btnPublishProfiles
            // 
            this.btnPublishProfiles.Location = new System.Drawing.Point(309, 49);
            this.btnPublishProfiles.Name = "btnPublishProfiles";
            this.btnPublishProfiles.Size = new System.Drawing.Size(75, 23);
            this.btnPublishProfiles.TabIndex = 4;
            this.btnPublishProfiles.Text = "发布目录";
            this.btnPublishProfiles.UseVisualStyleBackColor = true;
            this.btnPublishProfiles.Click += new System.EventHandler(this.btnPublishProfiles_Click);
            // 
            // txtPublishProfiles
            // 
            this.txtPublishProfiles.Location = new System.Drawing.Point(6, 49);
            this.txtPublishProfiles.Name = "txtPublishProfiles";
            this.txtPublishProfiles.Size = new System.Drawing.Size(297, 21);
            this.txtPublishProfiles.TabIndex = 3;
            // 
            // btnSln
            // 
            this.btnSln.Location = new System.Drawing.Point(309, 20);
            this.btnSln.Name = "btnSln";
            this.btnSln.Size = new System.Drawing.Size(75, 23);
            this.btnSln.TabIndex = 2;
            this.btnSln.Text = "解决方案";
            this.btnSln.UseVisualStyleBackColor = true;
            this.btnSln.Click += new System.EventHandler(this.btnSln_Click);
            // 
            // txtBackUpDir
            // 
            this.txtBackUpDir.Location = new System.Drawing.Point(6, 78);
            this.txtBackUpDir.Name = "txtBackUpDir";
            this.txtBackUpDir.Size = new System.Drawing.Size(297, 21);
            this.txtBackUpDir.TabIndex = 5;
            // 
            // btnRelease
            // 
            this.btnRelease.Location = new System.Drawing.Point(18, 187);
            this.btnRelease.Name = "btnRelease";
            this.btnRelease.Size = new System.Drawing.Size(75, 23);
            this.btnRelease.TabIndex = 7;
            this.btnRelease.Text = "发布";
            this.btnRelease.UseVisualStyleBackColor = true;
            this.btnRelease.Click += new System.EventHandler(this.btnRelease_Click);
            // 
            // rtbMessage
            // 
            this.rtbMessage.Location = new System.Drawing.Point(18, 220);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.Size = new System.Drawing.Size(384, 202);
            this.rtbMessage.TabIndex = 9;
            this.rtbMessage.Text = "";
            // 
            // cbbSystemType
            // 
            this.cbbSystemType.FormattingEnabled = true;
            this.cbbSystemType.Location = new System.Drawing.Point(6, 20);
            this.cbbSystemType.Name = "cbbSystemType";
            this.cbbSystemType.Size = new System.Drawing.Size(372, 20);
            this.cbbSystemType.TabIndex = 10;
            this.cbbSystemType.SelectedIndexChanged += new System.EventHandler(this.cbbSystemType_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbbSystemType);
            this.groupBox1.Location = new System.Drawing.Point(18, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 56);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "系统";
            // 
            // NetArtifact
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 434);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.rtbMessage);
            this.Controls.Add(this.参数);
            this.Controls.Add(this.btnRelease);
            this.Name = "NetArtifact";
            this.Text = "NetArtifact";
            this.Load += new System.EventHandler(this.NetArtifact_Load);
            this.参数.ResumeLayout(false);
            this.参数.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox 参数;
        private System.Windows.Forms.TextBox txtSln;
        private System.Windows.Forms.Button btnSln;
        private System.Windows.Forms.Button btnPublishProfiles;
        private System.Windows.Forms.TextBox txtPublishProfiles;
        private System.Windows.Forms.Button btnRelease;
        private System.Windows.Forms.Button btnBackUpDir;
        private System.Windows.Forms.TextBox txtBackUpDir;
        private System.Windows.Forms.RichTextBox rtbMessage;
        private System.Windows.Forms.ComboBox cbbSystemType;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

