using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Drawing;

namespace ToolUnit
{
    class FullTextSearchExt
    {
    }

    class CFullTextSearchDisplay 
    {
        public static ConcurrentQueue<CFileSearchDetail> m_InQueue = new ConcurrentQueue<CFileSearchDetail>();
        private Thread m_Handle;
        public FormFullTextSearch m_form;
        public static long MatchedLineNum = 0;
        public static long MatchedFileNum = 0;

        public static long TotalRecvFileNum = 0;

        public int m_textIndex;

        public CFullTextSearchDisplay(FormFullTextSearch f)
        {
            m_textIndex = 0;
            MatchedLineNum = 0;
            MatchedFileNum = 0;
            TotalRecvFileNum = 0;
            //m_InQueue = new ConcurrentQueue<CFileSearchDetail>();
            m_form = f;
            m_Handle = null;
            start();  
        }

        public void Stop()
        {
            if (m_Handle != null && m_Handle.IsAlive)
            {
                m_Handle.Abort();
            }
        }

        public void start()
        {
            m_Handle = new Thread(new ThreadStart(this.run));
            m_Handle.Start();
        }

        private void run()
        {
            bool bDequeueSuccesful = false;
            CFileSearchDetail fsd;
            m_form.RichTextBox1.Clear();
            while(true)
            {
                bDequeueSuccesful = m_InQueue.TryDequeue(out fsd);
                if (bDequeueSuccesful == false)
                {
                    continue;
                }
                if (fsd.FileName == "#$%EXIT%$#")
                {
                    m_form.FullTextSearchDone();
                    return;
                }
                TotalRecvFileNum++;
                if (!fsd.IsMatched) continue;
                MatchedFileNum++;

                m_form.RichTextBox1.AppendText(fsd.FileName + "\n");
                m_form.RichTextBox1.Select(m_textIndex, fsd.FileName.Length);
                m_form.RichTextBox1.SelectionColor = System.Drawing.Color.FromArgb(236, 0, 158);
                FontStyle style = FontStyle.Bold;
                Font oldFont = new Font("微软雅黑",11);
                m_form.RichTextBox1.SelectionFont = new Font(oldFont, oldFont.Style );
                m_form.RichTextBox1.SelectionLength = m_textIndex;

                m_textIndex = m_textIndex + fsd.FileName.Length+1;
                foreach (var item in fsd.m_SearchResults)
                {
                    string str = String.Format("\t{0}", item.Key);
                    m_form.RichTextBox1.AppendText(str);
                    m_form.RichTextBox1.Select(m_textIndex, str.Length);
                    m_form.RichTextBox1.SelectionColor = Color.FromArgb(10, 189, 17);
                    Font font = new Font("宋体", 10);
                    m_form.RichTextBox1.SelectionFont = new Font(font, font.Style);
                    m_form.RichTextBox1.SelectionLength = m_textIndex;
                    m_textIndex += str.Length;

                    str = ": ";
                    m_form.RichTextBox1.AppendText(str);
                    m_form.RichTextBox1.Select(m_textIndex, str.Length);
                    m_form.RichTextBox1.SelectionColor = Color.FromArgb(2, 248, 250);
                    font = new Font("宋体", 10);
                    m_form.RichTextBox1.SelectionFont = new Font(font, font.Style|FontStyle.Bold);
                    m_form.RichTextBox1.SelectionLength = m_textIndex;
                    m_textIndex += str.Length;

                    string matchStr;
                    if (fsd.m_MatchResults.TryGetValue(item.Key, out matchStr))
                    {
                        str = item.Value+"\n";
                        int start_pos = str.IndexOf(matchStr);
                        string str1 = str.Substring(0, start_pos - 0);
                        string str2 = str.Substring(start_pos + matchStr.Length, str.Length - start_pos - matchStr.Length);

                        m_form.RichTextBox1.AppendText(str1);
                        m_form.RichTextBox1.Select(m_textIndex, str1.Length);
                        m_form.RichTextBox1.SelectionColor = Color.FromArgb(0, 0, 0);
                        font = new Font("宋体", 10);
                        m_form.RichTextBox1.SelectionFont = new Font(font, font.Style);
                        m_form.RichTextBox1.SelectionLength = m_textIndex;
                        m_textIndex += str1.Length;

                        m_form.RichTextBox1.AppendText(matchStr);
                        m_form.RichTextBox1.Select(m_textIndex, matchStr.Length);
                        m_form.RichTextBox1.SelectionColor = Color.Red;
                        font = new Font("宋体", 10);
                        m_form.RichTextBox1.SelectionFont = new Font(font, font.Style|FontStyle.Bold);
                        m_form.RichTextBox1.SelectionLength = m_textIndex;
                        m_textIndex += matchStr.Length;

                        m_form.RichTextBox1.AppendText(str2);
                        m_form.RichTextBox1.Select(m_textIndex, str2.Length);
                        m_form.RichTextBox1.SelectionColor = Color.FromArgb(0, 0, 0);
                        font = new Font("宋体", 10);
                        m_form.RichTextBox1.SelectionFont = new Font(font, font.Style);
                        m_form.RichTextBox1.SelectionLength = m_textIndex;
                        m_textIndex += str2.Length;
                    }
                    else
                    {
                        str = item.Value + "\n";
                        m_form.RichTextBox1.AppendText(str);
                        m_form.RichTextBox1.Select(m_textIndex, str.Length);
                        m_form.RichTextBox1.SelectionColor = Color.FromArgb(0, 0, 0);
                        font = new Font("宋体", 10);
                        m_form.RichTextBox1.SelectionFont = new Font(font, font.Style);
                        m_form.RichTextBox1.SelectionLength = 0;
                        m_textIndex += str.Length;
                    }

                    MatchedLineNum++;
                }
            }
            
        }
    }

