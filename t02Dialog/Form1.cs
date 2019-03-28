using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace t02Dialog
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PerDialog d=new PerDialog();
            //d.MdiParent = this;
            d.getInput+=new GetInput(d_getInput);
            d.Show();
            this.Enabled = false;
        }
        private void d_getInput(string str)
        {
            label1.Text = str;
            this.Enabled = true;
        }
    }
}
