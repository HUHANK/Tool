using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using IBM.Data.DB2;

namespace ToolUnit
{
    class CCmd
    {
        private Process m_proc;
        private const string m_db2CmdFileName = "db2cmdAJDJKFEM.bat";

        public string cmd(string cmd)
        {
            string ret = "";

            cmd = cmd.Trim().TrimEnd('&') + "&exit";
            m_proc = new Process();
            m_proc.StartInfo.FileName = "cmd.exe";
            m_proc.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
            m_proc.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
            m_proc.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
            m_proc.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
            m_proc.StartInfo.CreateNoWindow = true;          //不显示程序窗口

            try
            {
                bool res = m_proc.Start();//启动程序
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            //向cmd窗口写入命令
            m_proc.StandardInput.WriteLine(cmd);
            m_proc.StandardInput.AutoFlush = true;

            //获取cmd窗口的输出信息
            ret = m_proc.StandardOutput.ReadToEnd();
            m_proc.WaitForExit();//等待程序执行完退出进程
            m_proc.Close();

            return ret;
        }

        public string db2cmd(string cmd)
        {
            string ret = "";
            //把命令写到文件里面方便后面执行
            if (File.Exists(m_db2CmdFileName))
            {
                File.Delete(m_db2CmdFileName);
            }
            StreamWriter sw = File.CreateText(m_db2CmdFileName);

            foreach(string line in cmd.Split('\n'))
            {
                 sw.WriteLine( "" + line );
            }
            sw.WriteLine("exit");
            sw.Flush();
            sw.Close();
            sw.Dispose();

            cmd = " db2cmd -i " + m_db2CmdFileName;

            ret = this.cmd(cmd);

            return ret;
        }
    }
    class SDB2Connection
    {
        public string alias;
        public string database;
        public string node;
        public string note;
        public string protocol;
        public string host;
        public string port;
        public string user;
        public string passwd;
    }
    class CDB2ConnectInfo
    {
        private const string TempDBFileName = "ABCEKFDLOG.OUT";
        private const string TempNodeFileName = "ABCEKNODEFDLOG.OUT";
        public List<SDB2Connection> m_conns;

        public CDB2ConnectInfo( )
        {
            m_conns = new List<SDB2Connection>();
            GetDBDirectorys();
            GetNodeDirectorys();
        }

