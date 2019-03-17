using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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

namespace s3Geometry
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
        }
        private void addFeature(string layerName, IGeometry geometry)
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
            IFeatureCursor featureCursor = featureClass.Search(null, false);
            IFeature feature = featureCursor.NextFeature();
            while (feature != null)
            {
                feature.Delete();
                feature = featureCursor.NextFeature();
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
            workspaceEdit.StartEditing(true);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
        }
        #region AddGeometry
        private void addGeometryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IGeometryCollection geometryCollection = new MultipointClass();
            IMultipoint multipoint;
            object missing = Type.Missing;
            IPoint point;
            for (int i = 0; i < 10; i++)
            {
                point = new PointClass();
                point.PutCoords(i * 2, i * 2);

                geometryCollection.AddGeometry(point as IGeometry, ref missing, ref missing);
            }
            multipoint = geometryCollection as IMultipoint;
            addFeature("multi_point", multipoint as IGeometry);
            axMapControl1.Extent = multipoint.Envelope;
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
                point.PutCoords(i * 2, i * 2);

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
                point.PutCoords(i, i * 2);
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
                point.PutCoords(i * 2, i * 2);
                geometryArray[i] = point as IGeometry;
            }
            geometryBridge.SetGeometries(geometryCollection1, ref geometryArray);

            multipoint = geometryCollection1 as IMultipoint;
            addFeature("multi_point", multipoint as IGeometry);
            axMapControl1.Extent = multipoint.Envelope;
            axMapControl1.Refresh();
        }
        #endregion
        #region ISegmentCollection
        // add
        private void addSegmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ISegment[] segmentArray = new ISegment[10];
            IPolyline polyline = new PolylineClass();
            ISegmentCollection segmentCollection = new PolylineClass();
            for (int i = 0; i < 10; i++)
            {
                ILine line = new LineClass();
                IPoint fromPoint = new PointClass();
                fromPoint.PutCoords(i * 10, i * 10);
                IPoint toPoint = new PointClass();
                toPoint.PutCoords(i * 15000, i * 15000);
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
            ISpatialReferenceDialog dialog=new SpatialReferenceDialogClass();
            IProjectedCoordinateSystem projectCoordSystem = dialog.DoModalCreate(true, false, false, 0) as IProjectedCoordinateSystem;
            axMapControl1.Map.SpatialReference = projectCoordSystem;
            axMapControl1.ActiveView.Refresh();
        }
    }
}








































































































































































































































