using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace s1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
            //string path = @"C:\Users\pc\Desktop\test1\";
            //string fileName = @"test1.mxd";
            string path = @"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\data\test\";
            string fileName = @"test.mxd";
            //string path = @"C:\Users\pc\Documents\Visual Studio 2010\C\AE\第五章\lesson3\data\World\";
            //string fileName = @"World.mxd";
            axMapControl1.LoadMxFile(path + fileName);
            axMapControl1.Extent = axMapControl1.FullExtent;
        }

        private void axMapControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {

        }

        private void axToolbarControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IToolbarControlEvents_OnMouseDownEvent e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
