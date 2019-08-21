using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace ToolUnit
{
    class CCmd
    {
        private Process m_proc;
        private const string m_db2CmdFileName = "db2cmd.bat";

        public string cmd(string cmd)
        {
            string ret = "";

            cmd = cmd.Trim().TrimEnd('&') + "&exit";
            m_proc = new Process();
            m_proc.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
            m_proc.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
            m_proc.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
            m_proc.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
            m_proc.StartInfo.CreateNoWindow = true;          //不显示程序窗口

            m_proc.Start();//启动程序

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
                 sw.WriteLine( "db2cmd " + line );
            }
            sw.Flush();
            sw.Close();
            sw.Dispose();

            cmd = " db2cmd -i " + m_db2CmdFileName;

            ret = this.cmd(cmd);

            return ret;
        }
    }
    struct SDB2Connection
    {
        public string alias;
        public string database;
        public string node;
        public string note;
    }
    class CDB2ConnectInfo
    {
        private const string TempFileName = "ABCEKFDLOG.OUT";
        private List<SDB2Connection> m_conns;

        public CDB2ConnectInfo()
        {
            m_conns = new List<SDB2Connection>();
        }

        private void GetDBDirectorys()
        {
            string cmdstr = " db2 list db directory > " + TempFileName;
            cmdstr += "\n exit";
            CCmd cmd = new CCmd();
            cmd.db2cmd(cmdstr);

            Thread.Sleep(1000);

            int LNUM = 0;
            foreach(string line in File.ReadAllLines(TempFileName))
            {
                if (line.Trim().Length < 1) continue;
                LNUM++;
                if (LNUM == 1) continue;


            }
        }

        private void GetNodeDirectorys()
        {
            string cmdstr = " db2 list node directory > " + TempFileName;
            cmdstr += "\n exit"; 
            CCmd cmd = new CCmd();
            cmd.db2cmd(cmdstr);

            Thread.Sleep(1000);

            foreach (string line in File.ReadAllLines(TempFileName))
            {
                if (line.Trim().Length < 1) continue;

            }
        }
    }
}
