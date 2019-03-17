using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;

namespace s4Map
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
            comboBox1.Items.Add("图层名称");
            comboBox1.SelectedIndex = 0;
        }
        // 添加 shp
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "open shp";
            //dialog.Filter = "shp layer(*.shp)|*.shp;*.lyr";
            dialog.Filter = "shp layer(*.shp)|*.shp";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string file = dialog.FileName;
                string fileName = dialog.SafeFileName;
                string filePath = file.Replace(fileName, "");
                //MessageBox.Show(file + "\n" + fileName + "\n" + filePath);
                IWorkspaceFactory workspaceFactory;
                IFeatureWorkspace featureWorkspace;
                IFeatureLayer featureLayer;
                workspaceFactory = new ShapefileWorkspaceFactoryClass();
                featureWorkspace = workspaceFactory.OpenFromFile(filePath, 0) as IFeatureWorkspace;

                featureLayer = new FeatureLayerClass();
                featureLayer.FeatureClass = featureWorkspace.OpenFeatureClass(fileName);
                // AliasName 无后缀文件名
                featureLayer.Name = featureLayer.FeatureClass.AliasName;
                if (comboBox1.SelectedItem.ToString() == "图层名称")
                {
                    comboBox1.Items.RemoveAt(0);
                }
                comboBox1.Items.Add(featureLayer.Name);
                comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                //MessageBox.Show(featureLayer.FeatureClass.AliasName + "\n" + fileName);
                axMapControl1.AddLayer(featureLayer);
                axMapControl1.Refresh();
            }
        }
        // 鼠标按下事件
        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            /*
            IMap pMap = axMapControl1.Map;
            IActiveView activeView = pMap as IActiveView;
            // TrackRectangle 返回 axMapControl1 的 IEnvelope，通过该对象选择实体？
            IEnvelope pEnv = axMapControl1.TrackRectangle();
            // ISelectionEnvironment 用于设置高亮颜色，查询范围
            // 1.查询范围 => 选中的元素
            ISelectionEnvironment PSelectionEnv = new SelectionEnvironmentClass();
            // 2.高亮的颜色
            PSelectionEnv.DefaultColor = getRGB(110,120,210);
            pMap.ClearSelection();
            // 
            pMap.SelectByShape(pEnv, PSelectionEnv,false);
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,null,null);
            */
        }
        private IRgbColor getRGB(int r,int g,int b)
        {
            IRgbColor rgb = new RgbColorClass();
            rgb.Red = r;
            rgb.Green = g;
            rgb.Blue = b;
            return rgb;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IMap pMap = axMapControl1.Map;
            Type type = pMap.GetType();
            string prop = "";
            foreach(PropertyInfo p in type.GetProperties()){
                MessageBox.Show(p.ToString());
                prop += p;
            }
            MessageBox.Show(prop);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            IGraphicsContainer graphicsContainer;
            IMap map = axMapControl1.Map;
            ILineElement lineElement = new LineElementClass();
            IElement element;
            IPolyline polyline = new PolylineClass();
            IPoint point = new PointClass();
            point.PutCoords(1,5);
            polyline.FromPoint = point;
            point.PutCoords(1000, 5000);
            polyline.ToPoint = point;

            element = lineElement as IElement;
            element.Geometry = polyline as IGeometry;
            // map 转 IGraphicsContainer 后可以使用 AddElement Update* Delete*
            graphicsContainer = map as IGraphicsContainer;
            graphicsContainer.AddElement(element,0);
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics,null,null);
        }
    }
}
