using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
        private Dictionary<string, SDBTable> m_SelTables;
        private string m_SerializeConnectsPath = "./cache/SDB2Connections.mis";
        public FormDataBaseSync()
        {
            InitializeComponent();
            m_SelTables = new Dictionary<string, SDBTable>();
        }

        private void FormDataBaseSync_Load(object sender, EventArgs e)
        {
            CTool.CheckPathExistOrCreate(m_SerializeConnectsPath);
            if (File.Exists(m_SerializeConnectsPath))
            {
                CSerialize ser = new CSerialize();
                ser.FileName = m_SerializeConnectsPath;
                m_db2Alias = (List<SDB2Connection>)ser.DeSerialize();
            }
            else
            {
                CDB2ConnectInfo db2Info = new CDB2ConnectInfo();
                m_db2Alias = db2Info.m_conns;  
            }

            /*初始化下拉框*/
            {
                this.comboBox_dDB.Items.Clear();
                this.comboBox_sDB.Items.Clear();
                foreach (SDB2Connection con in m_db2Alias)
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
                    {
                        this.listView_TblSel.Items.Add(item.Text);
                        SDBTable tbl = new SDBTable();
                        tbl.name = item.Text;
                        m_SelTables.Add(tbl.name, tbl);
                    }
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
                this.m_SelTables.Remove(item.Text);
            }
        }

        private void button_sync_Click(object sender, EventArgs e)
        {

            CGenDB2ExpImpBat batFile = new CGenDB2ExpImpBat();

            getSourceDBAndDestDBInfo(out batFile.m_sDB2Info, out batFile.m_dDB2Info);
            batFile.m_sDB2Info.user = textBox_sDBUser.Text;
            batFile.m_sDB2Info.passwd = textBox_sDBPwd.Text;

            batFile.m_dDB2Info.user = textBox_dDBUser.Text;
            batFile.m_dDB2Info.passwd = textBox_dDBPwd.Text;

            {
                SDB2Connection sConn, dConn;
                getSourceDBAndDestDBInfo(out sConn, out dConn);

                CDB2Option opt = new CDB2Option(dConn);
                opt.TableSchema = "KS";

                foreach (KeyValuePair<string, SDBTable> kv in m_SelTables)
                {
                    batFile.m_tables.Add(kv.Value);
                    SDBTable stbl = kv.Value;
                    if (stbl.delete_method == "delete")
                    {
                        opt.delete(stbl.name);
                    }
                    else if (stbl.delete_method == "truncate")
                    {
                        opt.truncate(stbl.name);
                    }
                }
            }

            CTool.CheckPathExistOrCreate("./export_data/");
            CTool.DeleteDirAllFiles("./export_data/");

            batFile.m_TableSchema = "KS";
            batFile.m_FileName = "./export_data/NJFKDJHSJFLSJFLS.bat";

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

            return true;
        }
        private void button_TestDBConnect_Click(object sender, EventArgs e)
        {
            string ErrMsg1, ErrMsg2;
            SDB2Connection sConn, dConn;
            getSourceDBAndDestDBInfo(out sConn, out dConn);
            sConn.user = textBox_sDBUser.Text;
            sConn.passwd = textBox_sDBPwd.Text;
            dConn.user = textBox_dDBUser.Text;
            dConn.passwd = textBox_dDBPwd.Text;

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

        private void listView_TblSel_Click(object sender, EventArgs e)
        {
            radioButton_delete.Checked = false;
            radioButton_truncate.Checked = false;
            radioButton_replace.Checked = false;
            radioButton_insert.Checked = false;
            radioButton_insert_update.Checked = false;
            radioButton_delete_clear.Checked = false;
            foreach (ListViewItem item in this.listView_TblSel.SelectedItems)
            {
                SDBTable tbl = m_SelTables[item.Text];
                if (radioButton_delete.Text == tbl.delete_method) radioButton_delete.Checked = true;
                if (radioButton_truncate.Text == tbl.delete_method) radioButton_truncate.Checked = true;

                if (radioButton_replace.Text == tbl.import_method) radioButton_replace.Checked = true;
                if (radioButton_insert.Text == tbl.import_method) radioButton_insert.Checked = true;
                if (radioButton_insert_update.Text == tbl.import_method) radioButton_insert_update.Checked = true;
            }
        }

        private void radioButtonClickProc(RadioButton rd, string way)
        {
            foreach (ListViewItem item in this.listView_TblSel.SelectedItems)
            {
                SDBTable tbl = m_SelTables[item.Text];
                if (way == "delete")
                {
                    if (rd.Checked)
                        tbl.delete_method = rd.Text;
                    else
                        tbl.delete_method = ""; 
                }
                else if ("import" == way)
                {
                    if (rd.Checked)
                        tbl.import_method = rd.Text;
                    else
                        tbl.import_method = "";
                }
            }
        }
        private void radioButton_delete_Click(object sender, EventArgs e)
        {
            radioButtonClickProc(radioButton_delete, "delete");
        }

        private void radioButton_truncate_Click(object sender, EventArgs e)
        {
            radioButtonClickProc(radioButton_truncate, "delete");
        }
        private void radioButton_delete_clear_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_delete_clear.Checked == true)
            {
                radioButton_delete.Checked = false;
                radioButton_truncate.Checked = false;
                radioButtonClickProc(radioButton_delete, "delete");
            }
        }

        private void radioButton_replace_Click(object sender, EventArgs e)
        {
            radioButtonClickProc(radioButton_replace, "import");
        }

        private void radioButton_insert_Click(object sender, EventArgs e)
        {
            radioButtonClickProc(radioButton_insert, "import");
        }

        private void radioButton_insert_update_Click(object sender, EventArgs e)
        {
            radioButtonClickProc(radioButton_insert, "import");
        }

        private void button_SaveConfig_Click(object sender, EventArgs e)
        {
            SDB2Connection sConn, dConn;
            getSourceDBAndDestDBInfo(out sConn, out dConn);
            if (sConn != null)
            {
                sConn.user = textBox_sDBUser.Text;
                sConn.passwd = textBox_sDBPwd.Text;
            }
            if (dConn != null)
            {
                dConn.user = textBox_dDBUser.Text;
                dConn.passwd = textBox_dDBPwd.Text;
            }

            CTool.CheckPathExistOrCreate(m_SerializeConnectsPath);
            CSerialize ser = new CSerialize();
            ser.FileName = m_SerializeConnectsPath;
            ser.Serialize(m_db2Alias);
        }

        private void comboBox_sDB_TextUpdate(object sender, EventArgs e)
        {
            textBox_sDBUser.Text = "";
            textBox_sDBPwd.Text = "";
            //textBox_dDBUser.Text = "";
            //textBox_dDBPwd.Text = "";
            SDB2Connection sConn, dConn;
            getSourceDBAndDestDBInfo(out sConn, out dConn);

            if (sConn != null)
            {
                textBox_sDBUser.Text = sConn.user;
                textBox_sDBPwd.Text = sConn.passwd;
            }
            //if (dConn != null)
            //{
            //    textBox_dDBUser.Text = dConn.user;
            //    textBox_dDBPwd.Text = dConn.passwd;
            //}
        }

        private void comboBox_sDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_sDBUser.Text = "";
            textBox_sDBPwd.Text = "";
            //textBox_dDBUser.Text = "";
            //textBox_dDBPwd.Text = "";
            SDB2Connection sConn, dConn;
            getSourceDBAndDestDBInfo(out sConn, out dConn);

            if (sConn != null)
            {
                textBox_sDBUser.Text = sConn.user;
                textBox_sDBPwd.Text = sConn.passwd;
            }
            //if (dConn != null)
            //{
            //    textBox_dDBUser.Text = dConn.user;
            //    textBox_dDBPwd.Text = dConn.passwd;
            //}
        }

        private void comboBox_dDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //textBox_sDBUser.Text = "";
            //textBox_sDBPwd.Text = "";
            textBox_dDBUser.Text = "";
            textBox_dDBPwd.Text = "";
            SDB2Connection sConn, dConn;
            getSourceDBAndDestDBInfo(out sConn, out dConn);

            //if (sConn != null)
            //{
            //    textBox_sDBUser.Text = sConn.user;
            //    textBox_sDBPwd.Text = sConn.passwd;
            //}
            if (dConn != null)
            {
                textBox_dDBUser.Text = dConn.user;
                textBox_dDBPwd.Text = dConn.passwd;
            }
        }

        private void comboBox_dDB_TextUpdate(object sender, EventArgs e)
        {
            //textBox_sDBUser.Text = "";
            //textBox_sDBPwd.Text = "";
            textBox_dDBUser.Text = "";
            textBox_dDBPwd.Text = "";
            SDB2Connection sConn, dConn;
            getSourceDBAndDestDBInfo(out sConn, out dConn);

            //if (sConn != null)
            //{
            //    textBox_sDBUser.Text = sConn.user;
            //    textBox_sDBPwd.Text = sConn.passwd;
            //}
            if (dConn != null)
            {
                textBox_dDBUser.Text = dConn.user;
                textBox_dDBPwd.Text = dConn.passwd;
            }
        }

        
    }
}
