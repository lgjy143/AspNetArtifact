using Newtonsoft.Json.Linq;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Windows.Forms;
using System.Xml;

namespace NetArtifact
{
    public partial class NetArtifact : Form
    {
        /// <summary>
        /// 当前系统类型
        /// </summary>
        public string SystemTypeCurr { get; set; }
        public NetArtifact()
        {
            InitializeComponent();
        }

        private void NetArtifact_Load(object sender, EventArgs e)
        {
            //txtSln.Text = @"E:\workspace\vssWork\Source\EAS_ShoeERP\EAS_ShoeERP.sln";
            //txtPublishProfiles.Text = @"C:\Users\U724\Desktop\Artifact";

            foreach (var item in Enum.GetValues(typeof(SystemType)))
            {
                var sysType = (SystemType)item;
                var description = (DescriptionAttribute)(sysType.GetType().GetField(sysType.ToString())
                                    .GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault());

                cbbSystemType.Items.Add(new ListItem() { Text = description.Description, Value = ((int)item).ToString() });
            }
        }
        private void btnSln_Click(object sender, EventArgs e)
        {
            FileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "解决方案(*.sln)|*.sln";
            fileDialog.RestoreDirectory = true;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                txtSln.Text = fileDialog.FileName;
            }
        }

        private void btnPublishProfiles_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = true;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtPublishProfiles.Text = folderBrowserDialog.SelectedPath;
            }
        }
        private void btnBackUpDir_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = true;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtBackUpDir.Text = folderBrowserDialog.SelectedPath;
            }
        }
        private void btnRelease_Click(object sender, EventArgs e)
        {
            BackgroundDoWorker(new DoWorkEventHandler(this.DoWork), new RunWorkerCompletedEventHandler(this.RunWorkerCompleted), new ProgressChangedEventHandler(this.ProgressChanged));
        }

        private bool VerifyResult(string result)
        {
            var bo = false;
            if (!string.IsNullOrEmpty(result))
            {
                SetEnable(true);
                bo = true;
            }
            return bo;
        }

        private string Publish(Utils.RichTextBoxUtils richText)
        {
            var retVal = string.Empty;
            richText.Write("正在发布解决");
            try
            {
                var sln = txtSln.Text;
                var publishDir = txtPublishProfiles.Text;
                var webProjectOutputDir = Path.Combine(publishDir, "Artifact");
                var outDir = Path.Combine(publishDir, "bin");
                var use32BitMsBuild = false;
                var useDebug = false;

                if (Directory.Exists(publishDir))
                {
                    var dir = new DirectoryInfo(publishDir);
                    dir.Delete(true);
                }

                var script = File.ReadAllText(@"PSScript\PSPulish.ps1");

                using (Runspace runspace = RunspaceFactory.CreateRunspace())
                {
                    runspace.Open();
                    PowerShell ps = PowerShell.Create();
                    ps.Runspace = runspace;
                    ps.AddScript(script);
                    ps.Invoke();
                    ps.AddCommand("InvokeMsBuild").AddParameters(
                        new Dictionary<string, object>()
                        {
                        {"Path", sln},
                        {"WebProjectOutputDir", webProjectOutputDir},
                        {"OutDir", outDir},
                        {"Use32BitMsBuild", use32BitMsBuild},
                        {"UseDebug", useDebug}
                        }
                    );
                    foreach (var result in ps.Invoke())
                    {
                        retVal += result;
                    }
                    richText.Write("发布解决完成" + retVal);
                }
            }
            catch (Exception ex)
            {
                retVal = "执行InvokeMsBuild出错：" + ex.Message;
                richText.Write("发布解决错误:" + retVal);
            }
            return retVal;
        }

        /// <summary>
        /// 删除配置文件
        /// </summary>
        /// <returns></returns>
        private string RemoveConfig(Utils.RichTextBoxUtils richText)
        {
            string retVal = string.Empty;

            var listRemoveDir = new List<string> { "App_Data" };
            var listRemoveConfig = new List<string>() { "log4net.config", "Web.config" };
            var dirConfig = "Config";
            var listConfigDirConfig = new List<string>() { "appSettings.config", "connectionStrings.config" };

            var listRemove = new List<string>();
            listRemove.AddRange(listRemoveDir);
            listRemove.AddRange(listRemoveConfig);
            listRemove.AddRange(listConfigDirConfig);

            richText.Write(string.Format("正在删除配置,{0}", string.Join(",", listRemove)));
            try
            {
                var dirArtifact = Path.Combine(txtPublishProfiles.Text, "Artifact");

                var dirFileInfo = new DirectoryInfo(dirArtifact);
                var dirFiles = dirFileInfo.GetFiles();
                if (dirFiles != null && dirFiles.Count() > 0)
                {
                    foreach (var item in dirFiles)
                    {
                        if (listRemoveConfig.Contains(item.Name))
                            item.Delete();
                    }
                }
                var dirs = dirFileInfo.GetDirectories();
                if (dirs != null && dirs.Count() > 0)
                {
                    foreach (var dir in dirs)
                    {
                        if (listRemoveDir.Contains(dir.Name))
                            dir.Delete(true);

                        if (dir.Name == dirConfig)
                        {
                            var dirConfigFiles = dir.GetFiles();
                            if (dirConfigFiles != null && dirConfigFiles.Count() > 0)
                            {
                                foreach (var item in dirConfigFiles)
                                {
                                    if (listConfigDirConfig.Contains(item.Name))
                                        item.Delete();
                                }
                            }
                        }
                    }
                }

                richText.Write("删除配置完成");
            }
            catch (Exception ex)
            {
                retVal = "删除配置文件失败：" + ex.Message;
                richText.Write("删除配置错误:" + retVal);
            }
            return retVal;
        }
        /// <summary>
        /// 打包发布文件
        /// </summary>
        /// <returns></returns>
        private string Pack(Utils.RichTextBoxUtils richText)
        {
            richText.Write("正在打包发布文件");

            var retVal = string.Empty;
            try
            {
                var dirPublish = Path.Combine(txtPublishProfiles.Text, "Artifact");

                var xmlPath = Path.Combine(dirPublish, "Config", "SystemConfig.config");

                XmlDocument xml = new XmlDocument();
                xml.Load(xmlPath);//读取文件地址

                var softVersion = xml.DocumentElement.GetElementsByTagName("SoftVersion")[0].InnerText;

                if (string.IsNullOrEmpty(softVersion))
                {
                    softVersion = DateTime.Now.ToString("yyyymmddhhmmss");
                }
                softVersion = "V" + softVersion;

                var to = txtPublishProfiles.Text + "\\" + softVersion + ".zip";

                ArchiveFromTo(dirPublish, to);

                richText.Write("打包发布文件完成");
            }
            catch (Exception ex)
            {
                retVal = "打包发布文件出错：" + ex.Message;
                richText.Write("打包发布文件错误:" + retVal);
            }
            return retVal;
        }

        private string ArchiveFromTo(string from, string toFile)
        {
            var retVal = string.Empty;
            try
            {
                //Stopwatch sw = new Stopwatch();
                //sw.Start();

                using (var archive = ZipArchive.Create())
                {
                    archive.AddAllFromDirectory(from);
                    archive.SaveTo(toFile, CompressionType.Deflate);
                }
                //sw.Stop();

                //txtPublishProfiles.Text = "archive.AddAllFromDirectory:" + sw.Elapsed.TotalSeconds + " 秒";

                //sw.Restart();
                //sw.Start();

                //using (var archive = ZipArchive.Create())
                //{
                //    ArchiveMulti(archive, from, toFile);

                //    using (FileStream fs_scratchPath = new FileStream(toFile, FileMode.OpenOrCreate, FileAccess.Write))
                //    {
                //        archive.SaveTo(fs_scratchPath, CompressionType.Deflate);
                //        fs_scratchPath.Close();
                //    }
                //}
                //sw.Stop();
                //txtPublishProfiles.Text += " | archive.AddEntry：" + sw.Elapsed.TotalSeconds + " 秒";
            }
            catch (Exception ex)
            {
                retVal = "压缩文件出错：" + ex.Message;
            }
            return retVal;
        }

        private void ArchiveMulti(ZipArchive archive, string from, string toFile)
        {
            DirectoryInfo di = new DirectoryInfo(from);
            foreach (var fi in di.GetFiles())
            {
                archive.AddEntry(fi.Name, fi.OpenRead(), true);
            }
            var dris = di.GetDirectories();
            foreach (var dir in dris)
            {
                ArchiveMulti(archive, dir.FullName, toFile);
            }
        }

        /// <summary>
        /// 源码备份
        /// </summary>
        /// <returns></returns>
        private string BackupSourceCode(Utils.RichTextBoxUtils richText)
        {
            var retVal = string.Empty;

            try
            {
                var backDir = txtBackUpDir.Text;
                if (string.IsNullOrEmpty(backDir))
                    return retVal;

                richText.Write("正在备份源码");

                var sln = txtSln.Text;

                var dir = Directory.GetParent(sln);

                var from = dir.FullName;

                var fileName = Path.GetFileNameWithoutExtension(sln);

                var to = Path.Combine(txtBackUpDir.Text, fileName + DateTime.Now.ToString("yyyy-MM-dd HH") + ".zip");

                ArchiveFromTo(from, to);

                richText.Write("备份源码完成");
            }
            catch (Exception ex)
            {

                retVal = "源码备份出错:" + ex.Message;

                richText.Write("备份源码错误:" + retVal);
            }

            return retVal;
        }

        #region SetControlEnable

        private void SetControlEnable(Control control, bool v)
        {
            control.BeginInvoke(new Action(() =>
            {
                control.Enabled = v;
            }));
        }

        private void SetEnable(bool v)
        {
            SetControlEnable(btnRelease, v);
            SetControlEnable(btnSln, v);
            SetControlEnable(btnPublishProfiles, v);
            SetControlEnable(btnBackUpDir, v);
            SetControlEnable(txtSln, v);
            SetControlEnable(txtPublishProfiles, v);
            SetControlEnable(txtBackUpDir, v);
            SetControlEnable(cbbSystemType, v);
        }

        #endregion

        public static void BackgroundDoWorker(DoWorkEventHandler doWork, RunWorkerCompletedEventHandler doWorkComplete, ProgressChangedEventHandler progressChanged)
        {
            var bgWorker = new BackgroundWorker();
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.DoWork += doWork;
            bgWorker.RunWorkerCompleted += doWorkComplete;
            bgWorker.ProgressChanged += progressChanged;
            if (!bgWorker.IsBusy)
            {
                bgWorker.RunWorkerAsync();
            }
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            SetEnable(false);

            var retVal = string.Empty;

            var richText = new Utils.RichTextBoxUtils(this.rtbMessage);

            richText.Write("==========发布程序开始==========");

            retVal = Publish(richText);
            if (VerifyResult(retVal))
                return;

            retVal = RemoveConfig(richText);
            if (VerifyResult(retVal))
                return;

            retVal = Pack(richText);
            if (VerifyResult(retVal))
                return;

            retVal = BackupSourceCode(richText);
            if (VerifyResult(retVal))
                return;

            richText.Write("==========发布程序完成==========");
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string msg = "出现异常";
            if (e.UserState != null)
                msg = (string)e.UserState;

            (new Utils.RichTextBoxUtils(this.rtbMessage)).Write(msg);
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetEnable(true);

            if (e.Error != null)
            {

            }
        }

        private void cbbSystemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dirConfig = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "config");
            if (!Directory.Exists(dirConfig))
            {
                Directory.CreateDirectory(dirConfig);
            }

            #region 保存changed前数据
            if (!string.IsNullOrEmpty(SystemTypeCurr))
            {
                var rootDirBeforeChange = Path.Combine(dirConfig, SystemTypeCurr + ".json");
                var jsonConfigBeforeChagne = string.Empty;
                if (File.Exists(rootDirBeforeChange))
                {
                    jsonConfigBeforeChagne = File.ReadAllText(rootDirBeforeChange);
                }
                var jObjBeforeChagne = new JObject();
                if (!string.IsNullOrEmpty(jsonConfigBeforeChagne))
                {
                    jObjBeforeChagne = JObject.Parse(jsonConfigBeforeChagne);
                }
                jObjBeforeChagne["sln"] = txtSln.Text;
                jObjBeforeChagne["sourcebackup"] = txtBackUpDir.Text;
                jObjBeforeChagne["publishdir"] = txtPublishProfiles.Text;
                File.WriteAllText(rootDirBeforeChange, jObjBeforeChagne.ToString());
            }
            #endregion

            #region 展示changed后数据
            var sytemType = cbbSystemType.Text;
            var rootDir = Path.Combine(dirConfig, sytemType + ".json");
            var jsonConfig = string.Empty;
            if (File.Exists(rootDir))
            {
                jsonConfig = File.ReadAllText(rootDir);
            }
            var jObj = new JObject();
            if (!string.IsNullOrEmpty(jsonConfig))
            {
                jObj = JObject.Parse(jsonConfig);
                txtSln.Text = jObj["sln"].ToString();
                txtPublishProfiles.Text = jObj["publishdir"].ToString();
                txtBackUpDir.Text = jObj["sourcebackup"].ToString();
            }
            else
            {

                txtSln.Text =
                txtPublishProfiles.Text =
                txtBackUpDir.Text = "";
            }
            #endregion

            SystemTypeCurr = sytemType;
        }
    }

    /// <summary>
    /// ListItem用于ComboBox控件添加项
    /// </summary>
    public class ListItem
    {
        public string Text
        {
            get;
            set;
        }
        public string Value
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.Text;
        }
    }

    public enum SystemType
    {
        [Description("鞋业ERP")]
        ShoeERP = 1,
        [Description("服装件资")]
        ClothPW = 2,
        [Description("鞋业鞋材")]
        ShoeMat = 3
    }


}