    class CFileSearchDetail
    {
        /*文件名*/
        private string m_FileName;
        public string FileName
        {
            get { return m_FileName; }
            set { m_FileName = value; }
        }
        /*行号+行的内容*/
        public Dictionary<int, string> m_MatchResults;
        public Dictionary<int, string> m_SearchResults;
        /*正则匹配的内容*/
        private string m_Pattern; 
        public string Pattern
        {
            get { return m_Pattern; }
            set { m_Pattern = value; }
        }
        /*是否匹配到内容*/
        private bool m_isMatched;
        public bool IsMatched
        {
            get { return m_isMatched; }
        }

        private bool m_IsProcessed;
        public bool IsProcessed
        {
            get { return m_IsProcessed; }
        }

        public ConcurrentQueue<CFileSearchDetail> m_OutQueue;

        public CFileSearchDetail(string fileName)
        {
            m_FileName = fileName;
            m_isMatched = false;
            m_SearchResults = new Dictionary<int, string>();
            m_MatchResults = new Dictionary<int, string>();
            m_IsProcessed = false;
            m_OutQueue = CFullTextSearchDisplay.m_InQueue;
        }

        private System.Text.Encoding GetFileEncodeType(string filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            Byte[] buffer = br.ReadBytes(2);
            if (buffer.Length>0 && buffer[0] >= 0xEF)
            {
                if (buffer[0] == 0xEF && buffer[1] == 0xBB)
                {
                    return System.Text.Encoding.UTF8;
                }
                else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                {
                    return System.Text.Encoding.BigEndianUnicode;
                }
                else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                {
                    return System.Text.Encoding.Unicode;
                }
                else
                {
                    return System.Text.Encoding.Default;
                }
            }
            else
            {
                return System.Text.Encoding.Default;
            }
        }

        public async void SearchInFile()
        {
            m_isMatched = false;
            StreamReader sRead = new StreamReader(m_FileName, GetFileEncodeType(m_FileName));
            string line;
            int LineNum = 0;
            while( (line = sRead.ReadLine( )) != null)
            {
                LineNum++;
                if (line.Trim().Length < 1) continue;

                Match match = Regex.Match(line, m_Pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {//匹配成功
                    m_isMatched = true;
                    m_SearchResults.Add(LineNum, line);
                    m_MatchResults.Add(LineNum, match.Value);
                }

            }
            m_IsProcessed = true;
        }

    }

    /*遍历文件夹，寻找出符合的文件*/
    class CFetchTaskTread
    {
        private string m_SearchDir;
        private ArrayList m_FileSuffixs;
        private Thread m_Handle;
        private bool m_bStop;
        public static int m_MatchedFileNum = 0;

        public static ConcurrentQueue<string> m_TaskQueue = new ConcurrentQueue<string>();

