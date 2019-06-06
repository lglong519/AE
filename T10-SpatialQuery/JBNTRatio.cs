using System;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace T10_SpatialQuery
{
    public partial class JBNTRatio : Form
    {
        private IMap map { get; set; }
        public JBNTRatio(IMap map)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            this.map = map;
            comboBoxBDTB.SelectedIndexChanged += new EventHandler(comboBoxBDTB_SelectedIndexChanged);
            new System.Threading.Timer(updateComboxLayers, null, 0, 1000);
            ToolTip button1Tip = new ToolTip();
            button1Tip.SetToolTip(button1, "比例大于等于50%的写入1，否则写入0");
        }

        int layerCount = 0;
        //更新下拉图层列表,清空属性列表
        private void updateComboxLayers(object state)
        {
            try
            {
                layerCount = map.LayerCount;
                if (layerCount == 0)
                {
                    if (comboBoxJBNT.Items == null && comboBoxBDTB.Items == null) return;
                    comboBoxJBNT.Items.Clear();
                    comboBoxBDTB.Items.Clear();
                    comboBoxField.Items.Clear();
                    return;
                }
                string[] layers = new string[layerCount];
                string[] layersB = new string[layerCount];
                bool updateFlag = false;
                int jIndex = 0;
                int bIndex = 0;
                for (int i = 0; i < layerCount; i++)
                {
                    ILayer layer = map.get_Layer(i);
                    if (layer != null)
                    {
                        layers[i] = layer.Name;
                        layersB[i] = layer.Name;
                        if (comboBoxJBNT.Items.IndexOf(layer.Name) != i)
                        {
                            updateFlag = true;
                        }
                        if (layer.Name == "JBNT")
                        {
                            jIndex = i;
                        }
                        if (layer.Name == "BDTB")
                        {
                            bIndex = i;
                        }
                    }
                }
                if (!updateFlag) return;
                comboBoxJBNT.Items.Clear();
                comboBoxJBNT.Items.AddRange(layers);
                comboBoxJBNT.SelectedIndex = jIndex;
                comboBoxBDTB.Items.Clear();
                comboBoxBDTB.Items.AddRange(layersB);
                comboBoxBDTB.SelectedIndex = bIndex;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }
        }
        // 选择的图层索引变化后更新属性列表
        private void comboBoxBDTB_SelectedIndexChanged(object sender, EventArgs e)
        {
            IFeatureLayer layer = map.get_Layer(comboBoxBDTB.SelectedIndex) as IFeatureLayer;
            comboBoxField.Items.Clear();
            if (layer != null)
            {
                int fieldCount = layer.FeatureClass.Fields.FieldCount;
                int jIndex = 0;
                int del = 0;
                for (int i = 0; i < fieldCount; i++)
                {
                    string fieldName = layer.FeatureClass.Fields.get_Field(i).Name;
                    if (fieldName == "FID" || fieldName == "Shape")
                    {
                        del++;
                        continue;
                    }
                    comboBoxField.Items.Add(fieldName);
                    if (fieldName == "JBNTBS")
                    {
                        jIndex = i - del;
                    }
                }
                comboBoxField.SelectedIndex = jIndex; ;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxJBNT.SelectedIndex == -1 || comboBoxBDTB.SelectedIndex == -1 || comboBoxField.Text == "")
                {
                    MessageBox.Show("请选择图层和字段");
                    return;
                }
                // targetFeatureLayer==null targetFeatureClass ShapeType==esriGeometryType.esriGeometryPolygon fieldIndex==-1
                IFeatureLayer targetFeatureLayer = map.get_Layer(comboBoxJBNT.SelectedIndex) as IFeatureLayer;
                IFeatureClass targetFeatureClass = targetFeatureLayer.FeatureClass;
                //source
                IFeatureLayer sourceFeatureLayer = map.get_Layer(comboBoxBDTB.SelectedIndex) as IFeatureLayer;
                IFeatureClass sourceFeatureClass = sourceFeatureLayer.FeatureClass;
                int sourceFeatureCount = sourceFeatureClass.FeatureCount(null);
                IFeatureCursor sourceFeatureCursor = sourceFeatureClass.Search(null, false);
                IFeature sourceFeature = sourceFeatureCursor.NextFeature();
                int fieldIndex = sourceFeatureClass.FindField(comboBoxField.Text);
                //start
                while (sourceFeature != null)
                {
                    //process
                    //参考图 查询过滤器
                    ISpatialFilter spatialFilter = new SpatialFilterClass()
                    {
                        Geometry = sourceFeature.Shape as IGeometry,
                        SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects,   //相交
                        GeometryField = sourceFeatureClass.ShapeFieldName
                    };
                    if (targetFeatureClass.FeatureCount(spatialFilter) < 1)
                    {
                        sourceFeature = sourceFeatureCursor.NextFeature();
                        continue;
                    }
                    IFeatureCursor targetFeatureCursor = targetFeatureClass.Search(spatialFilter, false);
                    IFeature targetFeature = targetFeatureCursor.NextFeature();
                    IGeometryCollection targetCollection = new GeometryBagClass();
                    while (targetFeature != null)
                    {
                        targetCollection.AddGeometry(targetFeature.ShapeCopy, Type.Missing, Type.Missing);
                        targetFeature = targetFeatureCursor.NextFeature();
                    }
                    ITopologicalOperator pNewPolygon = new PolygonClass() as ITopologicalOperator;
                    pNewPolygon.ConstructUnion(targetCollection as IEnumGeometry);
                    ITopologicalOperator topo = sourceFeature.Shape as ITopologicalOperator;
                    IGeometry intersection = topo.Intersect(pNewPolygon as IGeometry, esriGeometryDimension.esriGeometry2Dimension);
                    // 交集面积
                    IArea intersectionArea = intersection as IArea;
                    // 本底面积
                    IArea sourceArea = sourceFeature.Shape as IArea;
                    double ratio = Math.Round(intersectionArea.Area / sourceArea.Area, 2);
                    if (ratio >= .5)
                    {
                        sourceFeature.set_Value(fieldIndex, 1);
                    }
                    else
                    {
                        sourceFeature.set_Value(fieldIndex, 0);
                    }
                    sourceFeature.Store();
                    sourceFeature = sourceFeatureCursor.NextFeature();
                }
                //end
                MessageBox.Show("完成");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