        private System.Text.Encoding GetFileEncodeType(string filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            Byte[] buffer = br.ReadBytes(2);
            if (buffer.Length > 0 && buffer[0] >= 0xEF)
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

        private void GetDBDirectorys()
        {
            if ( !File.Exists(TempDBFileName))
            {
                string cmdstr = " db2 list db directory > " + TempDBFileName;
                //cmdstr += " exit";
                CCmd cmd = new CCmd();
                cmd.db2cmd(cmdstr);

                //Thread.Sleep(1000);
            }

            int LNUM = 0;
            int TotalEntryNum = 0;
            SDB2Connection db2 = new SDB2Connection();

            StreamReader sRead = new StreamReader(TempDBFileName, GetFileEncodeType(TempDBFileName));
            string line;
            while((line = sRead.ReadLine()) != null)
            {
                if (line.Trim().Length < 1) continue;
                LNUM++;
                if (LNUM == 1) continue;
                if (LNUM == 2)
                {
                    TotalEntryNum = int.Parse(line.Split('=')[1].Trim());
                    continue;
                }
                if ((LNUM-3)%10 == 0)
                {
                    if (3 != LNUM)
                    { 
                        m_conns.Add(db2);
                        db2 = new SDB2Connection();
                    }
                    continue;
                }

                if (line.Contains("数据库别名"))
                {
                    db2.alias = line.Split('=')[1].Trim();
                    continue;
                }
                if (line.Contains("数据库名称"))
                {
                    db2.database = line.Split('=')[1].Trim();
                    continue;
                }
                if (line.Contains("节点名"))
                {
                    db2.node = line.Split('=')[1].Trim();
                    continue;
                }
                if (line.Contains("注释"))
                {
                    db2.note = line.Split('=')[1].Trim();
                    continue;
                }
            }
            m_conns.Add(db2);
            sRead.Close();
            if ((LNUM-3)/10 != TotalEntryNum)
            {
                Console.WriteLine("生成的系统数据库目录文件不完整!");
            }
        }

        private void GetNodeDirectorys()
        {
            if ( !File.Exists(TempNodeFileName) )
            {
                string cmdstr = " db2 list node directory > " + TempNodeFileName;
                //cmdstr += "\n exit"; 
                CCmd cmd = new CCmd();
                cmd.db2cmd(cmdstr);

                //Thread.Sleep(1000);
            }

            int LNUM = 0;
            int TotalEntryNum = 0;
            string snode = "";
            string sprotocol = "";
            string shost = "";
            string sport = "";

            StreamReader sRead = new StreamReader(TempNodeFileName, GetFileEncodeType(TempNodeFileName));
            string line;
            while ((line = sRead.ReadLine()) != null)
            {
                if (line.Trim().Length < 1) continue;
                LNUM++;
                if (1 == LNUM) continue;
                if (2 == LNUM)
                {
                    TotalEntryNum = int.Parse( line.Split('=')[1].Trim() );
                    continue;
                }
                if ((LNUM-3)%7 == 0)
                {
                    if (LNUM != 3)
                    {
                        for(int i=0; i<m_conns.Count(); i++)
                        {
                            if (m_conns[i].node == snode)
                            {
                                m_conns[i].protocol = sprotocol;
                                m_conns[i].host = shost;
                                m_conns[i].port = sport;
                            }
                        }
                    }
                    continue;
                }
                if (line.Contains("节点名"))
                {
                    snode = line.Split('=')[1].Trim();
                }
                if (line.Contains("协议"))
                {
                    sprotocol = line.Split('=')[1].Trim();
                }
                if (line.Contains("主机名"))
                {
                    shost = line.Split('=')[1].Trim();
                }
                if (line.Contains("服务名称"))
                {
                    sport = line.Split('=')[1].Trim();
                }
            }
            for (int i = 0; i < m_conns.Count(); i++)
            {
                if (m_conns[i].node == snode)
                {
                    m_conns[i].protocol = sprotocol;
                    m_conns[i].host = shost;
                    m_conns[i].port = sport;
                }
            }

            sRead.Close();
            if ((LNUM-3)/7 != TotalEntryNum)
            {
                Console.WriteLine("生成的节点目录文件不完整!");
            }
        }
    }

    class CDB2Option
    {
        private DB2Connection m_connect;
        private SDB2Connection m_db2Alias;
        private DB2DataReader m_reader;
        private bool m_isConnected;
        public DB2DataReader Result
        {
            get { return m_reader; }
        }
        private string m_TableSchema;
        public string TableSchema
        {
            set { this.m_TableSchema = value; }
        }

        public CDB2Option(SDB2Connection con)
        {
            m_db2Alias = con;
            m_isConnected = false;
            connect();
        }

        public bool connect()
        {
            if (m_isConnected) return m_isConnected;

            string conStr = String.Format("Database=KSDBS;Server={0}:{1};UID={2};PWD={3}",
                m_db2Alias.host, m_db2Alias.port, m_db2Alias.user, m_db2Alias.passwd);
            
            try
            {
                m_connect = new DB2Connection(conStr);
                m_connect.Open();
                m_isConnected = true;
                return true;
            }
            catch
            {
                m_isConnected = false;
                return false;
            }
        }

        public bool select(string sql)
        {
            try
            {
                //DB2Transaction trans = m_connect.BeginTransaction();

                DB2Command cmd = m_connect.CreateCommand();
                cmd.CommandText = sql;
                //cmd.Transaction = trans;
                m_reader = cmd.ExecuteReader();
                //trans.Commit();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public List<string[]> getAllTables()
        {
            List<string[]> ret = new List<string[]>();

            string sql = @"SELECT TRIM(TABNAME), VALUE(REMARKS,'') FROM syscat.tables WHERE TABSCHEMA = '" + m_TableSchema + @"' AND TYPE = 'T' ORDER BY TABNAME";
            if (!select(sql))
            {
                return ret;
            }
            while (m_reader.Read())
            {
                string tblname = m_reader.GetString(0);
                string note = m_reader.GetString(1);
                string[] sarr = { tblname, note };
                ret.Add(sarr);
            }

            return ret;
        }

        public void test()
        {
            string sql = @"export to B_BUSINESS.ixf of ixf SELECT * FROM KS.B_BUSINESS";
            select(sql);
        }
        ~CDB2Option()
        {
            if (m_connect != null)
            {
                m_connect.Close();
            }
        }

    }
}
