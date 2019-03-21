using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;

namespace s6Geodatabase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IAoInitialize ao = new AoInitializeClass();
            ao.Initialize(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB);
            MessageBox.Show("0");
        }
        IWorkspace workspace;
        private void button1_Click(object sender, EventArgs e)
        {
            IPropertySet propertySet = new PropertySetClass();
            propertySet.SetProperty("DATABASE", @"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\data\test\test.mdb");
            IWorkspaceFactory workspaceFactory = new SdeWorkspaceFactory();
            try
            {
                workspace = workspaceFactory.Open(propertySet, 0);
                MessageBox.Show("1");
            }
            catch (Exception ex)
            {
                MessageBox.Show("2");
            }
            
        }
    }
}
