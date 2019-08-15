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
    public partial class FormFullTextSearch : Form
    {
        public FormFullTextSearch()
        {
            InitializeComponent();
        }

        private void button_selectFile_Click(object sender, EventArgs e)
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
            
            CFetchTaskTread task = new CFetchTaskTread(textBox_filePath.Text.Trim(), textBox_fileType.Text.Trim());

            CTaskProcessThread process = new CTaskProcessThread(textBox_searchText.Text.Trim());

        }

        private void button_cancle_Click(object sender, EventArgs e)
        {

        }
    }   
}