        public CFetchTaskTread(string dir, string suffixs)
        {
            //m_TaskQueue = new ConcurrentQueue<string>();
            {
                m_SearchDir = dir;
                if (dir.Length < 1) return;
                if (!Directory.Exists(dir)) return;
            }

            m_FileSuffixs = new ArrayList();
            {
                suffixs = suffixs.Replace('*',' ');

                string[] res = null;
                if (suffixs.Contains(","))
                {
                    res = suffixs.Split(',');
                }
                if (suffixs.Contains(";"))
                {
                    res = suffixs.Split(';');
                }

                if (res != null)
                    foreach(string item in res)
                    {
                        if (item.Trim().Length < 1) continue;
                        m_FileSuffixs.Add(item.Trim());
                    }
            }
            
            m_bStop = false;

            m_Handle = new Thread(new ThreadStart(this.run));
            m_Handle.Start();
        }

        public void Stop()
        {
            m_bStop = true;
            m_Handle.Join();
            if (m_Handle != null && m_Handle.IsAlive)
            {
                m_Handle.Abort();
            }
        }
        private void run()
        {
            SearchDirFilesRecurve(m_SearchDir);
            m_TaskQueue.Enqueue("#$%EXIT%$#");
        }

        private void SearchDirFilesRecurve(string dir)
        {
            if (m_bStop) return;
            DirectoryInfo di = new DirectoryInfo(dir);
            FileInfo[] fis = null;
            try
            {
                fis = di.GetFiles();
            }catch
            {
                //do nothing
            }

            if (fis != null)
            {
                foreach (FileInfo fi in fis)
                {
                    if (m_bStop) return;
                    //文件处理
                    string FileName = fi.Name.Trim();
                    bool bMatch = false;
                    foreach (string ext in m_FileSuffixs)
                    {
                        if (FileName.EndsWith(ext))
                        {
                            bMatch = true;
                        }
                    }

                    if (bMatch)
                    {
                        m_MatchedFileNum++;
                        m_TaskQueue.Enqueue(fi.FullName);
                    }
                }
            }
            

            if (m_bStop) return;
            DirectoryInfo[] dis = null;
            try
            {
                dis = di.GetDirectories();
            }
            catch
            {
                //do nothing
            }

            if (dis != null)
            {
                foreach (DirectoryInfo de in dis)
                {
                    if (m_bStop) return;
                    this.SearchDirFilesRecurve(de.FullName);
                }
            }  
        }
    }

    /*任务处理线程*/
    class CTaskProcessThread
    {
        public ConcurrentQueue<string> m_InQueue;

        public  int m_SuccessProcessCount;
        public  int m_FailedProcessCount;
        private Thread m_Handle;
        private string m_Pattern;

        public CTaskProcessThread( string pattern )
        {
            m_SuccessProcessCount = 0;
            m_FailedProcessCount = 0;
            m_InQueue = CFetchTaskTread.m_TaskQueue;
            m_Pattern = pattern;

            m_Handle = new Thread(new ThreadStart(this.run));
            m_Handle.Start();
        }

        public void Stop()
        {
            if (m_Handle != null && m_Handle.IsAlive)
            {
                m_Handle.Abort();
            }
        }

        public void run()
        {
            string filePath;
            bool bDequeueSuccesful = false;

            while (true)
            {
                bDequeueSuccesful = m_InQueue.TryDequeue(out filePath);
                if (!bDequeueSuccesful)
                {
                    m_FailedProcessCount++;
                    continue;
                }
                //如果是退出标志就退出
                if (filePath == "#$%EXIT%$#")
                {
                    while(m_SuccessProcessCount != CFullTextSearchDisplay.TotalRecvFileNum)
                    {
                        Thread.Sleep(50);
                    }
                    CFileSearchDetail fsd1 = new CFileSearchDetail(filePath);
                    fsd1.m_OutQueue.Enqueue(fsd1);
                    break;
                }
                m_SuccessProcessCount++;

                CFileSearchDetail fsd = new CFileSearchDetail(filePath);
                fsd.Pattern = m_Pattern;

                ThreadPool.QueueUserWorkItem(new WaitCallback(process), fsd);
            }
        }
        private static void process(object obj)
        {
            CFileSearchDetail fsd = obj as CFileSearchDetail;
            fsd.SearchInFile();
            fsd.m_OutQueue.Enqueue(fsd);
        }

    }
}
