using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;

namespace s4Map
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
            comboBox1.Items.Add("图层名称");
            comboBox1.SelectedIndex = 0;
            SetDesktopLocation(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - 300, System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - 300);
            axMapControl1.OnMouseDown += new IMapControlEvents2_Ax_OnMouseDownEventHandler(axMapControl2_OnMouseDown);
            checkBox1.Click += new EventHandler(checkBox1_Click);
            checkBox2.Click += new EventHandler(checkBox2_Click);
            checkBox3.Click += new EventHandler(checkBox3_Click);
        }
        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;
                checkBox3.Checked = false;
            }
        }
        private void checkBox2_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Checked = false;
                checkBox3.Checked = false;
            }
        }
        private void checkBox3_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
            }
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
                //if (comboBox1.SelectedItem.ToString() == "图层名称")
                //{
                //    comboBox1.Items.RemoveAt(0);
                //}
                //comboBox1.Items.Add(featureLayer.Name);
                //comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                //MessageBox.Show(featureLayer.FeatureClass.AliasName + "\n" + fileName);
                axMapControl1.AddLayer(featureLayer);
                axMapControl1.Refresh();

                if (axMapControl1.LayerCount > 0)
                {
                    string[] items = new string[axMapControl1.LayerCount];
                    for (int i = 0; i < axMapControl1.LayerCount; i++)
                    {
                        items[i] = axMapControl1.get_Layer(i).Name;
                    }
                    comboBox1.Items.Clear();
                    comboBox1.Items.AddRange(items);
                    comboBox1.SelectedIndex = items.Length-1;
                }
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
        private IRgbColor getRGB(int r, int g, int b)
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
            
            //Type type = pMap.GetType();
            //string prop = "";
            //foreach (PropertyInfo p in type.GetProperties())
            //{
            //    MessageBox.Show(p.ToString());
            //    prop += p;
            //}
            MessageBox.Show(pMap.LayerCount.ToString());
        }
        // IGraphicsContainer
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            IGraphicsContainer graphicsContainer;
            IMap map = axMapControl1.Map;
            ILineElement lineElement = new LineElementClass();
            IElement element;
            IPolyline polyline = new PolylineClass();
            IPoint point = new PointClass();
            point.PutCoords(1, 5);
            polyline.FromPoint = point;
            point.PutCoords(1000, 5000);
            polyline.ToPoint = point;

            element = lineElement as IElement;
            element.Geometry = polyline as IGeometry;
            // map 转 IGraphicsContainer 后可以使用 AddElement Update* Delete*
            graphicsContainer = map as IGraphicsContainer;
            graphicsContainer.AddElement(element, 0);
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {

        }
        // searchFeatures
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Regex reg = new Regex("^\\d+$");
            if (axMapControl1.LayerCount == 0 || textBox1.Text.Trim().Length < 1 || !reg.Match(textBox1.Text.Trim()).Success)
            {
                return;
            }
            //int fid = int.Parse(textBox1.Text);
            int fid;
            int.TryParse(textBox1.Text, out fid);
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
            searchFeatures("FID=" + fid, featureLayer);
        }
        // searchSelection
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            Regex reg = new Regex("^\\d+$");
            if (axMapControl1.LayerCount == 0 || textBox1.Text.Trim().Length < 1 || !reg.Match(textBox1.Text.Trim()).Success)
            {
                return;
            }
            int fid;
            int.TryParse(textBox1.Text, out fid);
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
            searchSelection("FID=" + fid, featureLayer);
        }
        // IFeatureLayer
        private void searchFeatures(string sqlFilter, IFeatureLayer featureLayer)
        {
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = sqlFilter;
            IFeatureCursor cursor = featureLayer.Search(queryFilter, true);
            IFeature feature = cursor.NextFeature();
            while (feature != null)
            {
                ISimpleFillSymbol symbol = new SimpleFillSymbolClass();
                symbol.Color = getRGB(220, 100, 50);
                // ?
                object oFillsyl = symbol;
                //IPolygon polygon = feature.Shape as IPolygon;
                IGeometry pShape = feature.Shape as IGeometry;
                // flash
                axMapControl1.FlashShape(pShape, 2, 80, oFillsyl);
                //axMapControl1.DrawShape(polygon, ref oFillsyl);
                feature = cursor.NextFeature();
            }
        }
        // IFeatureSelection
        private void searchSelection(string sqlFilter, IFeatureLayer featureLayer)
        {
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = sqlFilter;

            IFeatureSelection featureSelection = featureLayer as IFeatureSelection;
            // 选择选择集
            featureSelection.SelectFeatures(queryFilter,esriSelectionResultEnum.esriSelectionResultNew,true);
            featureSelection.SelectionColor = getRGB(90,240,120);

            axMapControl1.Refresh(esriViewDrawPhase.esriViewGeoSelection,null,null);

            ISelectionSet selectionSet = featureSelection.SelectionSet;
            ICursor cursor;
            selectionSet.Search(null, true, out cursor);
            IFeatureCursor featureCursor = cursor as IFeatureCursor;
            IFeature feature = featureCursor.NextFeature();
            while (feature != null)
            {
                ISimpleFillSymbol symbol = new SimpleFillSymbolClass();
                symbol.Color = getRGB(220, 100, 50);
                //MessageBox.Show(feature.Shape.GeometryType.ToString());
                axMapControl1.FlashShape(feature.Shape, 2, 80, symbol);
                feature = featureCursor.NextFeature();
            }
            // 清空选择集
            featureSelection.Clear();
        }
        // createLayer btn
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            CreateSelLayer();
        }
        private void CreateSelLayer()
        {
            if (axMapControl1.Map.LayerCount < 1)
            {
                MessageBox.Show("Layer is empty");
                return;
            }
            IMap map = axMapControl1.Map;
            IActiveView activeView = map as IActiveView;
            IFeatureLayer featureLayer = map.get_Layer(0) as IFeatureLayer;
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "FID>0";

            IFeatureSelection featureSelection = featureLayer as IFeatureSelection;
            featureSelection.SelectFeatures(queryFilter,esriSelectionResultEnum.esriSelectionResultNew,false);

            IFeatureLayerDefinition definition = featureLayer as IFeatureLayerDefinition;
            definition.DefinitionExpression = "FID>1";
            if (featureSelection.SelectionSet.Count < 1)
            {
                MessageBox.Show("SelectionSet is empty");
                return;
            }
            else
            {
                MessageBox.Show("SelectionSet.Count: " + featureSelection.SelectionSet.Count);
            }
            IFeatureLayer newFeatureLayer=definition.CreateSelectionLayer("new_"+featureLayer.Name,true,"","");

            featureSelection.Clear();
            map.AddLayer(newFeatureLayer);
            activeView.Refresh();
        }
        // ILayerFields
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            if (axMapControl1.Map.LayerCount < 1)
            {
                MessageBox.Show("Layer is empty");
                return;
            }
            IMap map = axMapControl1.Map;
            IActiveView activeView = map as IActiveView;
            IFeatureLayer featureLayer = map.get_Layer(0) as IFeatureLayer;
            IFeature feature= featureLayer.Search(null,true).NextFeature();
            
            int index = feature.Fields.FindField("num");
            if (index < 0) return;
            MessageBox.Show(feature.get_Value(index).ToString());
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            IIdentify identify = axMapControl1.Map.get_Layer(0) as IIdentify;
            IArray array;
            //array = identify.Identify(axMapControl1.Map.get_Layer(0) as IGeometry);
            array = identify.Identify(null);
            if (array != null)
            {
                IIdentifyObj idObj = array.get_Element(1) as IIdentifyObj;
                //idObj.Flash(axMapControl1.ActiveView.ScreenDisplay);
                MessageBox.Show("Identify done: \nLayerName: " + idObj.Layer.Name + "\n" + array.Count);
            }
            else
            {
                MessageBox.Show("IArray null");
            }
        }
        // ScreanDisplay
        private void axMapControl2_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            label2.Text = "x:"+e.mapX +" y:"+ e.mapY;
            // ScreanDisplay
            if (checkBox1.Checked)
            {
                IScreenDisplay screenDisplay = axMapControl1.ActiveView.ScreenDisplay;
                ISimpleLineSymbol lineSymbol = new SimpleLineSymbolClass();
                IRgbColor color = new RgbColorClass();
                color.Red = 255;
                lineSymbol.Color = color;
                IPolyline polyline = axMapControl1.TrackLine() as IPolyline;

                screenDisplay.StartDrawing(screenDisplay.hDC, (short)esriScreenCache.esriNoScreenCache);
                screenDisplay.SetSymbol(lineSymbol as ISymbol);
                screenDisplay.DrawPolyline(polyline);
                screenDisplay.FinishDrawing();
            }
            // LineElement | MarkerElement
            if (checkBox2.Checked)
            {
                IPoint point = new PointClass();
                point.PutCoords(e.mapX, e.mapY);

                ISimpleMarkerSymbol markerSymbol = new SimpleMarkerSymbolClass();
                markerSymbol.Color = getRGB(11,200,145);
                markerSymbol.Size = 4;
                markerSymbol.Style = esriSimpleMarkerStyle.esriSMSDiamond;

                IMarkerElement markerElement = new MarkerElementClass();
                //IElement element = markerElement as IElement;
                //element.Geometry = point;
                (markerElement as IElement).Geometry = point;
                markerElement.Symbol = markerSymbol;

                IGraphicsContainer graphicsContainer = axMapControl1.Map as IGraphicsContainer;
                graphicsContainer.AddElement(markerElement as IElement, 0);
                axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics,null,null);
            }
            // TextElement
            if (checkBox3.Checked)
            {
                IPoint point = new PointClass();
                point.PutCoords(e.mapX, e.mapY);

                ITextElement textElement = new TextElementClass();
                textElement.Text = "textElement";
                (textElement as IElement).Geometry = point;

                IGraphicsContainer graphicsContainer = axMapControl1.Map as IGraphicsContainer;
                graphicsContainer.AddElement(textElement as IElement, 0);
                axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
        }
    }
}
