using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ToolUnit
{
    public partial class FormSearchFiles : Form
    {
        string m_SearchPath;
        string m_SearchText;
        public FormSearchFiles()
        {
            InitializeComponent();
        }

        private void button_setPath_Click(object sender, EventArgs e)
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

        private void button_search_Click(object sender, EventArgs e)
        {
            m_SearchPath = textBox_filePath.Text.Trim();
            if (!Directory.Exists(m_SearchPath))
            {
                MessageBox.Show("路径["+m_SearchPath+"]不存在，请检查！");
                m_SearchPath = "";
                return;
            }
            m_SearchText = textBox_searchTxt.Text.Trim();

            this.listBox1.Items.Clear();
            SearchDirFiles(m_SearchPath);
        }

        private void SearchDirFiles(string dir)
        {
            DirectoryInfo di = new DirectoryInfo(dir);
            FileInfo[] fis = di.GetFiles();
            foreach(FileInfo fi in fis)
            {
                string FileName = fi.Name;
                FileName = FileName.Remove(FileName.LastIndexOf("."));
                if (FileName.ToLower().Contains(m_SearchText.ToLower()))
                {
                    this.listBox1.Items.Add(fi.FullName);
                }
            }

            DirectoryInfo[] dis = di.GetDirectories();
            foreach(DirectoryInfo de in dis)
            {
                this.SearchDirFiles(de.FullName);
            }

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            Console.WriteLine(this.listBox1.SelectedItem.ToString());
            string filePath = this.listBox1.SelectedItem.ToString();
            //filePath = filePath.Remove(filePath.LastIndexOf("\\"));
            Console.WriteLine(filePath);

            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
            psi.Arguments = "/e,/select," + filePath;
            System.Diagnostics.Process.Start(psi);
        }
    }
}
