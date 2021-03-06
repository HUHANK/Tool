﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using IBM.Data.DB2;
using System.Runtime.Serialization.Formatters.Binary;

namespace ToolUnit
{
    /*******************结构体类定义START*****************************/
    [Serializable]
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

        public SDB2Connection()
        {
            alias = "";
            database = "";
            node = "";
            note = "";
            protocol = "";
            host = "";
            port = "";
            user = "";
            passwd = "";
        }
    }

    [Serializable]
    class SDBTable
    {
        public string name;
        public string schema;
        public string import_method;
        public string delete_method;

        public SDBTable()
        {
            name = "";
            schema = "";
            import_method = "replace";
            delete_method = "";
        }
    }

    [Serializable]
    class SSolutionConfig
    {
        public string name;
        public string source_alias;
        public string dest_alias;
        public List<SDBTable> tables;

        public SSolutionConfig()
        {
            name = "";
            source_alias = "";
            dest_alias = "";
            tables = new List<SDBTable>();
        }
    }
    /*******************结构体类定义END*****************************/
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

        public string db2cmd(string cmd, bool bsync=true)
        {
            string ret = "";
            
            if (bsync)
            {
                cmd = " db2cmd -i " + cmd;
            }
            else
            {
                cmd = " db2cmd  " + cmd;
            }
            

            ret = this.cmd(cmd);

            return ret;
        }
    }
    
    class CDB2ConnectInfo
    {
        private const string TempDBFileName = "./cache/DB.CFG";
        private const string TempNodeFileName = "./cache/NODE.CFG";
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
                string cmdstr = " \"db2 list db directory > " + TempDBFileName + " && exit \"";
                CCmd cmd = new CCmd();
                cmd.db2cmd(cmdstr);
            }

            int LNUM = 0;
            int TotalEntryNum = 0;
            SDB2Connection db2 = new SDB2Connection();

            StreamReader sRead = new StreamReader(TempDBFileName, GetFileEncodeType(TempDBFileName));
            string line;

            sRead.ReadLine();
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
            //if ((LNUM-3)/10 != TotalEntryNum)
            //{
            //    Console.WriteLine("生成的系统数据库目录文件不完整!");
            //}
        }

        private void GetNodeDirectorys()
        {
            if ( !File.Exists(TempNodeFileName) )
            {
                string cmdstr = " \"db2 list node directory > " + TempNodeFileName + " && exit\"";
                CCmd cmd = new CCmd();
                cmd.db2cmd(cmdstr);
            }

            int LNUM = 0;
            int TotalEntryNum = 0;
            string snode = "";
            string sprotocol = "";
            string shost = "";
            string sport = "";

            StreamReader sRead = new StreamReader(TempNodeFileName, GetFileEncodeType(TempNodeFileName));
            string line;
            sRead.ReadLine();
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
            //if ((LNUM-3)/7 != TotalEntryNum)
            //{
            //    Console.WriteLine("生成的节点目录文件不完整!");
            //}
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

        public bool testConnect(out string msg)
        {
            string conStr = String.Format("Database=KSDBS;Server={0}:{1};UID={2};PWD={3}",
                m_db2Alias.host, m_db2Alias.port, m_db2Alias.user, m_db2Alias.passwd);
            try
            {
                DB2Connection conn = new DB2Connection(conStr);
                conn.Open();
                conn.Close();

                msg = "";
                return true;
            }
            catch (Exception e)
            {
                msg = e.Message;
                return false;
            }
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

        public bool update(string sql)
        {
            try
            {
                DB2Transaction trans = m_connect.BeginTransaction();
                DB2Command cmd = m_connect.CreateCommand();
                cmd.CommandText = sql;
                cmd.Transaction = trans;
                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool truncate(string tableName)
        {
            string sql = String.Format("TRUNCATE TABLE {0}.{1} IMMEDIATE", m_TableSchema, tableName);
            return update(sql);
        }
        public bool delete(string tableName, int count = 10000)
        {
            bool ret = true;
            int COUNT = 0;
            string sql = String.Format("SELECT COUNT(1) FROM {0}.{1}", m_TableSchema, tableName);
            if (select(sql))
            {
                m_reader.Read();

                int c = m_reader.GetInt32(0);
                COUNT = c / count + 1;

                m_reader.Close();
            }
            else
            {
                return false;
            }

            sql = String.Format("DELETE (SELECT * FROM {0}.{1} FETCH FIRST {2} ROWS ONLY)", m_TableSchema, tableName, count);

            for(int n = 0; n<COUNT; n++)
            {
                update(sql);
            }

            return ret;
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

        public List<string> getAllSchemas()
        {
            List<string> ret = new List<string>();

            string sql = @"SELECT TABSCHEMA FROM syscat.tables WHERE TYPE = 'T' GROUP BY TABSCHEMA";
            if (!select(sql))
            {
                return ret;
            }
            while (m_reader.Read())
            {
                string schema = m_reader.GetString(0);
                ret.Add(schema.Trim());
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

    class CGenDB2ExpImpBat
    {
        public SDB2Connection m_sDB2Info;
        public SDB2Connection m_dDB2Info;
        public List<SDBTable> m_tables;
        public string m_TableSchema;
        public string m_FileName;
        public string m_ExportDataPath;

        public CGenDB2ExpImpBat()
        {
            m_tables = new List<SDBTable>();
            m_ExportDataPath = "./export_data/";
        }

        public void GenFile()
        {
            CTool.CheckPathExistOrCreate(m_ExportDataPath);

            StreamWriter sWriter = new StreamWriter(m_FileName, false, Encoding.Default);
            string line = "";

            sWriter.WriteLine("@echo off");

            line = String.Format("db2 connect to {0} user {1} using {2}  \r\n call :PrintErrMsg %errorlevel%  连接{3}数据库  ", m_sDB2Info.alias, m_sDB2Info.user, m_sDB2Info.passwd, m_sDB2Info.alias) ;
            sWriter.WriteLine(line);

            foreach(SDBTable tbl in m_tables)
            {
                line = String.Format("db2 \"export to {0}/{1}.ixf of ixf SELECT * FROM {2}.{3} WITH UR \"  ", m_ExportDataPath, tbl.name, tbl.schema, tbl.name);
                sWriter.WriteLine(line);
                line = String.Format("call :PrintErrMsg %errorlevel%  导出表{0}.{1}  \r\n", tbl.schema, tbl.name);
                sWriter.WriteLine(line);
            }

            sWriter.WriteLine("db2 connect reset\r\n");

            line = String.Format("db2 connect to {0} user {1} using {2}   \r\n call :PrintErrMsg %errorlevel%  连接{3}数据库  ", m_dDB2Info.alias, m_dDB2Info.user, m_dDB2Info.passwd, m_dDB2Info.alias);
            sWriter.WriteLine(line);

            foreach (SDBTable tbl in m_tables)
            {
                if (tbl.import_method == "replace")
                {
                    line = String.Format("db2 \"import from {0}/{1}.ixf of ixf modified by compound=100 commitcount 10000 replace into {2}.{3} \"  ", m_ExportDataPath, tbl.name, tbl.schema, tbl.name);
                }
                else if (tbl.import_method == "insert")
                {
                    line = String.Format("db2 \"import from {0}/{1}.ixf of ixf  commitcount 10000 insert into {2}.{3} \"  ", m_ExportDataPath, tbl.name, tbl.schema, tbl.name);
                }
                else if (tbl.import_method == "insert_update")
                {
                    line = String.Format("db2 \"import from {0}/{1}.ixf of ixf  commitcount 10000 insert_update into {2}.{3} \"  ", m_ExportDataPath, tbl.name, tbl.schema, tbl.name);
                }
                sWriter.WriteLine(line);
                line = String.Format("call :PrintErrMsg %errorlevel%  导入表{0}.{1}  \r\n", tbl.schema, tbl.name);
                sWriter.WriteLine(line);
            }
            sWriter.WriteLine("db2 connect reset\n");

            line = "echo 数据库同步成功完成！ \r\n pause \r\n exit 0 \r\n:PrintErrMsg\r\n    if %1 equ 0 (goto SUCC) else (goto FAILD)\r\n\r\n    :FAILD\r\n        echo %2失败[%1]\r\n \r\n echo 数据库同步失败\r\n       pause\r\nexit %1\r\n    :SUCC\r\n        echo %2成功[%1]\r\nGOTO:EOF            ";
            sWriter.WriteLine(line);

            sWriter.Flush();
            sWriter.Close();
        }

    }

    class CSerialize
    {
        public string FileName;
        public FileStream FS;

        public void Serialize(Object o)
        {
            FS = new FileStream(FileName, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(FS, o);
            FS.Flush();
            FS.Close();
        }

        public object DeSerialize()
        {
            object ret = null;
            FS = new FileStream(FileName, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            ret = bf.Deserialize(FS);
            FS.Close();   
            return ret;
        }
    }

    class CTool
    {
        public static void CheckPathExistOrCreate(string filepath)
        {
            string DirName = Path.GetDirectoryName(filepath);
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
        }

        public static void DeleteDirAllFiles(string srcPath)
        {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();
            foreach(FileSystemInfo fi in fileinfo)
            {
                if (fi is DirectoryInfo)
                {
                    DirectoryInfo subdir = new DirectoryInfo(fi.FullName);
                    subdir.Delete(true);
                }
                else
                {
                    File.Delete(fi.FullName);
                }
            }
        }
    }
}
