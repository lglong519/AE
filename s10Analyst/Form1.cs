using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace s10Analyst
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
            SetDesktopLocation(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width / 2, System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - 300);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (axMapControl1.LayerCount < 1)
            {
                return;
            }
            IFeatureLayer featureLayer = axMapControl1.get_Layer(0) as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "num>200";
            IFeatureCursor featureCursor = featureClass.Search(queryFilter, true);
            IFeature feature = featureCursor.NextFeature();
            if (feature!=null)
            {
                axMapControl1.Map.SelectFeature(featureLayer, feature);
                axMapControl1.Refresh();
            }
        }
    }
}
