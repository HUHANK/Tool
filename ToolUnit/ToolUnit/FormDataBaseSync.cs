using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolUnit
{
    public partial class FormDataBaseSync : Form
    {
        private List<SDB2Connection> m_db2Alias; /*数据库别名信息*/
        public FormDataBaseSync()
        {
            InitializeComponent();
        }

        private void FormDataBaseSync_Load(object sender, EventArgs e)
        {
            CDB2ConnectInfo db2Info = new CDB2ConnectInfo();
            m_db2Alias = db2Info.m_conns;
            /*初始化下拉框*/
            {
                this.comboBox_dDB.Items.Clear();
                this.comboBox_sDB.Items.Clear();
                foreach (SDB2Connection con in db2Info.m_conns)
                {
                    this.comboBox_sDB.Items.Add(con.alias);
                    this.comboBox_dDB.Items.Add(con.alias);
                }
            }
        }

        private void button_uptTable_Click(object sender, EventArgs e)
        {
            List<string[]> sTables = null;
            List<string[]> dTables = null;

            /*源数据库*/
            string salias = this.comboBox_sDB.Text;
            SDB2Connection conn = null;
            foreach(SDB2Connection dn in m_db2Alias)
            {
                if (dn.alias == salias)
                {
                    conn = dn;
                    break;
                }
            }
            conn.user = this.textBox_sDBUser.Text;
            conn.passwd = this.textBox_sDBPwd.Text;

            {
                CDB2Option opt = new CDB2Option(conn);
                opt.TableSchema = "KS";
                sTables = opt.getAllTables();
            }
            
            /*目标数据库*/
            salias = this.comboBox_dDB.Text;
            foreach (SDB2Connection dn in m_db2Alias)
            {
                if (dn.alias == salias)
                {
                    conn = dn;
                    break;
                }
            }
            conn.user = this.textBox_dDBUser.Text;
            conn.passwd = this.textBox_dDBPwd.Text;

            {
                CDB2Option opt = new CDB2Option(conn);
                opt.TableSchema = "KS";
                dTables = opt.getAllTables();
            }

            int i = 0;
            this.listView_table.Items.Clear();
            foreach (string[] em in sTables)
            {
                bool bMatch = false;
                for(i=0; i<dTables.Count(); i++)
                {
                    if (em[0] == dTables[i][0])
                    {
                        bMatch = true;
                        dTables.Remove(dTables[i]);
                        break;
                    }
                }
                ListViewItem item = this.listView_table.Items.Add(em[0]);
                if (bMatch)
                {
                    item.SubItems.Add(em[0]);
                    item.SubItems.Add(em[1]);
                }
                else
                {
                    item.SubItems.Add("");
                    item.SubItems.Add(em[1]);
                }
            }
        }

        private void button_select_Click(object sender, EventArgs e)
        {
            var items = this.listView_table.SelectedItems;
            foreach(ListViewItem item in items)
            {
                bool badd = true;
                if (item.SubItems[1].Text.Trim().Length > 1)
                {
                    foreach (ListViewItem item2 in this.listView_TblSel.Items)
                    {
                        if (item.Text == item2.Text)
                        {
                            badd = false;
                        }
                    }
                    if (badd)
                        this.listView_TblSel.Items.Add(item.Text);
                }
                else
                {
                    MessageBox.Show("目标数据库中没有表"+item.Text+",无法同步表数据!");
                }
            }
        }

        private void listView_table_DoubleClick(object sender, EventArgs e)
        {
            button_select_Click(sender, e);
        }

        private void listView_TblSel_DoubleClick(object sender, EventArgs e)
        {
            foreach(ListViewItem item in this.listView_TblSel.SelectedItems)
            {
                this.listView_TblSel.Items.Remove(item);
            }
        }

        private void button_sync_Click(object sender, EventArgs e)
        {
            CGenDB2ExpImpBat batFile = new CGenDB2ExpImpBat();

            getSourceDBAndDestDBInfo(out batFile.m_sDB2Info, out batFile.m_dDB2Info);

            foreach(ListViewItem item in this.listView_TblSel.Items)
            {
                batFile.m_tables.Add(item.Text);
            }

            batFile.m_TableSchema = "KS";
            batFile.m_FileName = "NJFKDJHSJFLSJFLS.bat";

            batFile.GenFile();

            CCmd cmd = new CCmd();
            cmd.cmd("db2cmd "+ batFile.m_FileName);
        }

        private bool getSourceDBAndDestDBInfo(out SDB2Connection sDB, out SDB2Connection dDB)
        {
            sDB = null;
            dDB = null;

            string salias = this.comboBox_sDB.Text;
            foreach (SDB2Connection dn in m_db2Alias)
            {
                if (dn.alias == salias)
                {
                    sDB = dn;
                    break;
                }
            }

            salias = this.comboBox_dDB.Text;
            foreach (SDB2Connection dn in m_db2Alias)
            {
                if (dn.alias == salias)
                {
                    dDB = dn;
                    break;
                }
            }

            sDB.user = textBox_sDBUser.Text;
            sDB.passwd = textBox_sDBPwd.Text;
            dDB.user = textBox_dDBUser.Text;
            dDB.passwd = textBox_dDBPwd.Text;

            return true;
        }
        private void button_TestDBConnect_Click(object sender, EventArgs e)
        {
            string ErrMsg1, ErrMsg2;
            SDB2Connection sConn, dConn;
            getSourceDBAndDestDBInfo(out sConn, out dConn);

            CDB2Option dbOpt = new CDB2Option(sConn);
            dbOpt.testConnect(out ErrMsg1);

            dbOpt = new CDB2Option(dConn);
            dbOpt.testConnect(out ErrMsg2);

            if ((ErrMsg1.Length < 1) && (ErrMsg2.Length < 1))
            {
                MessageBox.Show("源数据库连接成功!\r\n目标数据库连接成功!","测试结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if ((ErrMsg1.Length < 1) && (ErrMsg2.Length > 0))
            {
                string str = "源数据库连接成功!\r\n目标数据库连接失败：\r\n";
                str += ErrMsg2;
                MessageBox.Show(str, "测试结果", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if ((ErrMsg1.Length > 0) && (ErrMsg2.Length < 1))
            {
                string str = "目标数据库连接成功!\r\n源数据库连接失败：\r\n";
                str += ErrMsg1;
                MessageBox.Show(str, "测试结果", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string str = "源数据库连接失败：\r\n";
                str += ErrMsg1;
                str += "\r\n目标数据库连接失败：\r\n";
                str += ErrMsg2;
                MessageBox.Show(str, "测试结果", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
