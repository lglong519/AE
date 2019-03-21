using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.GeoAnalyst;

namespace s8Raster
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
            SetDesktopLocation(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width / 2, System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - 300);
        }
        //8.2 访问栅格数据
        private void button1_Click(object sender, EventArgs e)
        {
            IWorkspaceFactory workspaceFactory = new RasterWorkspaceFactoryClass();
            string path = @"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\data\test";
            IRasterWorkspace2 rasterWorkspace = workspaceFactory.OpenFromFile(path, 0) as IRasterWorkspace2;
            string file = @"\test.bmp";
            IRasterDataset rasterDataset = rasterWorkspace.OpenRasterDataset(file);
            IRasterLayer rasterLayer = new RasterLayerClass();
            rasterLayer.CreateFromDataset(rasterDataset);
            axMapControl1.Map.AddLayer(rasterLayer as ILayer);
            axMapControl1.Extent = axMapControl1.FullExtent;
        }

        // 8.4 栅格数据处理
        // ToFeature
        private void button2_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\data\test";
            string file = @"\line.bmp";
            IConversionOp conversionOp = new RasterConversionOpClass();
            IWorkspaceFactory workspaceFactory = new RasterWorkspaceFactoryClass();
            IRasterWorkspace2 rasterWorkspace = workspaceFactory.OpenFromFile(path, 0) as IRasterWorkspace2;
            IRasterDataset rasterDataset = rasterWorkspace.OpenRasterDataset(file);
            IRasterLayer rasterLayer = new RasterLayerClass();
            IRaster raster = rasterLayer.Raster;

            // conversion
            IGeoDataset geoDataset;
            IWorkspaceFactory workspaceFactory2 = new RasterWorkspaceFactoryClass();
            string output=@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\data\test";
            IWorkspace workspace = workspaceFactory2.OpenFromFile(output, 0);
            string outFeatureClassName = "new.shp";
            //geoDataset = conversionOp.RasterDataToPointFeatureData(geoDataset, workspace, outFeatureClassName);
            MessageBox.Show("success");
        }
        // ToRaster
        private void button3_Click(object sender, EventArgs e)
        {
            ILayer layer = axMapControl1.get_Layer(0);
            MessageBox.Show(layer.Name);
            IFeatureLayer featureLayer = layer as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            IFeatureClassDescriptor featureClassDescriptor = new FeatureClassDescriptorClass();
            featureClassDescriptor.Create(featureClass,null,"OBJECTID");
            IGeoDataset geoDataset = featureClassDescriptor as IGeoDataset;

            string output = @"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\data\test";
            IWorkspaceFactory workspaceFactory = new RasterWorkspaceFactoryClass();
            IWorkspace workspace = workspaceFactory.OpenFromFile(output, 0);
            IConversionOp conversionOp = new RasterConversionOpClass();
            IRasterAnalysisEnvironment rasterAnalysisEnvironment = conversionOp as IRasterAnalysisEnvironment;
            double dCellSize = 0.0246668;
            object oCellSize = dCellSize;
            rasterAnalysisEnvironment.SetCellSize(esriRasterEnvSettingEnum.esriRasterEnvValue,ref oCellSize);
            // GRID
            // IMAGINE Image
            // TIFF
            IRasterDataset rasterDataset = conversionOp.ToRasterDataset(geoDataset, "IMAGINE Image", workspace, DateTime.Now.ToFileTime().ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadDocument();
        }
        public void LoadDocument()
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

                if (this.axMapControl1.CheckMxFile(filePath))
                {
                    this.axMapControl1.LoadMxFile(filePath, 0, Type.Missing);
                    this.axMapControl1.Extent = this.axMapControl1.FullExtent;
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
    }
}
