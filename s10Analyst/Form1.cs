using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;

namespace s10Analyst
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
            init();
        }
        private void init()
        {
            SetDesktopLocation(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width / 2 + 300, System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - 500);
            axMapControl1.OnAfterScreenDraw += new IMapControlEvents2_Ax_OnAfterScreenDrawEventHandler(axMapControl1_OnAfterScreenDraw);
            comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
            comboBox2.SelectedIndexChanged += new EventHandler(comboBox2_SelectedIndexChanged);
        }
        private int layerCount = 0;
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
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
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
        // 属性查询
        private void button2_Click(object sender, EventArgs e)
        {
            if (axMapControl1.LayerCount < 1)
            {
                return;
            }
            if (comboBox2.SelectedIndex < 0)
            {
                MessageBox.Show("please select query's field");
                return;
            }
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("please input query's value");
                return;
            }
            IActiveView activeView = axMapControl1.Map as IActiveView;
            IFeatureLayer featureLayer = axMapControl1.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            // 查询前清空选择集
            (featureLayer as IFeatureSelection).Clear();
            IFeatureClass featureClass = featureLayer.FeatureClass;

            IQueryFilter queryFilter = new QueryFilterClass();
            Regex reg = new Regex("id|double|int|float", RegexOptions.IgnoreCase);
            if (reg.IsMatch(featureClass.Fields.get_Field(comboBox2.SelectedIndex).Type.ToString()))
            {
                queryFilter.WhereClause = comboBox2.SelectedItem + "=" + textBox1.Text;
            }
            else
            {
                queryFilter.WhereClause = comboBox2.SelectedItem + "='" + textBox1.Text + "'";
            }
            int count = 0;
            /*
            #region IFeatureCursor
            IFeatureCursor featureCursor;
            IFeature feature = null;
            try
            {
                featureCursor = featureClass.Search(queryFilter, true);
                feature = featureCursor.NextFeature();
                while (feature != null)
                {
                    count++;
                    axMapControl1.Map.SelectFeature(featureLayer, feature);
                    feature = featureCursor.NextFeature();
                }
            }
            catch (Exception)
            {
            }
            #endregion
            */
            #region ITableSort
            ITableSort tableSort = new TableSortClass();
            tableSort.Fields = comboBox2.SelectedItem.ToString();
            tableSort.Table = featureClass as ITable;
            tableSort.QueryFilter = queryFilter;
            tableSort.set_CaseSensitive(comboBox2.SelectedItem.ToString(), false);
            tableSort.Sort(null);
            ICursor cursor = tableSort.Rows;
            IRow row = cursor.NextRow();
            while (row != null)
            {
                count++;
                axMapControl1.Map.SelectFeature(featureLayer, row as IFeature);
                row = cursor.NextRow();
            }
            #endregion

            if (count == 0)
            {
                MessageBox.Show("query's result is empty");
            }
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGeography, featureLayer, null);
        }
        // 空间位置查询
        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("start");
            ISpatialFilter spatialFilter = new SpatialFilterClass();
            IFeatureLayer featureLayer0=axMapControl1.Map.get_Layer(1) as IFeatureLayer;
            spatialFilter.Geometry = featureLayer0.FeatureClass.GetFeature(0) as IGeometry;
            spatialFilter.GeometryField = "Id";
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
            //spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelTouches;
            //spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);
            IFeature feature = featureCursor.NextFeature();
            while (feature!=null)
            {
                axMapControl1.Map.SelectFeature(featureLayer, feature);
                feature = featureCursor.NextFeature();
            }
            axMapControl1.Refresh();
            MessageBox.Show("done");
        }
        // 要素选择集
        private void button4_Click(object sender, EventArgs e)
        {
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "num>130";
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;

            IDataset dataset = featureClass as IDataset;
            ISelectionSet selectionSet = featureClass.Select(queryFilter, esriSelectionType.esriSelectionTypeHybrid, esriSelectionOption.esriSelectionOptionNormal, dataset.Workspace);
            // 获取 OID 集合
            IEnumIDs enumIDs = selectionSet.IDs;
            int id = enumIDs.Next();
            while (id != -1)
            {

                MessageBox.Show("done:" + featureClass.GetFeature(id).get_Value(3));
                id = enumIDs.Next();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
