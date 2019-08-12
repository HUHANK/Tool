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

        private void textBoxOnTextChange(object sender, EventArgs e)
        {
            m_QueryPath = textBox_filePath.Text.Trim();
            if (!Directory.Exists(m_QueryPath)) return;
            //MessageBox.Show(m_QueryPath);
            DirectoryInfo di = new DirectoryInfo(m_QueryPath);
            listView1.Items.Clear();

            foreach(FileSystemInfo fsi in di.GetFileSystemInfos())
            {
                string fileName = fsi.Name;
                string fileType = "";
                Color color = Color.Black;

                if (fsi is DirectoryInfo)
                {
                    fileName = fileName + "\\";
                    fileType = "文件夹";
                    color = Color.Blue;
                }
                else
                {
                    fileType = "文件";  
                }

                ListViewItem item = listView1.Items.Add(fileName);
                item.SubItems.Add(fsi.LastWriteTime.ToString());
                item.SubItems.Add(fileType);
                //item.ForeColor = color;
                item.UseItemStyleForSubItems = false;
                item.SubItems[0].ForeColor = color;
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            System.Console.WriteLine("Double Click!");
            var items = this.listView1.SelectedItems;
            foreach(ListViewItem item in items)
            {
                System.Console.WriteLine(item.ToString());
                System.Console.WriteLine(item.Text);
                System.Console.WriteLine(item.SubItems[2].Text);
                if (item.SubItems[2].Text.Trim() == "文件夹")
                {
                    textBox_filePath.Text = textBox_filePath.Text + item.Text;
                }
            }
            
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            string filePath = textBox_filePath.Text;
            string[] arr = filePath.Split('\\');
            string newFilePath = "";
            if (arr.Length  == 2)
            {
                //do nothing
                return;
            }else if(arr.Length > 2)
            {
                for(int i=0; i<=arr.Length-3; i++)
                {
                    newFilePath += arr[i] + "\\";
                }
            }
            textBox_filePath.Text = newFilePath;
            
        }
    }
}
