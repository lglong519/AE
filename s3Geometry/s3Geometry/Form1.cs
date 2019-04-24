using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
// AE 命名空间
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.GeocodingTools;
using ESRI.ArcGIS.CatalogUI;
using ESRI.ArcGIS.Catalog;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;

namespace s3Geometry
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            //ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
        }
        private void addFeature(string layerName, IGeometry geometry)
        {
            try
            {
                ILayer layer = null;
                for (int i = 0; i < axMapControl1.LayerCount; i++)
                {
                    layer = axMapControl1.Map.get_Layer(i);
                    if (layer.Name.ToLower() == layerName)
                    {
                        break;
                    }
                }
                if (layer == null)
                {
                    MessageBox.Show("图层为空");
                    return;
                }
                // layer FeatureClass dataset Workspace workspaceEdit StartEditing StartEditOperation
                // featureClass CreateFeatureBuffer featureBuffer
                // featureBuffer Search featureCursor NextFeature feature Delete
                // featureClass Insert featureCursor InsertFeature Flush
                IFeatureLayer featureLayer = layer as IFeatureLayer;
                IFeatureClass featureClass = featureLayer.FeatureClass;
                IDataset dataset = (IDataset)featureClass;
                IWorkspace workspace = dataset.Workspace;
                IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)workspace;
                workspaceEdit.StartEditing(true);
                workspaceEdit.StartEditOperation();
                IFeatureBuffer featureBuffer = featureClass.CreateFeatureBuffer();
                // param {boolean} bool 是否共享内存，true 会报错：对 COM 组件的调用返回了错误 HRESULT E_FAIL
                IFeatureCursor featureCursor;
                //featureCursor = featureClass.Search(null, false);
                //IFeature feature = featureCursor.NextFeature();
                //while (feature != null)
                {
                    //feature.Delete();
                    //feature = featureCursor.NextFeature();
                }
                featureCursor = featureClass.Insert(true);
                // 报错：No support for this geometry type.
                // 解决：查看featureClass的 geometry type 是否与 geometry 的相同
                if (featureClass.ShapeType != geometry.GeometryType)
                {
                    MessageBox.Show(string.Format("{0},{1}", featureClass.ShapeType, geometry.GeometryType));
                    return;
                }
                featureBuffer.Shape = geometry;
                object featureOID = featureCursor.InsertFeature(featureBuffer);
                featureCursor.Flush();
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
                //MessageBox.Show("完成");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                if (ex.Message.Contains("HRESULT:0x8004022D"))
                {
                    MessageBox.Show("图层被占用");
                }
            }
        }
        #region AddGeometry
        private void addGeometryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IGeometryCollection geometryCollection = new MultipointClass();
            //IMultipoint multipoint;
            object missing = Type.Missing;
            for (int i = 0; i < 10; i++)
            {
                IPoint point = new PointClass();
                point.PutCoords(i * 200, i * 200);

                geometryCollection.AddGeometry(point as IGeometry, Type.Missing, ref missing);
            }
            //multipoint = geometryCollection as IMultipoint;
            //addFeature("multi_point", multipoint as IGeometry);
            addFeature("multi_point", geometryCollection as IGeometry);
            //axMapControl1.Extent = multipoint.Envelope;
            axMapControl1.Refresh();
        }

        // AddGeometryCollection
        private void addGeometryCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IGeometryCollection geometryCollection1 = new MultipointClass();
            IGeometryCollection geometryCollection2 = new MultipointClass();
            IMultipoint multipoint;
            object missing = Type.Missing;
            IPoint point;
            for (int i = 0; i < 10; i++)
            {
                point = new PointClass();
                point.PutCoords(i * 224, i * 2);

                geometryCollection1.AddGeometry(point as IGeometry, ref missing, ref missing);
            }
            geometryCollection2.AddGeometryCollection(geometryCollection1);
            multipoint = geometryCollection2 as IMultipoint;
            addFeature("multi_point", multipoint as IGeometry);
            axMapControl1.Extent = multipoint.Envelope;
            axMapControl1.Refresh();
        }

        private void 加载ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadMapDocument();
        }
        // 加载地图文档
        private void loadMapDocument()
        {

            IMapDocument mapDocument = new ESRI.ArcGIS.Carto.MapDocumentClass();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "打开地图文档文件";
            // 提示+文件类型
            openFileDialog.Filter = "map documents(*.mxd)|*.mxd";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //MessageBox.Show("select");
                string filePath = openFileDialog.FileName;
                // 加载地图文档用于更新或其它操作
                mapDocument.Open(filePath, "");
                if (axMapControl1.CheckMxFile(filePath))
                {
                    axMapControl1.MousePointer = esriControlsMousePointer.esriPointerHourglass;
                    axMapControl1.LoadMxFile(filePath, 0, Type.Missing);
                    axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
                    axMapControl1.Extent = axMapControl1.FullExtent;
                }
                else
                {
                    MessageBox.Show("INVALID_FILE_PATH: " + filePath);
                }
            }
            else
            {
                mapDocument = null;
            }
        }
        // insert
        private void insertGeometryCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IGeometryCollection geometryCollection1 = new MultipointClass();
            IGeometryCollection geometryCollection2 = new MultipointClass();
            IGeometryCollection geometryCollection3 = new MultipointClass();
            IMultipoint multipoint;
            object missing = Type.Missing;
            IPoint point;
            for (int i = 0; i < 10; i++)
            {
                point = new PointClass();
                point.PutCoords(i * 2, i);
                geometryCollection1.AddGeometry(point as IGeometry, ref missing, ref missing);
            }
            for (int i = 0; i < 10; i++)
            {
                point = new PointClass();
                point.PutCoords(i, i);
                geometryCollection2.AddGeometry(point as IGeometry, ref missing, ref missing);
            }
            for (int i = 0; i < 10; i++)
            {
                point = new PointClass();
                point.PutCoords(i, i * 999);
                geometryCollection3.AddGeometry(point as IGeometry, ref missing, ref missing);
            }
            // 对象集插入另一个对象集的指定索引位置
            geometryCollection1.InsertGeometryCollection(1, geometryCollection2);
            geometryCollection1.InsertGeometryCollection(1, geometryCollection3);

            multipoint = geometryCollection1 as IMultipoint;
            addFeature("multi_point", multipoint as IGeometry);
            axMapControl1.Extent = multipoint.Envelope;
            axMapControl1.Refresh();
        }
        // setGeometries
        private void setGeometriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IGeometryCollection geometryCollection1 = new MultipointClass();
            IGeometryBridge geometryBridge = new GeometryEnvironmentClass();
            IGeometry[] geometryArray = new IGeometry[10];
            IMultipoint multipoint;
            object missing = Type.Missing;
            IPoint point;
            for (int i = 0; i < 10; i++)
            {
                point = new PointClass();
                point.PutCoords(i * 2, i * 2018);
                geometryArray[i] = point as IGeometry;
            }
            geometryBridge.SetGeometries(geometryCollection1, ref geometryArray);

            multipoint = geometryCollection1 as IMultipoint;
            addFeature("multi_point", multipoint as IGeometry);
            //addFeature("point", geometryCollection1 as IGeometry);
            //axMapControl1.Extent = multipoint.Envelope;
            axMapControl1.Refresh();
        }
        #endregion
        #region ISegmentCollection
        // add
        private void addSegmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int wave = 100;
            ISegment[] segmentArray = new ISegment[wave];
            IPolyline polyline = new PolylineClass();
            ISegmentCollection segmentCollection = new PolylineClass();
            for (int i = 0; i < wave; i++)
            {
                ILine line = new LineClass();
                IPoint fromPoint = new PointClass();
                if (i % 2 == 0)
                {
                    fromPoint.PutCoords(i * 10000, 0);
                }
                else
                {
                    fromPoint.PutCoords(i * 10000, 10000);
                }
                IPoint toPoint = new PointClass();
                toPoint.PutCoords(i * 10000, 10000);
                line.PutCoords(fromPoint, toPoint);
                segmentArray[i] = line as ISegment;
                segmentCollection.AddSegment(line as ISegment, Type.Missing, Type.Missing);
            }
            polyline = segmentCollection as IPolyline;
            addFeature("polyline", polyline as IGeometry);
            axMapControl1.Extent = polyline.Envelope;
            axMapControl1.Refresh();
        }
        // query
        private void querySegmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ISegment[] segmentArray = new ISegment[10];
            for (int i = 0; i < 10; i++)
            {
                ILine line = new LineClass();
                IPoint fromPoint = new PointClass();
                fromPoint.PutCoords(i * 10, i * 10);
                IPoint toPoint = new PointClass();
                toPoint.PutCoords(i * 15000, i * 15000);
                line.PutCoords(fromPoint, toPoint);
                segmentArray[i] = line as ISegment;
            }
            ISegmentCollection segmentCollection = new PolylineClass();
            IGeometryBridge geometryBridge = new GeometryEnvironmentClass();
            geometryBridge.AddSegments(segmentCollection, ref segmentArray);

            int index = 0;
            ISegment[] outputSegmentArray = new ISegment[segmentCollection.SegmentCount - index];
            for (int i = 0; i < 10; i++)
            {
                outputSegmentArray[i] = new LineClass();
            }
            geometryBridge.QuerySegments(segmentCollection, index, ref outputSegmentArray);
            string report = "";
            for (int i = 0; i < outputSegmentArray.Length; i++)
            {
                ISegment currentSegment = outputSegmentArray[i];
                ILine line = currentSegment as ILine;
                report = report + "index = " + i + " , FromPoint X= " + line.FromPoint.X + " , FromPoint Y= " + line.FromPoint.Y;
                report = string.Format("{0} ,ToPoint x={1},ToPoint Y={2}\n", report, line.ToPoint.X, line.ToPoint.Y);
            }
            MessageBox.Show(report);
        }

        private void setSegmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IPolyline polyline = new PolylineClass();
            ISegmentCollection segmentCollection = new PolylineClass();
            IGeometryBridge geometryBridge = new GeometryEnvironmentClass();
            ISegment[] segmentArray = new ISegment[5];
            for (int i = 0; i < 5; i++)
            {
                ILine line = new LineClass();
                IPoint fromPoint = new PointClass();
                fromPoint.PutCoords(i, 1);
                IPoint toPoint = new PointClass();
                toPoint.PutCoords(i * 100, 1000000);
                line.PutCoords(fromPoint, toPoint);
                segmentArray[i] = line as ISegment;
            }
            geometryBridge.SetSegments(segmentCollection, ref segmentArray);

            polyline = segmentCollection as IPolyline;
            addFeature("polyline", polyline as IGeometry);
            axMapControl1.Extent = polyline.Envelope;
            axMapControl1.Refresh();
        }
        #endregion
        #region IPointCollection
        private void addPointCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IPointCollection4 pointCollection1 = new MultipointClass();
            IPointCollection pointCollection2 = new MultipointClass();
            IGeometryBridge geometryBridge = new GeometryEnvironmentClass();
            IPoint[] points = new PointClass[10];
            IMultipoint multipoint;
            object missing = Type.Missing;
            IPoint point;
            for (int i = 0; i < 10; i++)
            {
                point = new PointClass();
                int a = i % 2 == 0 ? 1 : -1;
                point.PutCoords(i * 200 * a, i * 500);
                points[i] = point;
            }
            geometryBridge.SetPoints(pointCollection1, ref points);
            pointCollection2.AddPointCollection(pointCollection1);
            multipoint = pointCollection2 as IMultipoint;
            addFeature("multi_point", multipoint as IGeometry);
            axMapControl1.Extent = multipoint.Envelope;
            axMapControl1.Refresh();
        }
        // 查询点坐标
        private void queryPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IPoint point1 = new PointClass();
            point1.PutCoords(10, 10);
            IPoint point2 = new PointClass();
            point2.PutCoords(10000, 10000);
            IPoint[] inputPoint = new IPoint[2] { point1, point2 };
            IPointCollection4 pointCollection = new MultipointClass();
            IGeometryBridge geometryBridge = new GeometryEnvironmentClass();
            geometryBridge.AddPoints(pointCollection, ref inputPoint);

            //int index = 0;
            IPoint[] outputPoint = new IPoint[2] { new PointClass(), new PointClass() };
            pointCollection.QueryPoint(0, outputPoint[0]);
            for (int i = 0; i < outputPoint.Length; i++)
            {
                IPoint point = outputPoint[i];
                if (point.IsEmpty == true)
                {
                    MessageBox.Show("point is null");
                }
                else
                {
                    MessageBox.Show(string.Format("x= {0},y={1}", point.X, point.Y));
                }
            }
        }
        // 更新：点替换
        private void updatePointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IMultipoint multipoint;
            object missing = Type.Missing;
            IPoint point1 = new PointClass();
            point1.PutCoords(10, 10);
            IPoint point2 = new PointClass();
            point2.PutCoords(10000, 10000);
            IPointCollection4 pointCollection = new MultipointClass();
            pointCollection.AddPoint(point1, ref missing, ref missing);
            pointCollection.AddPoint(point2, ref missing, ref missing);
            point1 = new PointClass();
            point1.PutCoords(20000, 25000);
            pointCollection.UpdatePoint(1, point1);
            multipoint = pointCollection as IMultipoint;
            addFeature("multi_point", multipoint as IGeometry);
            axMapControl1.Extent = multipoint.Envelope;
            axMapControl1.Refresh();
        }
        #endregion
        // 设置坐标系
        private void alterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (axMapControl1.LayerCount == 0)
            {
                MessageBox.Show("layer is null");
                return;
            }
            IFeatureLayer layer = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
            IFeatureClass featureClass = layer.FeatureClass;
            IGeoDataset geoDataset = featureClass as IGeoDataset;
            IGeoDatasetSchemaEdit geoDatasetEdit = geoDataset as IGeoDatasetSchemaEdit;
            if (geoDatasetEdit.CanAlterSpatialReference == true)
            {
                ISpatialReferenceFactory2 spatRefFact = new SpatialReferenceEnvironmentClass();
                // 4214 beijing1954
                // 4547 2000
                IGeographicCoordinateSystem geoSys = spatRefFact.CreateGeographicCoordinateSystem(4214);
                geoDatasetEdit.AlterSpatialReference(geoSys);
                axMapControl1.ActiveView.Refresh();
                MessageBox.Show("geoDatasetEdit");

            }
        }
        // 获取坐标系
        private void getToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (axMapControl1.LayerCount == 0)
            {
                MessageBox.Show("layer is null");
                return;
            }
            IFeatureLayer layer = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
            IFeatureClass featureClass = layer.FeatureClass;
            IGeoDataset geoDataset = featureClass as IGeoDataset;
            ISpatialReference spatialReference = geoDataset.SpatialReference;
            MessageBox.Show(spatialReference.Name);
        }

        private void setToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (axMapControl1.LayerCount == 0)
            {
                MessageBox.Show("layer is null");
                return;
            }
            //ISpatialReferenceDialog dialog=new SpatialReferenceDialogClass();
            ISpatialReferenceDialog2 dialog = new SpatialReferenceDialogClass();
            IProjectedCoordinateSystem projectCoordSystem = dialog.DoModalCreate(true, false, false, 0) as IProjectedCoordinateSystem;
            axMapControl1.Map.SpatialReference = projectCoordSystem;
            axMapControl1.ActiveView.Refresh();
        }

        private void 圆形点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IFeatureLayer layer = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
            IFeatureClass featureClass = layer.FeatureClass;
            IPoint point = new PointClass();
            IPoint point2 = new PointClass();
            int param = 500000;
            int x = new Random().Next(param * 2);
            double a = -param;
            double y = Math.Sqrt(Math.Pow(param, 2) - Math.Pow(x + a, 2)) - a;
            double round = Math.Round(new Random().NextDouble());
            point.PutCoords(x, y);
            point2.PutCoords(x, 2 * param - y);
            IDataset dataset = (IDataset)featureClass;
            IWorkspace workspace = dataset.Workspace;
            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)workspace;

            workspaceEdit.StartEditing(true);
            workspaceEdit.StartEditOperation();

            IFeature feature = featureClass.CreateFeature();
            feature.Shape = point;
            feature.Store();
            feature = featureClass.CreateFeature();
            feature.Shape = point2;
            feature.Store();

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);
            axMapControl1.ActiveView.Refresh();
            //释放游标
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
        }

        private void 多点检测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IFeatureLayer layer = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
            IFeatureClass featureClass = layer.FeatureClass;
            IFeatureCursor featureCursor = featureClass.Search(null, false);
            IFeature feature = featureCursor.NextFeature();
            Console.WriteLine("start");
            Console.WriteLine("feature count:" + featureClass.FeatureCount(null));
            while (feature != null)
            {
                Console.WriteLine(feature.Shape.IsEmpty);
                feature = featureCursor.NextFeature();
            }
            Console.WriteLine("end");
        }

        private void divideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IFeatureLayer layer = axMapControl1.Map.get_Layer(4) as IFeatureLayer;
            IFeatureClass featureClass = layer.FeatureClass;
            IFeatureCursor featureCursor = featureClass.Search(null, false);
            IFeature feature = featureCursor.NextFeature();
            if (feature != null)
            {
                IDataset dataset = (IDataset)featureClass;
                IWorkspace workspace = dataset.Workspace;
                IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)workspace;
                workspaceEdit.StartEditing(true);
                workspaceEdit.StartEditOperation();

                ISegmentCollection collection = (ISegmentCollection)feature.Shape;
                ISegmentCollection collection2 = new PolylineClass();
                Console.WriteLine(collection.SegmentCount);
                ISegment segment = collection.Segment[0];
                ISegment[] newCollection = new ISegment[2];
                segment.SplitAtDistance(10000, false, out newCollection[0], out newCollection[1]);
                collection2.AddSegment(newCollection[0]);
                collection2.AddSegment(newCollection[1]);
                //collection.AddSegment(fSegment);
                //collection.AddSegment(tSegment);
                collection.RemoveSegments(0, 1, false);
                collection.InsertSegmentCollection(0, collection2);
                feature.Delete();
                featureCursor.Flush();
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);
                //feature.Store();
                addFeature("polyline", collection as IGeometry);
            }
        }

        private void 检测表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string vectorFileFullName = @"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\data\test\point.dbf"; //dbf全路径
            //IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();
            //IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(vectorFileFullName), 0);
            //IFeatureWorkspace pFeatureWorkspaceShp = pWorkspace as IFeatureWorkspace;
            ////ITable pTable = pFeatureWorkspaceShp.OpenTable(System.IO.Path.GetFileName(vectorFileFullName));
            //ITable pTable = pFeatureWorkspaceShp.OpenTable("point.dbf");
            //DataTable dt = ToDataTable(pTable);


            //dataGridView1.Columns.Add("RowState", "rowstate");//添加新列
            //foreach (DataColumn col in dt.Columns)
            //{
            //    dataGridView1.Columns.Add(col.ColumnName, col.ColumnName);
            //}
            //dataGridView1.Rows.Clear();
            //dataGridView1.Rows.Add(dt.Rows.Count);//增加同等数量的行数
            //int i = 0;
            //foreach (DataRow row in dt.Rows)//逐个读取单元格的内容；
            //{
            //    DataGridViewRow r1 = dataGridView1.Rows[i];
            //    r1.Cells[0].Value = row.RowState.ToString();
            //    for (int j = 0; j < dt.Columns.Count; j++)
            //    {
            //        r1.Cells[j + 1].Value = row[j].ToString();
            //    }
            //    i++;
            //}
            string filePath = @"C:\Users\lglong519\Documents\AE\data\test\polyline.shp";
            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();
            IWorkspaceFactoryLockControl pWorkspaceFactoryLockControl = pWorkspaceFactory as IWorkspaceFactoryLockControl;
            if (pWorkspaceFactoryLockControl.SchemaLockingEnabled)
            {
                pWorkspaceFactoryLockControl.DisableSchemaLocking();
            }
            IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(filePath), 0);
            IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
            IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(System.IO.Path.GetFileName(filePath));
            Console.WriteLine("123");
        }

        //将ITable转换为DataTable方法
        public DataTable toDataTable(ITable mTable)
        {
            try
            {
                DataTable pTable = new DataTable();
                for (int i = 0; i < mTable.Fields.FieldCount; i++)
                {
                    pTable.Columns.Add(mTable.Fields.get_Field(i).Name);
                }

                ICursor pCursor = mTable.Search(null, false);
                IRow pRrow = pCursor.NextRow();
                while (pRrow != null)
                {

                }
                {
                    DataRow pRow = pTable.NewRow();
                    string[] StrRow = new string[pRrow.Fields.FieldCount];
                    for (int i = 0; i < pRrow.Fields.FieldCount; i++)
                    {
                        StrRow[i] = pRrow.get_Value(i).ToString();
                    }
                    pRow.ItemArray = StrRow;
                    pTable.Rows.Add(pRow);
                    pRrow = pCursor.NextRow();
                }

                return pTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void shxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string shpfilepath = "";

        }

        private void constructAlongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILine line = new LineClass();
            IPoint fromPoint = new PointClass();
            fromPoint.PutCoords(0, 0);
            IPoint toPoint = new PointClass();
            toPoint.PutCoords(10000, 10000);
            line.PutCoords(fromPoint, toPoint);
            ICurve curve = line as ICurve;

            IConstructPoint constructPoint = new PointClass();
            constructPoint.ConstructAlong(curve, esriSegmentExtension.esriNoExtension, 10, true);
            IPoint point1 = constructPoint as IPoint;

            constructPoint = new PointClass();
            constructPoint.ConstructAlong(curve, esriSegmentExtension.esriNoExtension, 2000, false);
            IPoint point2 = constructPoint as IPoint;

            //ISpatialReferenceFactory2 spatRefFact = new SpatialReferenceEnvironmentClass();
            //IProjectedCoordinateSystem geoSys = spatRefFact.CreateProjectedCoordinateSystem(4547);
            //point2.SpatialReference = geoSys as ISpatialReference;

            drawMapShape(point1 as IGeometry);
            drawMapShape(point2 as IGeometry);
        }
        private void drawMapShape(IGeometry geometry)
        {
            // 定义图形样式：颜色，线框大小
            IRgbColor rgbColor = new RgbColorClass()
            {
                Red = 255,
                Green = 0,
                Blue = 0
            };
            object symbol = null;
            ISimpleMarkerSymbol simpleMarkerSymbol = new SimpleMarkerSymbolClass();
            simpleMarkerSymbol.Color = rgbColor;
            symbol = simpleMarkerSymbol;
            axMapControl1.DrawShape(geometry, ref symbol);
        }

        private void shpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ISpatialReferenceFactory2 spatRefFact = new SpatialReferenceEnvironmentClass();
            IProjectedCoordinateSystem geoSys = spatRefFact.CreateProjectedCoordinateSystem(4547);
            ISpatialReference spatialReference = geoSys as ISpatialReference;
            //IFeatureClass featureClass = CreateMemoryFeatureClass(spatialReference, esriGeometryType.esriGeometryPoint, "point");
            //IPoint point = new PointClass();
            //point.PutCoords(123, 456);
            //IFeature feature = featureClass.CreateFeature();
            //feature.Shape = point;
            //feature.Store();
            //IFeatureLayer featureLayer = new FeatureLayerClass();
            //featureLayer.FeatureClass = featureClass;
            //featureLayer.Name = "ppt";
            IFields fields = new FieldsClass();
            IFeatureLayer featureLayer = CreateFeatureLayerInmemeory("newdataset", "new layer", spatialReference, esriGeometryType.esriGeometryPoint, fields);
            axMapControl1.AddLayer(featureLayer);
        }
        public static IFeatureClass CreateMemoryFeatureClass(ISpatialReference spatialReference, esriGeometryType geometryType, string name = "Temp")
        {
            // 创建内存工作空间
            IWorkspaceFactory pWSF = new InMemoryWorkspaceFactoryClass();
            IWorkspaceName pWSName = pWSF.Create(@"C:\Users\lglong519\Documents\AE\data\", "Temp", null, 0);
            IName pName = (IName)pWSName;
            IWorkspace memoryWS = (IWorkspace)pName.Open();

            IField field = new FieldClass();
            IFields fields = new FieldsClass();
            IFieldsEdit fieldsEdit = fields as IFieldsEdit;
            IFieldEdit fieldEdit = field as IFieldEdit;


            fieldEdit.Name_2 = "OBJECTID";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            fieldEdit.IsNullable_2 = false;
            fieldEdit.Required_2 = false;
            fieldsEdit.AddField(field);

            field = new FieldClass();
            fieldEdit = field as IFieldEdit;
            IGeometryDef geoDef = new GeometryDefClass();
            IGeometryDefEdit geoDefEdit = (IGeometryDefEdit)geoDef;
            geoDefEdit.AvgNumPoints_2 = 5;
            geoDefEdit.GeometryType_2 = geometryType;
            geoDefEdit.GridCount_2 = 1;
            geoDefEdit.HasM_2 = false;
            geoDefEdit.HasZ_2 = false;
            geoDefEdit.SpatialReference_2 = spatialReference;
            fieldEdit.Name_2 = "SHAPE";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
            fieldEdit.GeometryDef_2 = geoDef;
            fieldEdit.IsNullable_2 = true;
            fieldEdit.Required_2 = true;
            fieldsEdit.AddField(field);

            field = new FieldClass();
            fieldEdit = field as IFieldEdit;
            fieldEdit.Name_2 = "Code";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
            fieldEdit.IsNullable_2 = true;
            fieldsEdit.AddField(field);

            //创建要素类
            IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)memoryWS;
            IFeatureClass featureClass = featureWorkspace.CreateFeatureClass(
                name, fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");

            return featureClass;
        }


        /// <summary>

        /// 在内存中创建图层

        /// </summary>

        /// <param name="DataSetName">数据集名称</param>

        /// <param name="AliaseName">别名</param>

        /// <param name="SpatialRef">空间参考</param>

        /// <param name="GeometryType">几何类型</param>

        /// <param name="PropertyFields">属性字段集合</param>

        /// <returns>IfeatureLayer</returns>

        public static IFeatureLayer CreateFeatureLayerInmemeory(string DataSetName, string AliaseName, ISpatialReference SpatialRef, esriGeometryType GeometryType, IFields PropertyFields)
        {

            IWorkspaceFactory workspaceFactory = new InMemoryWorkspaceFactoryClass();

            ESRI.ArcGIS.Geodatabase.IWorkspaceName workspaceName = workspaceFactory.Create("", "MyWorkspace", null, 0);

            ESRI.ArcGIS.esriSystem.IName name = (IName)workspaceName;

            ESRI.ArcGIS.Geodatabase.IWorkspace inmemWor = (IWorkspace)name.Open();

            IField oField = new FieldClass();

            IFields oFields = new FieldsClass();

            IFieldsEdit oFieldsEdit = null;

            IFieldEdit oFieldEdit = null;

            IFeatureClass oFeatureClass = null;

            IFeatureLayer oFeatureLayer = null;

            try
            {

                oFieldsEdit = oFields as IFieldsEdit;

                oFieldEdit = oField as IFieldEdit;

                for (int i = 0; i < PropertyFields.FieldCount; i++)
                {

                    oFieldsEdit.AddField(PropertyFields.get_Field(i));

                }

                IGeometryDef geometryDef = new GeometryDefClass();

                IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;

                geometryDefEdit.AvgNumPoints_2 = 5;

                geometryDefEdit.GeometryType_2 = GeometryType;

                geometryDefEdit.GridCount_2 = 1;

                geometryDefEdit.HasM_2 = false;

                geometryDefEdit.HasZ_2 = false;

                geometryDefEdit.SpatialReference_2 = SpatialRef;

                oFieldEdit.Name_2 = "SHAPE";

                oFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

                oFieldEdit.GeometryDef_2 = geometryDef;

                oFieldEdit.IsNullable_2 = true;

                oFieldEdit.Required_2 = true;

                oFieldsEdit.AddField(oField);

                oFeatureClass = (inmemWor as IFeatureWorkspace).CreateFeatureClass(DataSetName, oFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");

                (oFeatureClass as IDataset).BrowseName = DataSetName;

                oFeatureLayer = new FeatureLayerClass();

                oFeatureLayer.Name = AliaseName;

                oFeatureLayer.FeatureClass = oFeatureClass;

            }

            catch
            {

            }

            finally
            {

                try
                {

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oField);

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oFields);

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oFieldsEdit);

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oFieldEdit);

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(name);

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workspaceFactory);

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workspaceName);

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(inmemWor);

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oFeatureClass);

                }

                catch { }
                GC.Collect();
            }

            return oFeatureLayer;

        }

        private void constructAlongSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {

            IFeature feature = getSelectedFeature();
            ICurve curve = feature.Shape as ICurve;
            IConstructPoint constructPoint = new PointClass();
            constructPoint.ConstructAlong(curve, esriSegmentExtension.esriNoExtension, 10, true);
            IPoint point1 = constructPoint as IPoint;

            constructPoint = new PointClass();
            constructPoint.ConstructAlong(curve, esriSegmentExtension.esriNoExtension, 2000, false);
            IPoint point2 = constructPoint as IPoint;

            drawMapShape(point1 as IGeometry);
            drawMapShape(point2 as IGeometry);
        }

        private void constructAngleBisectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IFeature feature = getSelectedFeature();
            IPointCollection pointCollection = feature.Shape as IPointCollection;
            MessageBox.Show(pointCollection.PointCount.ToString());
            IPoint point1 = new PointClass();
            IPoint point2 = new PointClass();
            IPoint point3 = new PointClass();
            pointCollection.QueryPoint(1, point1);
            pointCollection.QueryPoint(2, point2);
            pointCollection.QueryPoint(3, point3);

            IConstructPoint constructPoint = new PointClass();
            constructPoint.ConstructAngleBisector(point1, point2, point3, 10000, true);
            IPoint point4 = constructPoint as IPoint;

            drawMapShape(point1 as IGeometry);
            drawMapShape(point2 as IGeometry);
            drawMapShape(point3 as IGeometry);
            drawMapShape(point4 as IGeometry);
        }
        private IFeature getSelectedFeature()
        {
            IFeatureLayer featureLayer = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
            if (axMapControl1.Map.SelectionCount == 0)
            {
                return null;
            }
            IFeatureSelection featureSelection = featureLayer as IFeatureSelection;
            if (featureSelection.SelectionSet.Count == 0)
            {
                return null;
            }
            ICursor cursor;
            featureSelection.SelectionSet.Search(null, false, out cursor);
            IFeature feature = (cursor as IFeatureCursor).NextFeature();
            return feature;
        }

        private void 获取我的文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ArcGIS";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string ini = path + @"\addin.ini";
                //OpenOrCreate 
                System.IO.FileStream fsWrite = new System.IO.FileStream(ini, FileMode.Create, FileAccess.Write);
                byte[] bteData = System.Text.Encoding.UTF8.GetBytes("30");
                fsWrite.Write(bteData, 0, bteData.Length);
                fsWrite.Flush();
                fsWrite.Dispose();
                MessageBox.Show(ini);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void iniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ini = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ArcGIS\addin.ini";
            //byte[] b = File.ReadAllBytes(ini);
            //foreach (byte temp in b)
            //{
            //    Console.Write((char)temp + " ");
            //}
            string[] str = File.ReadAllLines(ini);
            Console.WriteLine(str[0]);
            /*
            System.IO.FileStream fs = new System.IO.FileStream(ini, FileMode.OpenOrCreate, FileAccess.Read);
             StreamReader sr =new StreamReader(fs);
             */
            string str2 = File.ReadAllText(ini).Trim();
            Console.WriteLine(str2);
        }
    }
}








































































































































































































































