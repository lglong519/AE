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
    public delegate void GetInput(string str="");
    public partial class PerDialog : Form
    {
        public PerDialog()
        {
            InitializeComponent();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            getInput("");
            base.OnClosing(e);
        }
        public event GetInput getInput;
        private void button1_Click(object sender, EventArgs e)
        {
            close(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            close("");
        }
        private void close(string str)
        {
            getInput(str);
            this.Close();
        }
    }
}
