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

namespace ToolUnit
{
    class FullTextSearchExt
    {
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
        private Dictionary<int, string> m_SearchResults;
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

        public CFileSearchDetail(string fileName)
        {
            m_FileName = fileName;
            m_isMatched = false;
            m_SearchResults = new Dictionary<int, string>();
            m_IsProcessed = false;
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
                    Console.WriteLine("{0} {1}", LineNum, line);
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

        public void StopTask()
        {
            m_bStop = true;
            m_Handle.Join();
        }
        private void run()
        {
            SearchDirFilesRecurve(m_SearchDir);
        }

        private void SearchDirFilesRecurve(string dir)
        {
            if (m_bStop) return;
            DirectoryInfo di = new DirectoryInfo(dir);
            FileInfo[] fis = di.GetFiles();
            foreach(FileInfo fi in fis)
            {
                if (m_bStop) return;
                //文件处理
                string FileName = fi.Name.Trim();
                bool bMatch = false;
                foreach(string ext in m_FileSuffixs)
                {
                    if (FileName.EndsWith(ext))
                    {
                        bMatch = true;
                    }
                }

                if (bMatch)
                {
                    m_TaskQueue.Enqueue(fi.FullName);
                }
            }

            if (m_bStop) return;
            DirectoryInfo[] dis = di.GetDirectories();
            foreach( DirectoryInfo de in dis)
            {
                if (m_bStop) return;
                this.SearchDirFilesRecurve(de.FullName);
            }
        }
    }

    /*任务处理线程*/
    class CTaskProcessThread
    {
        public ConcurrentQueue<string> m_InQueue;

        private long m_SuccessProcessCount;
        private long m_FailedProcessCount;
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
        }

    }
}
