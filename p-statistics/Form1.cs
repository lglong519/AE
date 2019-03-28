using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace p_statistics
{
    public partial class Form1 : Form
    {
        private int layerCount = 0;
        public Form1()
        {
            init(0);
            InitializeComponent();
            init(1);
        }
        private void init(int i = 1)
        {
            if (i == 0)
            {
                ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
                ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
                return;
            }
            SetDesktopLocation(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width / 2 + 300, System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - 500);
            axMapControl1.OnAfterScreenDraw += new IMapControlEvents2_Ax_OnAfterScreenDrawEventHandler(axMapControl1_OnAfterScreenDraw);
            comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
        }
        // 地图绘制完成后更新下拉图层列表
        private void axMapControl1_OnAfterScreenDraw(object sender, IMapControlEvents2_OnAfterScreenDrawEvent e)
        {
            if (layerCount != axMapControl1.Map.LayerCount)
            {
                updateComboxLayers();
            }
        }
        // 选择的图层索引变化后更新属性列表
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            IFeatureLayer layer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            comboBox2.Items.Clear();
            if (layer != null)
            {
                int fieldCount = layer.FeatureClass.Fields.FieldCount;
                for (int i = 0; i < fieldCount; i++)
                {
                    comboBox2.Items.Add(layer.FeatureClass.Fields.get_Field(i).Name);
                    if (i == 0)
                    {
                        comboBox2.SelectedIndex = i;
                    }
                }
            }
        }
        //更新下拉图层列表,清空属性列表
        private void updateComboxLayers()
        {
            layerCount = axMapControl1.Map.LayerCount;
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            if (axMapControl1.Map.LayerCount == 0)
            {
                return;
            }
            for (int i = 0; i < axMapControl1.Map.LayerCount; i++)
            {
                ILayer layer = axMapControl1.Map.get_Layer(i);
                if (layer != null)
                {
                    comboBox1.Items.Add(layer.Name);
                    if (i == 0)
                    {
                        comboBox1.SelectedIndex = i;
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            IFeatureCursor featureCursor = featureClass.Search(null, false);
            IFeature feature = featureCursor.NextFeature();
            while (feature != null)
            {
                string val = "";
                for (int i = 0; i < feature.Fields.FieldCount; i++)
                {
                    //.Shape.GeometryType
                    val += "type: " + feature.get_Value(i).GetType().ToString() + "  ";
                    val += feature.Fields.get_Field(i).Name + ": ";
                    val += feature.get_Value(i).ToString() + "\n";
                }
                MessageBox.Show(val);
                feature = featureCursor.NextFeature();
                break;
            }

        }
        private bool checkLayerCount()
        {
            if (axMapControl1.LayerCount < 1)
            {
                MessageBox.Show("empty layer");
                return false;
            }
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            IFeatureCursor featureCursor = featureClass.Search(null, false);
            IFeature feature = featureCursor.NextFeature();
            DateTime now = DateTime.Now;
            int i = 0;
            while (feature != null)
            {
                int index = feature.Fields.FindField("GDDB");
                if (index < 0 || !feature.Fields.get_Field(index).Editable)
                {
                    feature = featureCursor.NextFeature();
                    continue;
                }
                //feature.set_Value(index, Math.Floor(i / 6180.00));
                feature.set_Value(index, i);
                feature.Store();
                feature = featureCursor.NextFeature();
                i++;
            }
            int sc = (DateTime.Now - now).Seconds;
            if (i == 0)
            {
                MessageBox.Show("GDDB not found" + "\n--ms: " + sc);
            }
            else
            {
                MessageBox.Show("modified features count: " + i + "\n--ms: " + sc);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            if (featureClass.ShapeType != esriGeometryType.esriGeometryPolygon)
            {
                return;
            }
            int angIndex = featureClass.FindField("angle");
            /*
            IDataset dataset = featureClass as IDataset;
            IWorkspaceEdit wse = dataset.Workspace as IWorkspaceEdit;
            wse.StartEditing(true);
            wse.StartEditOperation();
            */
            if (angIndex < 0)
            {
                IField angField = new FieldClass();
                IFieldEdit fieldEdit = angField as IFieldEdit;
                fieldEdit.Name_2 = "angle";
                fieldEdit.IsNullable_2 = false;
                fieldEdit.Required_2 = true;
                fieldEdit.DefaultValue_2 = "";
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                featureClass.AddField(angField);
            }
            angIndex = featureClass.FindField("angle");
            if (angIndex < 0)
            {
                throw new Exception("Field: angle not found");
            }
            

            IFeatureCursor featureCursor = featureClass.Search(null, false);
            IFeature feature = featureCursor.NextFeature();
            DateTime now = DateTime.Now;
            while (feature != null)
            {
                //MessageBox.Show(featureClass.FindField("angle").ToString());
                //feature.set_Value(angIndex, "");
                //feature.Store();
                //feature = featureCursor.NextFeature();
                IPointCollection pointCollection = feature.Shape as IPointCollection;
                MessageBox.Show(pointCollection.PointCount.ToString());
                IPoint f = new PointClass();
                IPoint c = new PointClass();
                IPoint l = new PointClass();
                pointCollection.QueryPoint(0, f);
                pointCollection.QueryPoint(1, c);
                pointCollection.QueryPoint(2, l);
                MessageBox.Show("angle:" + getAngle(c, f, l));
                return;
                //MessageBox.Show("--feature");
            }
            /*
            wse.StopEditOperation();
            wse.StopEditing(true);
            */
            int sc = (DateTime.Now - now).Seconds;
            MessageBox.Show(sc + "--Seconds");
        }
        private void getAndSetAllAngles(IFeature feature)
        {
            IPointCollection pointCollection = feature.Shape as IPointCollection;
            if (pointCollection.PointCount<3)
            {
                throw new Exception("Topological Error");
            }
            IPoint f = new PointClass();
            IPoint c = new PointClass();
            IPoint l = new PointClass();
            int fi;
            int ci;
            int li;
            for (int i = 0; i < pointCollection.PointCount; i++)
            {
                fi = i - 1;
                ci = i;
                li = i + 1;
                if (i==0)
                {
                    fi = pointCollection.PointCount - 1;
                }
                if (i == pointCollection.PointCount-1)
                {
                    fi = 0;
                }
                pointCollection.QueryPoint(fi, f);
                pointCollection.QueryPoint(ci, c);
                pointCollection.QueryPoint(li, l);
            }
        }
        private double getAngle(IPoint center,IPoint first,IPoint last)
        {
            double c2 = Math.Pow(first.X - last.X, 2) + Math.Pow(first.Y - last.Y, 2);
            double f2 = Math.Pow(center.X - last.X, 2) + Math.Pow(center.Y - last.Y, 2);
            double l2 = Math.Pow(first.X - center.X, 2) + Math.Pow(first.Y - center.Y, 2);
            double acos=Math.Acos((f2 + l2 - c2) / (2 * Math.Sqrt(f2 * l2)));
            return acos*180/Math.PI;
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
