using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolUnit
{
    public partial class FormFixFileTime : Form
    {
        private string m_QueryPath;
        public FormFixFileTime()
        {
            InitializeComponent();
        }

        private void FormFixFileTime_Load(object sender, EventArgs e)
        {

        }

        private void button_select_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                textBox_filePath.Text = foldPath;
            }
        }

        private void textBoxOnTextChange(object sender, EventArgs e)
        {
            m_QueryPath = textBox_filePath.Text.Trim();
            if (!Directory.Exists(m_QueryPath)) return;
            //MessageBox.Show(m_QueryPath);
            DirectoryInfo di = new DirectoryInfo(m_QueryPath);
            listView1.Items.Clear();

            foreach(FileSystemInfo fsi in di.GetFileSystemInfos())
            {
                ListViewItem item = listView1.Items.Add(fsi.Name);
                item.SubItems.Add(fsi.LastWriteTime.ToString());

                if (fsi is DirectoryInfo)
                {
                    item.ForeColor = Color.Blue;
                    item.SubItems.Add("文件夹");
                }
                else
                {
                    item.SubItems.Add("文件");
                }
            }
        }
    }
}
