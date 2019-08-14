using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace ToolUnit
{
    public partial class FormSearchFiles : Form
    {
        string  m_SearchPath;
        string  m_SearchText;
        bool m_bSearchProcessEnd;
        string m_dialogTitle;
        bool m_bQuit;
        Thread m_thread1;
        public FormSearchFiles()
        {
            /*设置线程之间可以非安全的操作控件*/
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();

            m_dialogTitle = this.Text;
            this.m_bQuit = false;
            this.m_thread1 = null;
            this.m_bSearchProcessEnd = false;
        }

        private void button_setPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (textBox_filePath.Text.Length > 0)
            {
                dialog.SelectedPath = textBox_filePath.Text;
            }
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                if (foldPath[foldPath.Length - 1] != '\\') foldPath += "\\";
                textBox_filePath.Text = foldPath;
            }
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            m_SearchPath = textBox_filePath.Text.Trim();
            if (!Directory.Exists(m_SearchPath))
            {
                MessageBox.Show("路径["+m_SearchPath+"]不存在，请检查！");
                m_SearchPath = "";
                return;
            }
            m_SearchText = textBox_searchTxt.Text.Trim();
            m_bSearchProcessEnd = false;
            this.Text = m_dialogTitle + "  [搜索中...]";
            this.progressBar1.Style = ProgressBarStyle.Marquee;

            this.listBox1.Items.Clear();
            
            Thread thread1 = new Thread(this.ThreadProcess);
            thread1.Start();
            m_thread1 = thread1;
        }

        public void ThreadProcess()
        {
            this.textBox_filePath.Enabled = false;
            this.textBox_searchTxt.Enabled = false;
            this.button_setPath.Enabled = false;
            this.button_search.Text = "正在检索";
            this.button_search.Enabled = false;
            SearchDirFiles(m_SearchPath);
            m_bSearchProcessEnd = true;
            this.Text = m_dialogTitle + "  [搜索完毕!]";
            this.progressBar1.Style = ProgressBarStyle.Blocks;
            this.button_search.Text = "开始检索";
            this.button_search.Enabled = true;
            this.button_setPath.Enabled = true;
            this.textBox_filePath.Enabled = true;
            this.textBox_searchTxt.Enabled = true;
        }

        private bool CheckFileNameConform(string FileName)
        {
            bool ret = false;
            RegexOptions opt = RegexOptions.None;
            if( this.checkBox_UL.Checked )
            { //匹配大小写
                opt = RegexOptions.None;
            }
            else
            {//不匹配大小写
                opt = opt | RegexOptions.IgnoreCase;
            }

            Match match = Match.Empty;
            if (this.checkBox_WM.Checked)
            { //匹配通配符
                match = Regex.Match(FileName, m_SearchText, opt);
                if (match.Success) ret = true;
            }
            else
            { //不匹配通配符
                if ( (opt & RegexOptions.IgnoreCase) == RegexOptions.IgnoreCase )
                {
                    if (FileName.ToLower().Contains(m_SearchText.ToLower()))
                    {
                        ret = true;
                    }
                }
                else
                {
                    if (FileName.Contains(m_SearchText))
                    {
                        ret = true;
                    }
                }
            }

            return ret;
        }

        private void SearchDirFiles(string dir)
        {
            if (m_bQuit) return;
            DirectoryInfo di = new DirectoryInfo(dir);
            FileInfo[] fis = di.GetFiles();
            foreach(FileInfo fi in fis)
            {
                if (m_bQuit) return;
                string FileName = fi.Name;
                if (FileName.Contains("."))
                    FileName = FileName.Remove(FileName.LastIndexOf("."));
                if (CheckFileNameConform(FileName))
                {
                    this.listBox1.Items.Add(fi.FullName);
                }
            }

            DirectoryInfo[] dis = di.GetDirectories();
            foreach(DirectoryInfo de in dis)
            {
                if (m_bQuit) return;
                this.SearchDirFiles(de.FullName);
            }

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            //Console.WriteLine(this.listBox1.SelectedItem.ToString());
            string filePath = this.listBox1.SelectedItem.ToString();
            //filePath = filePath.Remove(filePath.LastIndexOf("\\"));
            //Console.WriteLine(filePath);

            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
            psi.Arguments = "/e,/select," + filePath;
            System.Diagnostics.Process.Start(psi);

        }

        private void FormSearchFiles_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.m_bQuit = true;
            if (this.m_thread1 != null)
            {
                if (this.m_thread1.IsAlive)
                    this.m_thread1.Join();
            }
        }
    }
}
