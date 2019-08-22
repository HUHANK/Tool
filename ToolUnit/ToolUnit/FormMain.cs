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
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormFixFileTime dlg = new FormFixFileTime();
            dlg.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormSearchFiles dlg = new FormSearchFiles();
            dlg.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormFullTextSearch dlg = new FormFullTextSearch();
            dlg.Show();
        }

        private void buttonTB_Click(object sender, EventArgs e)
        {
            FormDataBaseSync dlg = new FormDataBaseSync();
            dlg.Show();
        }
    }
}
