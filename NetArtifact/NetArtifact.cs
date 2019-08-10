using Dapper;
using NetArtifact.Utils;
using Newtonsoft.Json.Linq;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
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

            Initializers_DBExport_Load();
        }

        #region 数据导出配置

        private void btnDC_Click(object sender, EventArgs e)
        {
            CreateSql();
        }

        private void Initializers_DBExport_Load()
        {
            string tablesStr = ConfigurationManager.AppSettings["Tables"].ToString();//获取要生成脚本的表串
            string[] arrTable = tablesStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (arrTable.Length > 0)
            {
                System.Object[] ItemObject = new System.Object[arrTable.Length];
                for (int i = 0; i < arrTable.Length; i++)//循环每个表
                {
                    ItemObject[i] = arrTable[i];
                }
                chklTableName.Items.AddRange(ItemObject);
            }
        }
        private void chkAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chklTableName.Items.Count; i++)
            {
                chklTableName.SetItemChecked(i, chkAll.Checked);
            }
        }

        private void CreateSql()
        {
            List<string> arrTable = new List<string>();
            if (chklTableName.CheckedItems.Count == 0)
            {
                MessageBox.Show("请选择表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            for (int i = 0; i < chklTableName.CheckedItems.Count; i++)
            {
                arrTable.Add(chklTableName.CheckedItems[i].ToString());
            }

            #region 导出数据

            DataSet ds = new DataSet();
            foreach (string tableName in arrTable)
            {
                string sql = string.Format("select * from {0}", tableName);

                using (var conn = DapperUtils.DapperFactory.CreateOracleConnection())
                {
                    var listDataReader = conn.ExecuteReader(sql);
                    if (listDataReader != null)
                    {
                        var dt = new DataTable();
                        dt.Load(listDataReader);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            dt.TableName = tableName;
                            ds.Tables.Add(dt);
                        }
                    }

                }
            }
            if (ds.Tables.Count > 0)
            {
                saveFileDialog1.Filter = "hybak files(.bak)|*.bak";
                saveFileDialog1.RestoreDirectory = true;
                //saveFileDialog1.InitialDirectory = txtPublishProfiles.Text;
                saveFileDialog1.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    byte[] bytes = GetBinaryFormatDataSet(ds);

                    FileStream fs = new FileStream(saveFileDialog1.FileName.ToString(), FileMode.Create);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();

                    MessageBox.Show("导出成功.");
                }
            }
            else
            {
                MessageBox.Show("生成的数据为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            #endregion
        }

        public static byte[] GetBinaryFormatDataSet(DataSet ds)
        {
            //创建内存流
            MemoryStream memStream = new MemoryStream();
            //产生二进制序列化格式
            IFormatter formatter = new BinaryFormatter();
            //指定DataSet串行化格式是二进制
            ds.RemotingFormat = SerializationFormat.Binary;
            //串行化到内存中
            formatter.Serialize(memStream, ds);
            //将DataSet转化成byte[]
            byte[] binaryResult = memStream.ToArray();
            //清空和释放内存流
            memStream.Close();
            memStream.Dispose();
            return binaryResult;
        }

        /// <summary>
        /// DataSet反序列化
        /// </summary>
        /// <param name="binaryData">需要反序列化的byte[]</param>
        /// <returns></returns>
        public static DataSet RetrieveDataSet(byte[] binaryData)
        {
            //创建内存流
            MemoryStream memStream = new MemoryStream(binaryData);
            //产生二进制序列化格式
            IFormatter formatter = new BinaryFormatter();
            //反串行化到内存中
            object obj = formatter.Deserialize(memStream);
            //类型检验
            if (obj is DataSet)
            {
                DataSet dataSetResult = (DataSet)obj;
                return dataSetResult;
            }
            else
            {
                return null;
            }
        }

        #endregion

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

        //修改/Config/SystemConfig.config版本号
        private string UpateVersion(Utils.RichTextBoxUtils richText)
        {
            var retVal = string.Empty;
            richText.Write("正在修改版本号");
            try
            {
                //解决方案路径
                var sln = txtSln.Text;
                //解决方案目录
                var dirSln = Path.GetDirectoryName(sln);
                //hy.Web/Config/SystemConfig.config
                var pathVersion = Path.Combine(dirSln, "hy.Web", "Config", "SystemConfig.config");
                //xml加载
                XmlDocument xml = new XmlDocument();
                xml.Load(pathVersion);
                //获取旧版本信息
                var softVersion = xml.DocumentElement.GetElementsByTagName("SoftVersion")[0].InnerText;
                //当前时间
                var dateTime = DateTime.Now;
                //最新版本号
                var v = $"{softVersion.Split('.')[0]}.{dateTime.ToString("yy.MM.ddHH")}";
                //1.20.08.0511 大版本.年.月.日时
                xml.DocumentElement.GetElementsByTagName("SoftVersion")[0].InnerText = v;
                //保存修改值
                xml.Save(pathVersion);

                richText.Write("修改版本号完成.");
            }
            catch (Exception ex)
            {
                retVal = "执行修改版本号出错：" + ex.Message;
                richText.Write("修改版本号:" + retVal);
            }
            return retVal;
        }

        //发布解决方案
        private string Publish(Utils.RichTextBoxUtils richText)
        {
            var retVal = string.Empty;
            richText.Write("正在发布解决方案");
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
                    richText.Write("发布解决方案完成" + retVal);
                }
            }
            catch (Exception ex)
            {
                retVal = "执行InvokeMsBuild出错：" + ex.Message;
                richText.Write("发布解决方案错误:" + retVal);
            }
            return retVal;
        }

        // 删除配置文件
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

        //打包发布文件
        private string Pack(Utils.RichTextBoxUtils richText, string dateFileStr)
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

                var dateFileDirStr = Path.Combine(txtPublishProfiles.Text, dateFileStr);

                CreateDirNot(dateFileDirStr);

                var to = Path.Combine(dateFileDirStr, softVersion + ".zip");

                retVal = ArchiveFromTo(dirPublish, to);
                if (!string.IsNullOrEmpty(retVal))
                {
                    richText.Write("打包发布文件错误:" + retVal);
                }

                richText.Write("打包发布文件完成");
            }
            catch (Exception ex)
            {
                retVal = "打包发布文件出错：" + ex.Message;
                richText.Write("打包发布文件错误:" + retVal);
            }
            return retVal;
        }

        private void CreateDirNot(string dateFileDirStr)
        {
            if (!Directory.Exists(dateFileDirStr)) { Directory.CreateDirectory(dateFileDirStr); }
        }

        #region 文件压缩
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

        private string ArchiveFromTo(string from, string toFile, string notDirName)
        {
            var retVal = string.Empty;
            try
            {
                //Stopwatch sw = new Stopwatch();
                //sw.Start();

                //using (var archive = ZipArchive.Create())
                //{
                //    archive.AddAllFromDirectory(from);
                //    archive.SaveTo(toFile, CompressionType.Deflate);
                //}
                //sw.Stop();

                //txtPublishProfiles.Text = "archive.AddAllFromDirectory:" + sw.Elapsed.TotalSeconds + " 秒";

                //sw.Restart();
                //sw.Start();

                using (var archive = ZipArchive.Create())
                {
                    ArchiveMulti(archive, from, from, toFile, notDirName);

                    using (FileStream fs_scratchPath = new FileStream(toFile, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        archive.SaveTo(fs_scratchPath, CompressionType.Deflate);
                        fs_scratchPath.Close();
                    }
                }
                //sw.Stop();
                //txtPublishProfiles.Text += " | archive.AddEntry：" + sw.Elapsed.TotalSeconds + " 秒";
            }
            catch (Exception ex)
            {
                retVal = "压缩文件出错：" + ex.Message;
            }
            return retVal;
        }

        private void ArchiveMulti(ZipArchive archive, string root, string from, string toFile, string notDirName)
        {
            DirectoryInfo di = new DirectoryInfo(from);
            var dris = di.GetDirectories();
            foreach (var fi in di.GetFiles())
            {
                //var fileInfo = new FileInfo(from);
                //archive.AddEntry(fi.FullName.Substring(from.Length), fileInfo.OpenRead(), true, fileInfo.Length,
                //                         fileInfo.LastWriteTime);

                archive.AddEntry(fi.FullName.Substring(root.Length), fi.OpenRead(), true, fi.Length, fi.LastWriteTime);

                //archive.AddEntry(fi.Name, fi.OpenRead(), true);
            }
            foreach (var dir in dris)
            {
                if (dir.Name == notDirName)//vs 编译配置文件，不添加
                    continue;

                ArchiveMulti(archive, root, dir.FullName, toFile, notDirName);
            }
        }
        #endregion

        // 源码备份
        private string BackupSourceCode(Utils.RichTextBoxUtils richText)
        {
            var retVal = string.Empty;
            var notDirName = ".vs"; //vs开发工具配置文件，过滤不备份
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

                retVal = ArchiveFromTo(from, to, notDirName);
                //ZipHelper.ZipFileDirectory(from, to);
                //ArchiveFromTo(from, to, notDirName);
                if (!string.IsNullOrEmpty(retVal))
                {
                    richText.Write("源码备份出错: " + retVal);
                }

                richText.Write("备份源码完成");
            }
            catch (Exception ex)
            {

                retVal = "源码备份出错:" + ex.Message;

                richText.Write("备份源码错误:" + retVal);
            }

            return retVal;
        }
        //数据配置导出
        private string DataExport(RichTextBoxUtils richText, string dateFileStr)
        {
            var retVal = string.Empty;
            richText.Write("正在数据配置导出");

            try
            {
                string tablesStr = ConfigurationManager.AppSettings["Tables"].ToString();//获取要生成脚本的表串
                string[] arrTable = tablesStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (arrTable.Length == 0)
                {
                    retVal = "未配置表";
                    richText.Write("数据配置导出:" + retVal);
                    return retVal;
                }

                DataSet ds = new DataSet();
                foreach (string tableName in arrTable)
                {
                    string sql = string.Format("select * from {0}", tableName);

                    using (var conn = DapperUtils.DapperFactory.CreateOracleConnection())
                    {
                        var listDataReader = conn.ExecuteReader(sql);
                        if (listDataReader != null)
                        {
                            var dt = new DataTable();
                            dt.Load(listDataReader);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                dt.TableName = tableName;
                                ds.Tables.Add(dt);
                            }
                        }
                    }
                }
                if (ds.Tables.Count > 0)
                {
                    byte[] bytes = GetBinaryFormatDataSet(ds);

                    var dateFileDirStr = Path.Combine(txtPublishProfiles.Text, dateFileStr);

                    CreateDirNot(dateFileDirStr);

                    FileStream fs = new FileStream(Path.Combine(dateFileDirStr, DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak"), FileMode.Create);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();

                    richText.Write("数据配置导出完成.");
                }
                else
                {
                    richText.Write("生成的数据为空.");
                }
            }
            catch (Exception ex)
            {
                retVal = "数据配置导出错误:" + ex.Message;
                richText.Write("数据配置导出错误:" + retVal);
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
            SetControlEnable(chkDataExport, v);
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

            var dateFileStr = DateTime.Now.ToString("yyyy-MM-dd HH");

            var retVal = string.Empty;

            var richText = new Utils.RichTextBoxUtils(this.rtbMessage);

            richText.Write("==========发布程序开始==========");
            //修改/Config/SystemConfig.config版本号
            retVal = UpateVersion(richText);
            if (VerifyResult(retVal))
                return;

            //发布解决方案
            retVal = Publish(richText);
            if (VerifyResult(retVal))
                return;
            //移除配置
            retVal = RemoveConfig(richText);
            if (VerifyResult(retVal))
                return;
            //打包发布文件
            retVal = Pack(richText, dateFileStr);
            if (VerifyResult(retVal))
                return;
            //先导出数据配置
            if (chkDataExport.Checked)
            {
                retVal = DataExport(richText, dateFileStr);
                if (VerifyResult(retVal))
                    return;
            }
            //后备份源码
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

            if (e.Error != null) { }

            string dir = txtPublishProfiles.Text;
            if (!string.IsNullOrEmpty(dir))
            {
                //存在目录,打开
                if (Directory.Exists(dir)) System.Diagnostics.Process.Start("Explorer.exe", dir);

                //if (MessageBox.Show("是否打开发布目录?", "打开发布目录", MessageBoxButtons.YesNo) == DialogResult.Yes)
                //    System.Diagnostics.Process.Start("Explorer.exe", dir);
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
