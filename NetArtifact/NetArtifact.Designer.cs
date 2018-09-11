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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDC = new System.Windows.Forms.Button();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.chklTableName = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.参数.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.参数.Location = new System.Drawing.Point(25, 84);
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
            this.btnRelease.Location = new System.Drawing.Point(31, 205);
            this.btnRelease.Name = "btnRelease";
            this.btnRelease.Size = new System.Drawing.Size(75, 23);
            this.btnRelease.TabIndex = 7;
            this.btnRelease.Text = "发布";
            this.btnRelease.UseVisualStyleBackColor = true;
            this.btnRelease.Click += new System.EventHandler(this.btnRelease_Click);
            // 
            // rtbMessage
            // 
            this.rtbMessage.Location = new System.Drawing.Point(31, 238);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.Size = new System.Drawing.Size(384, 239);
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
            this.groupBox1.Location = new System.Drawing.Point(31, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 56);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "系统";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(2, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(452, 510);
            this.tabControl1.TabIndex = 12;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.btnRelease);
            this.tabPage1.Controls.Add(this.rtbMessage);
            this.tabPage1.Controls.Add(this.参数);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(444, 484);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "发布配置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(444, 484);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "数据导出配置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDC);
            this.groupBox2.Controls.Add(this.chkAll);
            this.groupBox2.Controls.Add(this.chklTableName);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.splitter1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox2.Size = new System.Drawing.Size(438, 478);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " 系统初始化脚本 ";
            // 
            // btnDC
            // 
            this.btnDC.Location = new System.Drawing.Point(14, 428);
            this.btnDC.Name = "btnDC";
            this.btnDC.Size = new System.Drawing.Size(166, 23);
            this.btnDC.TabIndex = 15;
            this.btnDC.Text = "导出升级包(不支持菜单）";
            this.btnDC.UseVisualStyleBackColor = true;
            this.btnDC.Click += new System.EventHandler(this.btnDC_Click);
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(13, 30);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(48, 16);
            this.chkAll.TabIndex = 11;
            this.chkAll.Text = "全选";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.Click += new System.EventHandler(this.chkAll_Click);
            // 
            // chklTableName
            // 
            this.chklTableName.FormattingEnabled = true;
            this.chklTableName.Location = new System.Drawing.Point(8, 52);
            this.chklTableName.Name = "chklTableName";
            this.chklTableName.Size = new System.Drawing.Size(406, 356);
            this.chklTableName.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(67, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "要生成脚本的表如下：";
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(5, 19);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(425, 454);
            this.splitter1.TabIndex = 9;
            this.splitter1.TabStop = false;
            // 
            // NetArtifact
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 514);
            this.Controls.Add(this.tabControl1);
            this.Name = "NetArtifact";
            this.Text = "NetArtifact";
            this.Load += new System.EventHandler(this.NetArtifact_Load);
            this.参数.ResumeLayout(false);
            this.参数.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnDC;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.CheckedListBox chklTableName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

