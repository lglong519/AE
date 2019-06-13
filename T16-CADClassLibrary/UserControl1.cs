using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace T16_CADClassLibrary
{
    public partial class UserControl1 : UserControl
    {
        Form1 frm;
        public UserControl1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (frm != null && frm.Visible)
            {
                frm.Dispose();
                return;
            }
            if (frm == null)
            {
                frm = new Form1();
                frm.Disposed += new EventHandler((object sender1, EventArgs e1) =>
                {
                    frm = null;
                });
            }
            if (!frm.Visible)
            {
                frm.Show();
            }
            frm.Focus();
        }
    }
}
