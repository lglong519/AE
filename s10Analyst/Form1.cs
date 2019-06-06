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
using ESRI.ArcGIS.Display;

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
            if (!checkInput()) return;
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
            MessageBox.Show("query's result:" + count);
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGeography, featureLayer, null);
        }
        // 空间位置查询
        private void button3_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            MessageBox.Show("start");
            ISpatialFilter spatialFilter = new SpatialFilterClass();
            IFeatureLayer featureLayer0 = axMapControl1.Map.get_Layer(1) as IFeatureLayer;
            spatialFilter.Geometry = featureLayer0.FeatureClass.GetFeature(0) as IGeometry;
            spatialFilter.GeometryField = "Id";
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
            //spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelTouches;
            //spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);
            IFeature feature = featureCursor.NextFeature();
            while (feature != null)
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
            if (!checkLayerCount()) return;
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
        //Boundary
        /**
         * 1.获取要素作为拓扑对象
         * 2.使拓扑一致 Simplify
         * 3.获取边界线 Boundary
         * 4.线图形显示
         * */
        private void button1_Click(object sender, EventArgs e)
        {
            if (!checkInput()) return;
            IActiveView activeView = axMapControl1.Map as IActiveView;
            IScreenDisplay screenDisplay = activeView.ScreenDisplay;

            IFeature feature = getFeatureCursor().NextFeature();
            if (feature != null)
            {
                IPolygon polygon = feature.Shape as IPolygon;
                ITopologicalOperator topo = polygon as ITopologicalOperator;
                if (topo == null) return;
                topo.Simplify();
                IGeometry geometry = topo.Boundary;
                if (geometry != null)
                {
                    if (geometry.GeometryType == esriGeometryType.esriGeometryPolyline)
                    {
                        ISymbol symbol = GetSimpleLineSymbol();
                        screenDisplay.StartDrawing(screenDisplay.hDC, (short)esriScreenCache.esriNoScreenCache);
                        screenDisplay.SetSymbol(symbol);
                        screenDisplay.DrawPolyline(geometry);
                        screenDisplay.FinishDrawing();
                    }
                }
            }
        }
        // Buffer
        // 根据选择的图形的缓冲区选择缓冲区范围内的要素
        /*
         * 1.获取要素作为拓扑对象
         * 2.设置缓冲区距离 ConvertPixelsToMapUnits
         * 3.获取缓冲区 Buffer
         * 4.使用缓冲区做空间位置查询
         * 5.高亮查询结果
         */
        private void button5_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IActiveView activeView = axMapControl1.ActiveView;
            IFeatureCursor featureCursor = featureLayer.FeatureClass.Search(null, false);
            IFeature feature = featureCursor.NextFeature();
            if (feature != null)
            {
                ITopologicalOperator topo = feature.Shape as ITopologicalOperator;
                double bufferLength = ConvertPixelsToMapUnits(activeView, 100);
                IGeometry geometry = topo.Buffer(bufferLength);

                ISpatialFilter spatialFilter = new SpatialFilterClass();
                spatialFilter.Geometry = geometry;
                spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                //MessageBox.Show(featureLayer.FeatureClass.ShapeFieldName);
                spatialFilter.GeometryField = featureLayer.FeatureClass.ShapeFieldName;
                spatialFilter.SubFields = comboBox2.SelectedItem.ToString();
                spatialFilter.WhereClause = "";

                IFeatureSelection featureSelection = featureLayer as IFeatureSelection;
                featureSelection.Clear();
                featureSelection.SelectFeatures(spatialFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                ISelectionSet selectionSet = featureSelection.SelectionSet;
                ICursor cursor;
                selectionSet.Search(null, true, out cursor);
                IFeatureCursor fCursor = cursor as IFeatureCursor;
                IFeature pFeature = fCursor.NextFeature();
                while (pFeature != null)
                {
                    axMapControl1.Map.SelectFeature(featureLayer, pFeature);
                    pFeature = fCursor.NextFeature();
                }
                activeView.PartialRefresh(esriViewDrawPhase.esriViewGeography, featureLayer, null);
            }
        }
        // clip
        /*
         * 1.获取要素作为拓扑对象
         * 2.获取包络线的大小的1/3作为裁剪的范围
         * 3.新建几何图形作为裁剪结果的容器
         * 4.裁剪 QueryClipped
         * 5.填充图形显示裁剪结果
         */
        private void button6_Click(object sender, EventArgs e)
        {
            if (!checkInput()) return;
            IFeatureLayer featureLayer = axMapControl1.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IActiveView activeView = axMapControl1.ActiveView;
            IScreenDisplay screenDisplay = activeView.ScreenDisplay;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            IFeatureCursor featureCursor = featureClass.Search(null, false);
            IFeature feature = featureCursor.NextFeature();
            if (feature != null)
            {
                ITopologicalOperator topo = feature.Shape as ITopologicalOperator;
                IEnvelope env = feature.Shape.Envelope;
                double width, height;
                width = env.XMax - env.XMin;
                height = env.YMax - env.YMin;
                env.XMin = env.XMin + width / 3;
                env.XMax = env.XMax - width / 3;
                env.YMin = env.YMin + height / 3;
                env.YMax = env.YMax - height / 3;
                IGeometry geometry = new PolygonClass();
                topo.QueryClipped(env, geometry);
                ISymbol symbol = GetSimpleFillSymbol(100, 255);

                screenDisplay.StartDrawing(screenDisplay.hDC, (short)esriScreenCache.esriNoScreenCache);
                screenDisplay.SetSymbol(symbol);
                screenDisplay.DrawPolygon(geometry);
                screenDisplay.FinishDrawing();
            }
        }
        // ConstructUnion
        /*
         * 1.新建几何对象要素集 GeometryBagClass g
         * 2.g 收集图层一个或多个要素几何对象 Shape
         * 3.新建一个多边形类作为拓扑对象 p
         * 4.拓扑.合并几何对象集 ConstructUnion
         * 5.填充图形显示合并结果 p
         */
        private void button7_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IScreenDisplay screenDisplay = axMapControl1.ActiveView.ScreenDisplay;
            IGeometryCollection geometryCollection = new GeometryBagClass();
            IFeatureClass featureClass = featureLayer.FeatureClass;
            for (int i = 0; i < featureClass.FeatureCount(null); i++)
            {
                geometryCollection.AddGeometry(featureClass.GetFeature(i).Shape, Type.Missing, Type.Missing);
            }
            IPolygon polygon = new PolygonClass();
            ITopologicalOperator topo = polygon as ITopologicalOperator;
            topo.ConstructUnion(geometryCollection as IEnumGeometry);

            ISymbol symbol = GetSimpleFillSymbol(200, 200);
            screenDisplay.StartDrawing(screenDisplay.hDC, (short)esriScreenCache.esriNoScreenCache);
            screenDisplay.SetSymbol(symbol);
            screenDisplay.DrawPolygon(polygon);
            screenDisplay.FinishDrawing();
        }
        // ConvexHull 凸多边形
        /*
         * 1.获取图层某个要素几何图形作为拓扑对象
         * 2.获取凸多边形 ConvexHull
         * 3.线图形显示
         */
        private void button8_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            ITopologicalOperator topo = featureLayer.FeatureClass.GetFeature(0).Shape as ITopologicalOperator;
            IGeometry geometry = topo.ConvexHull();

            IScreenDisplay screenDisplay = axMapControl1.ActiveView.ScreenDisplay;
            ISymbol symbol = GetSimpleLineSymbol();
            screenDisplay.StartDrawing(screenDisplay.hDC, (short)esriScreenCache.esriNoScreenCache);
            screenDisplay.SetSymbol(symbol);
            screenDisplay.DrawPolyline(geometry);
            screenDisplay.FinishDrawing();
        }
        // Cut
        /*
         * 1.获取图层某个要素几何图形作为拓扑对象
         * 2.获取包络线取左下角+右上角，画对角线
         * 3.新建左右几何对象 IGeometry，用来接收切割后的图形
         * 4.使用对角线切割图形 Cut
         * 5.填充图形显示切割结果
         */
        private void button13_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IFeature pfeature = featureLayer.FeatureClass.GetFeature(0);
            ITopologicalOperator topo = pfeature.Shape as ITopologicalOperator;

            IEnvelope env = featureLayer.FeatureClass.GetFeature(0).Shape.Envelope;
            IPolyline polyline = new PolylineClass();
            IPoint fromPoint = new PointClass();
            IPoint toPoint = new PointClass();
            //IGeometry leftPolygon = new PolygonClass();
            //IGeometry rightPolygon = new PolygonClass();
            double x = div(pfeature.Shape, 500, env.XMin, env.XMax);

            IGeometry leftPolygon, rightPolygon;
            fromPoint.PutCoords(x, env.YMin);
            toPoint.PutCoords(x, env.YMax);
            polyline.FromPoint = fromPoint;
            polyline.ToPoint = toPoint;

            topo.Cut(polyline, out leftPolygon, out rightPolygon);
            ISymbol symbol = GetSimpleFillSymbol(200, 200);
            IScreenDisplay screenDisplay = axMapControl1.ActiveView.ScreenDisplay;
            screenDisplay.StartDrawing(screenDisplay.hDC, (short)esriScreenCache.esriNoScreenCache);
            screenDisplay.SetSymbol(symbol);


            IDataset dataset = (IDataset)featureLayer.FeatureClass;
            IWorkspace workspace = dataset.Workspace;
            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)workspace;
            //开启编辑
            workspaceEdit.StartEditing(true);
            workspaceEdit.StartEditOperation();

            IFeatureBuffer feature = featureLayer.FeatureClass.CreateFeatureBuffer();
            feature.Shape = leftPolygon;
            IFeatureCursor cursor = featureLayer.FeatureClass.Insert(true);
            cursor.InsertFeature(feature);
            feature = featureLayer.FeatureClass.CreateFeatureBuffer();
            feature.Shape = rightPolygon;
            cursor.InsertFeature(feature);

            cursor.Flush();
            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            screenDisplay.DrawPolygon(leftPolygon);
            screenDisplay.DrawPolygon(rightPolygon);
            screenDisplay.FinishDrawing();
        }
        private double div(IGeometry shape, double area, double XMin, double XMax)
        {
            IEnvelope env = shape.Envelope;
            double y1 = env.YMin;
            double y2 = env.YMax;
            double width = (XMax - XMin) / 2;
            double x = width + XMin;
            IPoint fromPoint = new PointClass() { X = x, Y = y1 };
            IPoint toPoint = new PointClass() { X = x, Y = y2 };
            IPolyline polyline = new PolylineClass() { FromPoint = fromPoint, ToPoint = toPoint };
            IGeometry leftPolygon, rightPolygon;
            ITopologicalOperator topo = shape as ITopologicalOperator;
            topo.Cut(polyline, out leftPolygon, out rightPolygon);
            double left = Math.Round((leftPolygon as IArea).Area, 2);
            double right = Math.Round((rightPolygon as IArea).Area, 2);
            Console.WriteLine("left:" + left);
            Console.WriteLine("right:" + right);
                        
            if (left == area)
            {
                return x;
            }
            if (left > area)
            {
                return div(shape, area, XMin, x);
            }
            else
            {
                return div(shape, area, x, XMax);
            }
        }
        // Difference
        /*
         * 1.获取要素A、B
         * 2.C=A-B Difference
         * 3.填充显示
         */
        private void button11_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            ITopologicalOperator topo = featureLayer.FeatureClass.GetFeature(0).Shape as ITopologicalOperator;
            IGeometry geometry = featureLayer.FeatureClass.GetFeature(1).Shape as IGeometry;

            IGeometry difference = topo.Difference(geometry);

            ISymbol symbol = GetSimpleFillSymbol(200, 200);
            IScreenDisplay screenDisplay = axMapControl1.ActiveView.ScreenDisplay;
            screenDisplay.StartDrawing(screenDisplay.hDC, (short)esriScreenCache.esriNoScreenCache);
            screenDisplay.SetSymbol(symbol);
            screenDisplay.DrawPolygon(difference);
            screenDisplay.FinishDrawing();
        }
        // Intersection Intersect
        /*
         * 1.获取要素A、B
         * 2.C=A交B Intersect
         * 3.填充显示
         */
        private void button12_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            ITopologicalOperator topo = featureLayer.FeatureClass.GetFeature(0).Shape as ITopologicalOperator;
            IGeometry geometry = featureLayer.FeatureClass.GetFeature(1).Shape as IGeometry;

            IGeometry intersection = topo.Intersect(geometry, esriGeometryDimension.esriGeometry2Dimension);

            ISymbol symbol = GetSimpleFillSymbol(200, 200);
            IScreenDisplay screenDisplay = axMapControl1.ActiveView.ScreenDisplay;
            screenDisplay.StartDrawing(screenDisplay.hDC, (short)esriScreenCache.esriNoScreenCache);
            screenDisplay.SetSymbol(symbol);
            screenDisplay.DrawPolygon(intersection);
            screenDisplay.FinishDrawing();
        }
        // SymmetricDifference
        /*
         * 1.获取要素A、B
         * 2.C=A并B-A交B SymmetricDifference
         * 3.填充显示
         */
        private void button10_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            ITopologicalOperator topo = featureLayer.FeatureClass.GetFeature(0).Shape as ITopologicalOperator;
            IGeometry geometry = featureLayer.FeatureClass.GetFeature(1).Shape as IGeometry;

            IGeometry intersection = topo.SymmetricDifference(geometry);

            ISymbol symbol = GetSimpleFillSymbol(200, 200);
            IScreenDisplay screenDisplay = axMapControl1.ActiveView.ScreenDisplay;
            screenDisplay.StartDrawing(screenDisplay.hDC, (short)esriScreenCache.esriNoScreenCache);
            screenDisplay.SetSymbol(symbol);
            screenDisplay.DrawPolygon(intersection);
            screenDisplay.FinishDrawing();
        }
        // Union
        /*
         * 1.获取要素A、B
         * 2.C=A并B Union
         * 3.填充显示
         */
        private void button9_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            ITopologicalOperator topo = featureLayer.FeatureClass.GetFeature(0).Shape as ITopologicalOperator;
            IGeometry geometry = featureLayer.FeatureClass.GetFeature(1).Shape as IGeometry;

            IGeometry intersection = topo.Union(geometry);

            ISymbol symbol = GetSimpleFillSymbol(200, 200);
            IScreenDisplay screenDisplay = axMapControl1.ActiveView.ScreenDisplay;
            screenDisplay.StartDrawing(screenDisplay.hDC, (short)esriScreenCache.esriNoScreenCache);
            screenDisplay.SetSymbol(symbol);
            screenDisplay.DrawPolygon(intersection);
            screenDisplay.FinishDrawing();
        }
        #region 10.5 空间关系
        // 1.Contains
        private void button14_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IGeometry geometry0 = featureLayer.FeatureClass.GetFeature(0).Shape as IGeometry;
            IGeometry geometry1 = featureLayer.FeatureClass.GetFeature(1).Shape as IGeometry;

            IRelationalOperator relationalOperator = geometry0 as IRelationalOperator;
            bool isContain = relationalOperator.Contains(geometry1);
            this.Text = isContain ? "A Contains B" : "A not Contains B";

            drawAB(geometry0, geometry1);
        }
        // 2.Crosses : polygon Crosses polyline
        private void button15_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IGeometry geometry0 = featureLayer.FeatureClass.GetFeature(0).Shape as IGeometry;
            if (geometry0.GeometryType != esriGeometryType.esriGeometryPolygon)
            {
                MessageBox.Show("invalid type: " + geometry0.GeometryType.ToString());
                return;
            }
            IFeatureLayer lineLayer = axMapControl1.Map.get_Layer(2) as IFeatureLayer;
            IGeometry geometry1 = lineLayer.FeatureClass.GetFeature(0).Shape as IGeometry;

            IRelationalOperator relationalOperator = geometry0 as IRelationalOperator;
            bool isCross = relationalOperator.Crosses(geometry1);

            drawAB(geometry0);
            if (isCross)
            {
                MessageBox.Show("A Crosses B");
            }
            else
            {
                MessageBox.Show("A dosn't Crosses B");
            }
        }
        // 3.Equals
        private void button16_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IGeometry geometry0 = featureLayer.FeatureClass.GetFeature(0).Shape as IGeometry;
            IGeometry geometry1 = featureLayer.FeatureClass.GetFeature(1).Shape as IGeometry;

            IRelationalOperator relationalOperator = geometry0 as IRelationalOperator;
            bool isEqual = relationalOperator.Equals(geometry1);
            MessageBox.Show(isEqual ? "A Equals B" : "A dosn't Equals B");
        }
        // 4.Touch
        private void button17_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IGeometry geometry0 = featureLayer.FeatureClass.GetFeature(0).Shape as IGeometry;
            IGeometry geometry1 = featureLayer.FeatureClass.GetFeature(1).Shape as IGeometry;

            IRelationalOperator relationalOperator = geometry0 as IRelationalOperator;
            bool isTouches = relationalOperator.Touches(geometry1);
            MessageBox.Show(isTouches ? "A Touches B" : "A dosn't Touch B");
        }
        // 5.Disjoin
        private void button18_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IGeometry geometry0 = featureLayer.FeatureClass.GetFeature(0).Shape as IGeometry;
            IGeometry geometry1 = featureLayer.FeatureClass.GetFeature(1).Shape as IGeometry;

            IRelationalOperator relationalOperator = geometry0 as IRelationalOperator;
            bool disjoint = relationalOperator.Disjoint(geometry1);
            MessageBox.Show(disjoint ? "A Disjoint B" : "A joint B");
        }
        // 6.Overlap
        private void button19_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IGeometry geometry0 = featureLayer.FeatureClass.GetFeature(0).Shape as IGeometry;
            IGeometry geometry1 = featureLayer.FeatureClass.GetFeature(1).Shape as IGeometry;

            IRelationalOperator relationalOperator = geometry0 as IRelationalOperator;
            bool isOverlaps = relationalOperator.Overlaps(geometry1);
            MessageBox.Show(isOverlaps ? "A Overlaps B" : "A dosn't Overlap B");
        }
        // 8.Within  A in B
        private void button21_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IGeometry geometry0 = featureLayer.FeatureClass.GetFeature(0).Shape as IGeometry;
            IGeometry geometry1 = featureLayer.FeatureClass.GetFeature(1).Shape as IGeometry;

            IRelationalOperator relationalOperator = geometry0 as IRelationalOperator;
            bool isWithin = relationalOperator.Within(geometry1);
            MessageBox.Show(isWithin ? "A Within B" : "A dosn't Within B");
        }
        // 7.B in A
        private void button20_Click(object sender, EventArgs e)
        {
            if (!checkLayerCount()) return;
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
            IGeometry geometry0 = featureLayer.FeatureClass.GetFeature(0).Shape as IGeometry;
            IFeature feature = getSelected();
            if (feature == null)
            {
                MessageBox.Show("empty selection");
                return;
            }
            IGeometry geometry1 = feature.Shape as IGeometry; ;

            IRelationalOperator relationalOperator = geometry1 as IRelationalOperator;
            bool isWithin = relationalOperator.Within(geometry0);
            MessageBox.Show(isWithin ? "B in A" : "B not in A");
        }
        #endregion

        // 获取选中要素
        private IFeature getSelected()
        {
            if (axMapControl1.Map.SelectionCount < 1)
            {
                return null;
            }
            ISelection selection = axMapControl1.Map.FeatureSelection;
            IEnumFeatureSetup enumFeatureSetup = selection as IEnumFeatureSetup;
            enumFeatureSetup.AllFields = true;
            IEnumFeature enumFeature = enumFeatureSetup as IEnumFeature;
            enumFeature.Reset();
            return enumFeature.Next();
        }
        private void drawAB(IGeometry a, IGeometry b = null)
        {
            ISymbol symbol = GetSimpleFillSymbol(200, 200);
            ISymbol symbo2 = GetSimpleFillSymbol(100, 255);
            IScreenDisplay screenDisplay = axMapControl1.ActiveView.ScreenDisplay;
            screenDisplay.StartDrawing(screenDisplay.hDC, (short)esriScreenCache.esriNoScreenCache);

            screenDisplay.SetSymbol(symbol);
            screenDisplay.DrawPolygon(a);

            if (b != null)
            {
                screenDisplay.SetSymbol(symbo2);
                screenDisplay.DrawPolygon(b);
            }

            screenDisplay.FinishDrawing();
        }
        private IFeatureCursor getFeatureCursor()
        {
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
            return featureClass.Search(queryFilter, false);
        }
        private bool checkInput()
        {
            if (!checkLayerCount())
            {
                return false;
            }
            if (comboBox2.SelectedIndex < 0)
            {
                MessageBox.Show("please select query's field");
                return false;
            }
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("please input query's value");
                return false;
            }
            return true;
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
    }
}
