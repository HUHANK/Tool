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
            foreach(string[] em in sTables)
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
    }
}
