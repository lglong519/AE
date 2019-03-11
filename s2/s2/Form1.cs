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

namespace s2
{
    public partial class Form1 : Form
    {
        // mxd 文件
        IMapDocument mapDocument;
        private int drawType = 0;
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
        }
        #region 1.地图加载
        private void 加载地图文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadMapDocument();
        }
        private void 加载特定地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadMapDocument2();
        }
        private void 保存地图文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveMapDocument();
        }

        private void 另存地图文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAsMapDocument();
        }
        // 加载地图文档
        private void loadMapDocument()
        {
            mapDocument = new ESRI.ArcGIS.Carto.MapDocumentClass();
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
                    loadEagleEyeDocument(filePath);
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
        // 加载特定地图
        private void loadMapDocument2()
        {
            mapDocument = new ESRI.ArcGIS.Carto.MapDocumentClass();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "打开地图文档";
            openFileDialog.Filter = "map documents(*.mxd)|*.mxd";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                mapDocument.Open(filePath, "");
                if (axMapControl1.CheckMxFile(filePath))
                {
                    // ReadMxMaps 获取 mxd 地图数组
                    IArray arrayMap = axMapControl1.ReadMxMaps(filePath, Type.Missing);
                    // 地图根元素
                    IMap map;
                    for (int i = 0; i < arrayMap.Count; i++)
                    {
                        map = arrayMap.get_Element(i) as IMap;
                        // 根元素名称：Layers|图层|其他自定义
                        if (map.Name == "Layers")
                        {
                            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerHourglass;
                            axMapControl1.LoadMxFile(filePath, 0, Type.Missing);
                            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
                            loadEagleEyeDocument(filePath);
                            axMapControl1.Extent = axMapControl1.FullExtent;
                            break;
                        }
                        else
                        {
                            MessageBox.Show("map.Name: " + map.Name);
                        }
                    }
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
        // 保存
        private void saveMapDocument()
        {
            if (mapDocument == null)
            {
                MessageBox.Show("mapDocument is empty");
            }
            else
            {
                // 判断文件是否只读
                if (mapDocument.get_IsReadOnly(mapDocument.DocumentFilename) == true)
                {
                    MessageBox.Show("mapDocument is read-only");
                }
                else
                {
                    try
                    {
                        // 保存文件，默认是使用相对路径
                        mapDocument.Save(mapDocument.UsesRelativePaths, true);
                        MessageBox.Show("成功保存至：" + mapDocument.DocumentFilename);
                    }
                    catch (Exception e)
                    {
                        if (e.ToString().Contains("共享冲突"))
                        {
                            MessageBox.Show("文件被占用,保存失败");
                            return;
                        }
                        MessageBox.Show(e.ToString());
                    }
                }
            }
        }
        // 另存
        private void saveAsMapDocument()
        {
            if (mapDocument == null)
            {
                MessageBox.Show("mapDocument is empty");
                return;
            }
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "选择保存的路径";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string map = mapDocument.DocumentFilename;
                int lastIndex = map.LastIndexOf("\\");
                if (lastIndex > -1)
                {
                    string filename = map.Substring(lastIndex + 1, map.Length - 2 - lastIndex);
                    string fullname = dialog.SelectedPath + "\\" + filename;
                    // 创建 mxd
                    // 另存 mxd
                    //mapDocument.SaveAs();
                    MessageBox.Show("SaveAs: " + fullname);
                }
            }
        }
        #endregion
        #region 2.图层管理
        private void 添加SHP文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addShapeFile();
        }
        private void 添加图层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addLayerFile();
        }
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteLayer();
        }
        private void 移动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            moveLayer();
        }
        // 添加图层文件
        private void addLayerFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Title = "打开图层文件", Filter = "map documents(*.lyr)|*.lyr" };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    axMapControl1.AddLayerFromFile(filePath);
                    axMapControl2.AddLayerFromFile(filePath);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }
        // 添加shp文件
        private void addShapeFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Title = "打开图层文件", Filter = "map documents(*.shp)|*.shp" };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // 获取路径+文件名
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                string path = fileInfo.Directory.ToString();
                string fileName = fileInfo.Name.Substring(0, fileInfo.Name.IndexOf("."));
                try
                {
                    // 添加 shp
                    axMapControl1.AddShapeFile(path, fileName);
                    axMapControl2.AddShapeFile(path, fileName);
                }
                catch (Exception e)
                {
                    MessageBox.Show("添加shp失败");
                }
            }
        }
        // 删除图层
        private void deleteLayer()
        {
            MessageBox.Show(axMapControl1.LayerCount.ToString());
            if (axMapControl1.LayerCount > 0)
            {
                axMapControl1.DeleteLayer(0);
            }
        }
        // 移动图层叠加顺序
        private void moveLayer()
        {
            if (axMapControl1.LayerCount > 1)
            {
                axMapControl1.MoveLayerTo(axMapControl1.LayerCount - 1, 0);
                //axMapControl1.Refresh();
            }
        }
        #endregion
        #region 3.图形绘制
        private void 线条ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawType = 1;
        }

        private void 矩形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawType = 2;
        }

        private void 文本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawType = 3;
        }

        private void 圆形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawType = 4;
        }
        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            IGeometry geometry = null;
            if (drawType != 0)
            {
                switch (drawType)
                {
                    case 1:
                        geometry = this.axMapControl1.TrackLine();
                        break;
                    case 2:
                        geometry = this.axMapControl1.TrackRectangle();
                        break;
                    case 3:
                        IPoint point = new PointClass();
                        point.X = e.mapX;
                        point.Y = e.mapY;
                        geometry = point as IGeometry;
                        break;
                    case 4:
                        geometry = this.axMapControl1.TrackCircle();
                        break;
                    default:
                        drawType = 0;
                        break;
                }
                if (drawType == 1 || drawType == 2 || drawType == 4)
                {
                    drawMapShape(geometry);
                }
                if (drawType == 3)
                {
                    drawMapText(geometry);
                }
                return;
            }
            // 查询
        }
        // 画图形
        private void drawMapShape(IGeometry geometry)
        {
            // 定义图形样式：颜色，线框大小
            IRgbColor rgbColor = new RgbColorClass();
            rgbColor.Red = 255;
            rgbColor.Green = 255;
            rgbColor.Blue = 0;
            object symbol = null;
            if (geometry.GeometryType == esriGeometryType.esriGeometryPolyline || geometry.GeometryType == esriGeometryType.esriGeometryLine)
            {
                ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
                simpleLineSymbol.Color = rgbColor;
                simpleLineSymbol.Width = 5;
                symbol = simpleLineSymbol;
            }
            else
            {
                ISimpleFillSymbol simpleFillSymbol = new SimpleFillSymbolClass();
                simpleFillSymbol.Color = rgbColor;
                symbol = simpleFillSymbol;
            }
            axMapControl1.DrawShape(geometry, ref symbol);
        }
        // 画文本
        private void drawMapText(IGeometry geometry)
        {
            IRgbColor rgbColor = new RgbColorClass();
            rgbColor.Red = 255;
            rgbColor.Green = 0;
            rgbColor.Blue = 0;
            ITextSymbol textSymbol = new TextSymbolClass();
            textSymbol.Color = rgbColor;
            object symbol = textSymbol;
            axMapControl1.DrawText(geometry, "test text", ref symbol);
        }
        #endregion
        #region 4.空间查询
        private void 名称查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.textBox1.Visible = true;
            this.label1.Visible = true;
            // 报错
            //searchByName();
        }
        private void searchByName()
        {
            string searchName = this.textBox1.Text.Trim();
            MessageBox.Show(searchName);
            if (searchName != null && searchName.Length > 1)
            {
                ILayer layer = axMapControl1.Map.get_Layer(0);
                IFeatureLayer featureLayer = layer as IFeatureLayer;
                IFeatureClass featureClass = featureLayer.FeatureClass;
                IQueryFilter queryFilter = new QueryFilterClass();
                IFeatureCursor featureCursor;
                IFeature feature = null;
                queryFilter.WhereClause = "continent like '%" + searchName + "%'";
                //queryFilter.WhereClause = string.Format("continent like '%{0}%'", searchName);
                try
                {
                    featureCursor = featureClass.Search(queryFilter, true);
                    feature = featureCursor.NextFeature();
                    if (feature != null)
                    {
                        axMapControl1.Map.SelectFeature(axMapControl1.get_Layer(0), feature);
                        axMapControl1.Refresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }
        #endregion
        #region 鹰眼图
        //1.axMapControl2也加载图层，2.设置2的比例，以1的中心为中心，3.监听1的更新同时更新2
        // 重置样式
        private void setLoadEagle()
        {
            axMapControl2.MapScale = this.axMapControl1.MapScale * 2.0;
            IPoint centerPoint = new PointClass();
            centerPoint.X = (axMapControl1.Extent.XMax + axMapControl1.Extent.XMin) / 2;
            centerPoint.Y = (axMapControl1.Extent.YMax + axMapControl1.Extent.YMin) / 2;
            // 居中
            axMapControl2.CenterAt(centerPoint);
            axMapControl2.Refresh();
        }
        // event handler
        private void axMapControl1_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            setLoadEagle();
        }
        // axMapControl2 加载地图，重置样式
        private void loadEagleEyeDocument(string filePath)
        {
            if (axMapControl2.CheckMxFile(filePath))
            {
                axMapControl2.MousePointer = esriControlsMousePointer.esriPointerHourglass;
                axMapControl2.LoadMxFile(filePath, 0, Type.Missing);
                axMapControl2.MousePointer = esriControlsMousePointer.esriPointerDefault;
                setLoadEagle();
            }
            else
            {
                MessageBox.Show("INVALID_FILE_PATH: " + filePath);
            }
        }
        #endregion
        #region IObjectCopy
        private void copyToPageLayout()
        {
            // systerm
            IObjectCopy objectCopy = new ObjectCopyClass();
            // 来源
            object copyFromMap = axMapControl1.Map;
            // 复制
            object copyMap = objectCopy.Copy(copyFromMap);
            // 目标
            object copyToMap = axPageLayoutControl1.ActiveView.FocusMap;
            // 复制的地图 写入 布局控件(缩略图)
            objectCopy.Overwrite(copyMap,ref copyToMap);
        }
        // 图层加载结束事件
        private void axMapControl1_OnAfterScreenDraw(object sender, IMapControlEvents2_OnAfterScreenDrawEvent e)
        {
            //MessageBox.Show("axMapControl1_OnAfterScreenDraw");
            IActiveView activeView = (IActiveView)axPageLayoutControl1.ActiveView.FocusMap;
            // ?
            IDisplayTransformation displayTramsformation = activeView.ScreenDisplay.DisplayTransformation;
            displayTramsformation.VisibleBounds = axPageLayoutControl1.Extent;
            axPageLayoutControl1.ActiveView.Refresh();
            copyToPageLayout();
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {

        }

    }
}
