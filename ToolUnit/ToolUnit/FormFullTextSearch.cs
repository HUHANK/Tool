using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ToolUnit
{
    public partial class FormFullTextSearch : Form
    {
        private CFetchTaskTread m_FetchTaskTread;
        private CFullTextSearchDisplay m_FullTextSearchDisplay;
        private CTaskProcessThread m_TaskProcessThread;
        private string m_Title;
        public FormFullTextSearch()
        {
            /*设置线程之间可以非安全的操作控件*/
            Control.CheckForIllegalCrossThreadCalls = false;
            m_Title = "全文检索";
            InitializeComponent();
        }
        public System.Windows.Forms.RichTextBox RichTextBox1
        {
            get { return this.richTextBox1; }
        }

        private void button_selectFile_Click(object sender, EventArgs e)
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
            if (textBox_filePath.Text.Trim().Length < 1)
            {
                return;
            }
            else
            {
                if (Directory.Exists(textBox_filePath.Text.Trim()) == false)
                {
                    return;
                }
            }

            closeAllTasks();
            CFullTextSearchDisplay.m_InQueue = new System.Collections.Concurrent.ConcurrentQueue<CFileSearchDetail>();
            CFetchTaskTread.m_TaskQueue = new System.Collections.Concurrent.ConcurrentQueue<string>();

            m_FetchTaskTread = new CFetchTaskTread(textBox_filePath.Text.Trim(), textBox_fileType.Text.Trim());
            m_FullTextSearchDisplay = new CFullTextSearchDisplay(this);
            m_TaskProcessThread = new CTaskProcessThread(textBox_searchText.Text.Trim());
            this.Text = m_Title + " [搜索中...]";
            textBox_filePath.Enabled = false;
            textBox_fileType.Enabled = false;
            textBox_searchText.Enabled = false;
            button_search.Enabled = false;
            button_selectFile.Enabled = false;
        }

        public void FullTextSearchDone()
        {
            this.Text = m_Title + " [搜索完毕!]";
            
            string str = String.Format("\n总共找到{0}个符合条件的文件,总共成功匹配{1}个文件，总共匹配{2}处;", 
                CFetchTaskTread.m_MatchedFileNum, CFullTextSearchDisplay.MatchedFileNum, CFullTextSearchDisplay.MatchedLineNum);
            //初始化
            CFullTextSearchDisplay.MatchedFileNum = 0;
            CFullTextSearchDisplay.MatchedLineNum = 0;
            CFetchTaskTread.m_MatchedFileNum = 0;

            this.richTextBox1.AppendText(str + "\n");
            richTextBox1.Select(m_FullTextSearchDisplay.m_textIndex, str.Length);
            richTextBox1.SelectionColor = Color.Black;
            Font font = new Font("宋体", 11);
            richTextBox1.SelectionFont = new Font(font, font.Style | FontStyle.Bold);
            richTextBox1.SelectionLength = 0;

            textBox_filePath.Enabled = true;
            textBox_fileType.Enabled = true;
            textBox_searchText.Enabled = true;
            button_search.Enabled = true;
            button_selectFile.Enabled = true;
            closeAllTasks();

            CFullTextSearchDisplay.m_InQueue = null;
            CFetchTaskTread.m_TaskQueue = null;
        }

        private void button_cancle_Click(object sender, EventArgs e)
        {
            this.Text = m_Title + " [搜索停止!]";
            closeAllTasks();
            textBox_filePath.Enabled = true;
            textBox_fileType.Enabled = true;
            textBox_searchText.Enabled = true;
            button_search.Enabled = true;
            button_selectFile.Enabled = true;
        }

        public void closeAllTasks()
        {
            if (m_FetchTaskTread != null)
                m_FetchTaskTread.Stop();
            if (m_TaskProcessThread != null)
                m_TaskProcessThread.Stop();
            if (m_FullTextSearchDisplay != null )
                m_FullTextSearchDisplay.Stop();
            CFullTextSearchDisplay.m_InQueue = null;
            CFetchTaskTread.m_TaskQueue = null;
        }

        private void FormFullTextSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeAllTasks();
        }
    }   
}
