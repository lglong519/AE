using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.DataSourcesGDB;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.DataManagementTools;
using ESRI.ArcGIS.DataSourcesFile;

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
            for (int n = 0; n < 16; n++)
            {
                comboBox4.Items.Add(n);
            }
            comboBox4.SelectedIndex = 0;
            ToolTip t = new ToolTip();
            t.SetToolTip(button6, "Save");
            t.InitialDelay = 0;
            button6.MouseEnter += new EventHandler((object sender, EventArgs e) => { t.ShowAlways = true; });
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
        // fields
        private void button1_Click(object sender, EventArgs e)
        {
            /*
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
            */
            IActiveView activeView = axMapControl1.ActiveView;
            pGC = activeView.FocusMap as IGraphicsContainer;
            IPoint ipNew = new PointClass();
            //屏幕坐标转地图坐标
            //ipNew = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
            ipNew.PutCoords(0, 0);
            IRgbColor color = new RgbColorClass();
            color.Red = 255;
            color.Blue = 255;
            color.Green = 0;
            /*
            /文字标注
            ITextSymbol symbol = new TextSymbol();
            symbol.Color = color as IColor;
            symbol.Size = 15;
            symbol.Font.Bold = true;
            ITextElement textelement = new TextElementClass();
            textelement.Symbol = symbol;
            textelement.Text = "Text";
            IElement element = textelement as IElement;
            */
            //点标注
            ISimpleMarkerSymbol symbol = new SimpleMarkerSymbolClass();
            symbol.Color = color;
            symbol.Size = 4;
            symbol.Style = esriSimpleMarkerStyle.esriSMSDiamond;
            IMarkerElement markerElement = new MarkerElementClass();
            //markerElement.Symbol = symbol;
            IElement element = markerElement as IElement;

            element.Geometry = ipNew;
            pGC.AddElement(element, 0);
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
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
        // calc deg
        private void button2_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            int indexGDDB = featureClass.FindField("GDDB");
            int indexDLBM = featureClass.FindField("DLBM");
            int indexGZZD = featureClass.FindField("GZZD");
            int indexZWDC1 = featureClass.FindField("ZWDC1");
            int indexCLBXS1 = featureClass.FindField("CLBXS1");
            int indexZWDC2 = featureClass.FindField("ZWDC2");
            int indexCLBXS2 = featureClass.FindField("CLBXS2");
            int indexZWDC3 = featureClass.FindField("ZWDC3");
            int indexCLBXS3 = featureClass.FindField("CLBXS3");
            if (indexGDDB < 0)
            {
                MessageBox.Show("缺少字段：（GDDB）耕地等别");
                return;
            }
            if (indexDLBM < 0)
            {
                MessageBox.Show("缺少字段：（DLBM）地类编码");
                return;
            }
            if (indexGZZD < 0)
            {
                MessageBox.Show("缺少字段：（GZZD）耕地制度");
                return;
            }
            if (indexZWDC1 < 0)
            {
                MessageBox.Show("缺少字段：（ZWDC1）作物单产1");
                return;
            }
            if (indexCLBXS1 < 0)
            {
                MessageBox.Show("缺少字段：（CLBXS1）产量比系数1");
                return;
            }
            if (indexZWDC2 < 0)
            {
                MessageBox.Show("缺少字段：（ZWDC2）作物单产1");
                return;
            }
            if (indexCLBXS2 < 0)
            {
                MessageBox.Show("缺少字段：（CLBXS2）产量比系数1");
                return;
            }
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "DLBM in('01','0101','0102','0103')";
            IFeatureCursor featureCursor = featureClass.Search(queryFilter, false);
            IFeature feature = featureCursor.NextFeature();
            DateTime now = DateTime.Now;

            int count = featureClass.FeatureCount(queryFilter);
            double c = 1;
            progressBar1.Visible = true;

            int i = 0;
            while (feature != null)
            {
                progressBar1.Value = (int)Math.Ceiling(c++ / count * 100.0);
                int GZZD = int.Parse(feature.get_Value(indexGZZD).ToString());
                int d = 0;
                switch (GZZD)
                {
                    case 1: d = deg(double.Parse(feature.get_Value(indexZWDC1).ToString()) * double.Parse(feature.get_Value(indexCLBXS1).ToString()));
                        break;
                    case 2: d = deg(double.Parse(feature.get_Value(indexZWDC1).ToString()) * double.Parse(feature.get_Value(indexCLBXS1).ToString()) + double.Parse(feature.get_Value(indexZWDC2).ToString()) * double.Parse(feature.get_Value(indexCLBXS2).ToString()));
                        break;
                }
                feature.set_Value(indexGDDB, d);
                feature.Store();
                feature = featureCursor.NextFeature();
                i++;
            }
            TimeSpan span = DateTime.Now - now;
            //MessageBox.Show("success count：" + i + "\ncost time: " + span.Minutes + ":" + span.Seconds);
            MessageBox.Show("完成");
            progressBar1.Visible = false;
        }
        private int deg(double dancan)
        {
            if (dancan > 1400)
            {
                return 1;
            }
            if (dancan > 1300 && dancan <= 1400)
            {
                return 2;
            }
            if (dancan > 1200 && dancan <= 1300)
            {
                return 3;
            }
            if (dancan > 1100 && dancan <= 1200)
            {
                return 4;
            }
            if (dancan > 1000 && dancan <= 1100)
            {
                return 5;
            }
            if (dancan > 900 && dancan <= 1000)
            {
                return 6;
            }
            if (dancan > 800 && dancan <= 900)
            {
                return 7;
            }
            if (dancan > 700 && dancan <= 800)
            {
                return 8;
            }
            if (dancan > 600 && dancan <= 700)
            {
                return 9;
            }
            if (dancan > 500 && dancan <= 600)
            {
                return 10;
            }
            if (dancan > 400 && dancan <= 500)
            {
                return 11;
            }
            if (dancan > 300 && dancan <= 400)
            {
                return 12;
            }
            if (dancan > 200 && dancan <= 300)
            {
                return 13;
            }
            if (dancan > 100 && dancan <= 200)
            {
                return 14;
            }
            return 15;
        }
        #region angle
        private void button3_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            double deg;
            if (!double.TryParse(comboBox3.Text.Trim(), out deg))
            {
                MessageBox.Show("invalid angle");
                return;
            };
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            if (featureClass.ShapeType != esriGeometryType.esriGeometryPolygon)
            {
                return;
            }
            IGraphicsContainer pGC = axMapControl1.ActiveView.FocusMap as IGraphicsContainer;
            pGC.DeleteAllElements();
            //int angIndex = featureClass.FindField("angle");
            try
            {
                //if (angIndex < 0)
                //{
                //    IField angField = new FieldClass();
                //    IFieldEdit fieldEdit = angField as IFieldEdit;
                //    fieldEdit.Name_2 = "angle";
                //    fieldEdit.IsNullable_2 = false;
                //    fieldEdit.Required_2 = true;
                //    fieldEdit.DefaultValue_2 = "";
                //    fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                //    featureClass.AddField(angField);
                //}
                //angIndex = featureClass.FindField("angle");
                //if (angIndex < 0)
                //{
                //    throw new Exception("Field: angle not found");
                //}
                //calcPointCount(featureClass);
                IFeatureCursor featureCursor = featureClass.Search(null, false);
                IFeature feature = featureCursor.NextFeature();
                DateTime now = DateTime.Now;
                int count = featureClass.FeatureCount(null);
                double c = 1;
                progressBar1.Visible = true;
                while (feature != null)
                {
                    //MessageBox.Show(featureClass.FindField("angle").ToString());

                    //ThreadPool.QueueUserWorkItem(state => calcAllAngles(feature));
                    //new Thread(o =>
                    //{
                    //calcAllAngles(feature);
                    //progressBar1.Value = (int)Math.Ceiling(c++ / count * 100.0);
                    //}).Start();
                    calcAllAngles(feature);

                    //feature.set_Value(angIndex, calcAllAngles(feature));
                    //feature.Store();
                    //MessageBox.Show(progressBar1.Value.ToString());
                    //this.Refresh();
                    //System.Threading.Thread.Sleep(1200);
                    feature = featureCursor.NextFeature();
                }
                /*
                wse.StopEditOperation();
                wse.StopEditing(true);
                */
                double sc = (DateTime.Now - now).TotalSeconds;
                axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                MessageBox.Show(sc + "--Seconds\ncount: " + c);
                //MessageBox.Show("完成");
                //progressBar1.Visible = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.Message.Contains("schema lock"))
                {
                    MessageBox.Show("图层被占用");
                }
            }
        }
        private void calcAllAngles(IFeature feature)
        {
            if (feature == null)
            {
                return;
            }
            IActiveView activeView = axMapControl1.ActiveView;
            IPointCollection pointCollection = feature.Shape as IPointCollection;
            pGC = activeView.FocusMap as IGraphicsContainer;
            //MessageBox.Show(activeView.FocusMap.Name);

            if (pointCollection.PointCount < 4)
            {
                throw new Exception("Topological Error");
            }
            IPoint f = new PointClass();
            //IPoint c = new PointClass();
            IPoint l = new PointClass();
            int fi;
            int ci;
            int li;
            //string angle = "";
            int pointCount = pointCollection.PointCount - 1;
            for (int i = 0; i < pointCount; i++)
            {
                IPoint c = new PointClass();
                fi = i - 1;
                ci = i;
                li = i + 1;
                if (i == 0)
                {
                    fi = pointCount - 1;
                }
                if (i == pointCount - 1)
                {
                    li = 0;
                }
                pointCollection.QueryPoint(fi, f);
                pointCollection.QueryPoint(ci, c);
                pointCollection.QueryPoint(li, l);
                double deg = getAngle(c, f, l);
                //angle += deg + "; ";
                // 添加标记
                if (deg <= double.Parse(comboBox3.Text.Trim()))
                {
                    IRgbColor color = new RgbColorClass();
                    color.Red = 255;
                    color.Blue = 255;
                    color.Green = 0;
                    ISimpleMarkerSymbol symbol = new SimpleMarkerSymbolClass();
                    symbol.Color = color;
                    symbol.Size = 4;
                    symbol.Style = esriSimpleMarkerStyle.esriSMSDiamond;
                    IMarkerElement markerElement = new MarkerElementClass();
                    markerElement.Symbol = symbol;
                    IElement element = markerElement as IElement;
                    element.Geometry = c;
                    pGC.AddElement(element, 0);
                }
            }
            //MessageBox.Show(angle);
            //return angle;
            return;
        }
        private double getAngle(IPoint center, IPoint first, IPoint last)
        {
            //Console.WriteLine(center.ID);
            double c2 = Math.Pow(first.X - last.X, 2) + Math.Pow(first.Y - last.Y, 2);
            double f2 = Math.Pow(center.X - last.X, 2) + Math.Pow(center.Y - last.Y, 2);
            double l2 = Math.Pow(first.X - center.X, 2) + Math.Pow(first.Y - center.Y, 2);
            double acos = Math.Acos((f2 + l2 - c2) / (2 * Math.Sqrt(f2 * l2)));
            return Math.Round(acos * 180 / Math.PI, 2);
        }
        #endregion

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
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            ITableSort tableSort = new TableSortClass();
            tableSort.Table = featureLayer.FeatureClass as ITable;
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "GDDB=" + comboBox4.Text;
            tableSort.QueryFilter = queryFilter;
            tableSort.Fields = "GDDB";
            tableSort.Sort(null);
            ICursor cursor = tableSort.Rows;
            IRow row = cursor.NextRow();
            int index = featureLayer.FeatureClass.Fields.FindField("GDDB");
            if (row != null)
            {
                IGeometry shape = (row as IFeature).Shape;
                IPoint center = new PointClass();
                center.PutCoords((shape as IArea).Centroid.X, (shape as IArea).Centroid.Y);
                axMapControl1.CenterAt(center);
                //让地图显示窗口立刻重新绘制，更新显示,避免先闪烁后缩放
                axMapControl1.ActiveView.ScreenDisplay.UpdateWindow();
                axMapControl1.FlashShape(shape, 2, 100, symbol);
            }
            else
            {
                MessageBox.Show("null");
            }
        }
        private ISymbol symbol
        {
            get
            {
                ISimpleFillSymbol s = new SimpleFillSymbolClass();
                s.Color = new RgbColorClass()
                {
                    Red = 255,
                    Green = 0,
                    Blue = 0
                };
                return s as ISymbol;
            }
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
            try
            {
                for (int i = 0; i < axMapControl1.Map.LayerCount; i++)
                {
                    IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(i) as IFeatureLayer;
                    if (featureLayer != null && featureLayer.Visible)
                    {
                        IFeatureClass featureClass = featureLayer.FeatureClass;
                        int index = featureClass.FindField("shapeName");
                        if (index < 0)
                        {
                            IField field = new FieldClass();
                            IFieldEdit fieldEdit = field as IFieldEdit;
                            fieldEdit.Name_2 = "shapeName";
                            fieldEdit.AliasName_2 = "文件名";
                            fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                            fieldEdit.Length_2 = 50;
                            featureClass.AddField(field);
                            index = featureClass.FindField("shapeName");
                        }
                        IFeatureCursor featureCursor = featureClass.Search(null, false);
                        IFeature feature = featureCursor.NextFeature();
                        while (feature != null)
                        {
                            if (feature.get_Value(index).ToString() != featureLayer.Name)
                            {
                                feature.set_Value(index, featureLayer.Name);
                                feature.Store();
                            }
                            feature = featureCursor.NextFeature();
                        }
                    }
                }
                MessageBox.Show("完成");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                if (ex.Message.Contains("HRESULT E_FAIL"))
                {
                    MessageBox.Show("图层可能被占用");
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(axMapControl1.DocumentMap);
            //Console.WriteLine(axMapControl1.Map.Name);
            ILayer layer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex);
            IDataLayer2 pDataLayer = layer as IDataLayer2;
            IDatasetName datasetName = pDataLayer.DataSourceName as IDatasetName;
            string pathName = datasetName.WorkspaceName.PathName;
            if (layer is IFeatureLayer)
            {
                //获得图层要素
                IFeatureLayer featureLayer = layer as IFeatureLayer;
                Console.WriteLine("IFeatureLayer: " + featureLayer.Name);

            }
            if (layer is IGroupLayer)
            {
                Console.WriteLine("IGroupLayer: " + layer.Name);
                //遍历图层组
                ICompositeLayer compositeLayer = layer as ICompositeLayer;
                for (int j = 0; j < compositeLayer.Count; j++)
                {
                    if (compositeLayer.get_Layer(j) is IFeatureLayer)
                    {
                        IFeatureLayer featureLayer = compositeLayer.get_Layer(j) as IFeatureLayer;
                        Console.WriteLine("SubFeatureLayer: " + featureLayer.Name);
                    }
                }
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            ILayer layer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex);
            IFeatureLayer featureLayer = layer as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            IPointCollection pointCollection = new PolygonClass();
            IFeatureCursor featureCursor = featureClass.Search(null, false);
            IFeature feature = featureCursor.NextFeature();
            int count = 0;
            while (feature != null)
            {
                if (feature.ShapeCopy != null)
                {
                    count += (feature.ShapeCopy as IPointCollection).PointCount;
                }
                feature = featureCursor.NextFeature();
            }
            TimeSpan span = DateTime.Now - now;
            MessageBox.Show(count + ":" + span.TotalSeconds);
        }
        public int calcPointCount(IFeatureClass featureClass)
        {
            IPointCollection pointCollection = new PolygonClass();
            IFeatureCursor featureCursor = featureClass.Search(null, false);
            IFeature feature = featureCursor.NextFeature();
            int count = 0;
            while (feature != null)
            {
                if (feature.ShapeCopy != null)
                {
                    count += (feature.ShapeCopy as IPointCollection).PointCount;
                }
                feature = featureCursor.NextFeature();
            }
            MessageBox.Show("count:" + count);
            return count;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            double deg;
            if (!double.TryParse(comboBox3.Text.Trim(), out deg))
            {
                MessageBox.Show("invalid angle");
                return;
            };
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            if (featureClass.ShapeType != esriGeometryType.esriGeometryPolygon)
            {
                return;
            }
            IGraphicsContainer pGC = axMapControl1.ActiveView.FocusMap as IGraphicsContainer;
            pGC.DeleteAllElements();
            //int angIndex = featureClass.FindField("angle");
            try
            {
                ITable pTable = featureClass as ITable;
                ICursor pCursor = pTable.Update(null, false);
                IRow pRow = pCursor.NextRow();

                DateTime now = DateTime.Now;
                progressBar1.Visible = true;
                while (pRow != null)
                {
                    calcAllAngles(pRow as IFeature);
                    pRow = pCursor.NextRow();
                }

                double sc = (DateTime.Now - now).TotalSeconds;
                axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                MessageBox.Show(sc + "--Seconds");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.Message.Contains("schema lock"))
                {
                    MessageBox.Show("图层被占用");
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("值不能为空");
                return;
            }
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            IFeatureCursor featureCursor = featureClass.Search(null, false);
            IFeature feature = featureCursor.NextFeature();
            if (feature != null)
            {
                //IAffineTransformation2D transform = feature.Shape as IAffineTransformation2D;
                ITransform2D transform = feature.Shape as ITransform2D;
                IArea area = feature.Shape as IArea;
                IPoint point = (area).Centroid;
                double minus = double.Parse(textBox1.Text);
                double value = Math.Sqrt((area.Area - minus) / area.Area);
                transform.Scale(point, value, value);
                IFeatureBuffer featureBuffer = featureClass.CreateFeatureBuffer();
                featureBuffer.Shape = transform as IGeometry;
                IFeatureCursor cursor = featureLayer.FeatureClass.Insert(true);
                cursor.InsertFeature(featureBuffer);
                cursor.Flush();
            }
            MessageBox.Show("完成");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            IFeatureCursor featureCursor = featureClass.Search(null, false);
            IFeature feature = featureCursor.NextFeature();
            while (feature != null)
            {
                feature.set_Value(feature.Fields.FieldCount - 1, (feature.Shape as IArea).Area);
                //featureCursor.UpdateFeature(feature);
                feature.Store();
                feature = featureCursor.NextFeature();
            }
            MessageBox.Show("完成");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            IFeatureCursor featureCursor = featureClass.Search(null, false);
            IFeature feature = featureCursor.NextFeature();
            int n = 0;
            while (feature != null)
            {
                if (n++ == 0)
                { }
                else
                {
                    feature.Delete();
                }
                feature = featureCursor.NextFeature();
            }
            MessageBox.Show("完成");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            IFeatureCursor featureCursor = featureClass.Search(null, false);
            IFeature feature = featureCursor.NextFeature();
            int n = 0;
            while (feature != null)
            {
                if (feature.Shape == null)
                {
                    feature.Delete();
                }
                feature = featureCursor.NextFeature();
            }
            MessageBox.Show("完成");
        }
    }
}
