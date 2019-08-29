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
    public partial class FormDataBaseSyncSave : Form
    {
        private List<SSolutionConfig> m_configs;
        public FormDataBaseSyncSave(object cnf)
        {
            m_configs = ( List < SSolutionConfig > )cnf;
            InitializeComponent();
        }

        private void FormDataBaseSyncSave_Load(object sender, EventArgs e)
        {

        }

        private void button_Save_Click(object sender, EventArgs e)
        {

        }
    }
}
