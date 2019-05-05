using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace T10_SpatialQuery
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IFeatureLayer targetFeatureLayer = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
            IFeatureClass targetFeatureClass = targetFeatureLayer.FeatureClass;
            //source
            IFeatureLayer sourceFeatureLayer = axMapControl1.Map.get_Layer(1) as IFeatureLayer;
            IFeatureClass sourceFeatureClass = sourceFeatureLayer.FeatureClass;
            IFeatureCursor sourceFeatureCursor = sourceFeatureClass.Search(null, false);
            IFeature sourceFeature = sourceFeatureCursor.NextFeature();
            while (sourceFeature != null)
            {
                //参考图 查询过滤器
                ISpatialFilter spatialFilter = new SpatialFilterClass()
                {
                    Geometry = sourceFeature.Shape as IGeometry,
                    SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects,   //相交
                    GeometryField = sourceFeatureClass.ShapeFieldName
                };
                if (targetFeatureClass.FeatureCount(spatialFilter)<1)
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
                //MessageBox.Show("面积比：" + intersectionArea.Area / sourceArea.Area);
                if (intersectionArea.Area / sourceArea.Area>1)
                {
                    Console.WriteLine("面积比");
                    Console.WriteLine(intersectionArea.Area / sourceArea.Area);
                    Console.WriteLine(sourceFeature.OID);
                }
                sourceFeature = sourceFeatureCursor.NextFeature();
            }
        }
    }
}
