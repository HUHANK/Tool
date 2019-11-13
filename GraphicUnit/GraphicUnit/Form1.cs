using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicUnit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Pen p = new Pen(Color.Blue, 1);

            g.DrawLine(p, 10, 10, 100, 100);
            g.DrawRectangle(p, 10, 10, 100, 100);
            g.DrawEllipse(p, 10, 10, 100, 100);
        }
    }
}
