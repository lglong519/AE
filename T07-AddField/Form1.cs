using System;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace p_statistics
{
    public partial class Form1 : Form
    {
        private int layerCount = 0;
        private IGraphicsContainer pGC;
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
            CheckForIllegalCrossThreadCalls = false;
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

        private bool checkLayerCount()
        {
            if (axMapControl1.LayerCount < 1)
            {
                MessageBox.Show("empty layer");
                return false;
            }
            return true;
        }


        private void button4_Click(object sender, EventArgs e)
        {
            pGC = axMapControl1.Map as IGraphicsContainer;
            pGC.DeleteAllElements();
            axMapControl1.ActiveView.Refresh();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            axMapControl1.Map.ClearLayers();
            axTOCControl1.Update();
            axTOCControl1.Refresh();
            axMapControl1.ActiveView.Refresh();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (axMapControl1.DocumentFilename == null)
            {
                MessageBox.Show("mapDocument is empty");
                return;
            }
            IMapDocument pMapDocument = new MapDocumentClass();
            pMapDocument.Open(axMapControl1.DocumentFilename, "");
            pMapDocument.ReplaceContents(axMapControl1.Map as IMxdContents);
            if (pMapDocument.get_IsReadOnly(axMapControl1.DocumentFilename))
            {
                MessageBox.Show("mapDocument is read-only");
            }
            else
            {
                pMapDocument.Save(pMapDocument.UsesRelativePaths, true);
                pMapDocument.Close();
                MessageBox.Show("保存文档成功");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            if (checkBox1.Checked)
            {
                for (int i = 0; i < axMapControl1.Map.LayerCount; i++)
                {
                    processOneLayer(i);
                }
            }
            else
            {
                if (comboBox1.SelectedIndex == -1) return;
                processOneLayer(comboBox1.SelectedIndex);
            }
            MessageBox.Show("完成");
        }
        private bool processOneLayer(int layerIndex)
        {
            IMap map = axMapControl1.Map;
            IFeatureLayer featureLayer = map.get_Layer(layerIndex) as IFeatureLayer;
            if (featureLayer == null) return false;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            if (featureClass == null) return false;
            try
            {
                string fieldName = "shapeName";
                int index = featureClass.FindField(fieldName);
                if (index < 0)
                {
                    IField field = new FieldClass();
                    IFieldEdit fieldEdit = field as IFieldEdit;
                    fieldEdit.Name_2 = fieldName;
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    fieldEdit.AliasName_2 = "文件名";
                    fieldEdit.Length_2 = 50;
                    fieldEdit.Editable_2 = true;
                    featureClass.AddField(fieldEdit);
                    index = featureClass.FindField(fieldName);
                }

                IQueryFilter pFilter = new QueryFilterClass();
                pFilter.WhereClause = String.Format("{0} is null OR {0}<>'{1}'", fieldName, featureLayer.Name);
                pFilter.SubFields = fieldName;
                ITable pTable = featureClass as ITable;
                IRowBuffer pBuffer = pTable.CreateRowBuffer();
                pBuffer.set_Value(index, featureLayer.Name);
                pTable.UpdateSearchedRows(pFilter, pBuffer);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFilter);
            }
            catch (Exception ex)
            {
                string res = string.Format("[{0}]设置失败，错误信息：{1}", featureLayer.Name, ex.Message);
                MessageBox.Show(res);
                return false;
            }
            return true;
        }
    }
}
