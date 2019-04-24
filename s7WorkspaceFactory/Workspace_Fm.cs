using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;

namespace s7WorkspaceFactory
{
    public partial class Workspace_Fm : Form
    {
        public Workspace_Fm()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
            movePointFeedback = new MovePointFeedbackClass();
            comboBox1.TextChanged += new EventHandler(comboBox1_TextChanged);
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            WorkspaceEdit();
        }
        private void WorkspaceEdit()
        {
            // 1.启动一个工作空间工厂
            IWorkspaceFactory workspaceFactory = new AccessWorkspaceFactoryClass();
            // 2.用工作空间工厂打开数据库、SDE数据库、SHP文件的路径
            // 返回的是工作空间(IWorkspace)的继承接口(IFeatureWorkspace)的实例，也继承了 IWorkspaceEdit
            IFeatureWorkspace featureWorkspace = workspaceFactory.OpenFromFile(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\data\test\test.mdb", 0) as IFeatureWorkspace;
            // 3.用返回的工作空间打开指定 名称 的 SHP 文件、图层
            IFeatureClass featureClass = featureWorkspace.OpenFeatureClass("tl");
            IWorkspaceEdit workspaceEdit = featureWorkspace as IWorkspaceEdit;
            bool hasEdits = true;
            // 4.开启编辑
            workspaceEdit.StartEditing(true);
            // ？不明
            workspaceEdit.RedoEditOperation();
            //for (int i = featureClass.FeatureCount(null); i > 0; i--)
            IFeature feature;
            for (int i = 80000; i < 240000; i++)
            {
                try
                {
                    // 5.根据 ID 获取要素，并删除
                    feature = featureClass.GetFeature(i);
                    feature.Delete();
                }
                catch (Exception e) { }
            }
            // 6.完成编辑
            workspaceEdit.StopEditOperation();
            DialogResult dialogResult;
            //dialogResult = MessageBox.Show("后退一步？", "undo", MessageBoxButtons.YesNo);
            //if (dialogResult == DialogResult.Yes)
            //{
            //    workspaceEdit.UndoEditOperation();
            //}
            //workspaceEdit.HasEdits(ref hasEdits);
            //if (hasEdits)
            //{
            dialogResult = MessageBox.Show("保存编辑？", "保存", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                // 7.结束编辑，保存编辑结果
                workspaceEdit.StopEditing(true);
                MessageBox.Show("保存");
            }
            else
            {
                // 8.结束编辑，不保存编辑结果
                workspaceEdit.StopEditing(false);
                MessageBox.Show("取消");
            }
            //}
            MessageBox.Show(featureClass.FeatureCount(null).ToString());
        }
        #region 图形编辑
        // 操作类型
        string strOperator = "";
        // 当前视图
        IActiveView activeView = null;
        // 当前操作图层
        IFeatureLayer featureLayer = null;
        // 当前操作实体
        IFeature feature = null;
        // 当前点移动反馈对象
        IMovePointFeedback movePointFeedback;
        // 当前线移动反馈对象
        IMoveLineFeedback moveLineFeedback;// = new MoveLineFeedbackClass();
        // 当前面移动反馈对象
        IMovePolygonFeedback movePolygonFeedback;// = new MovePolygonFeedbackClass();

        // move
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            strOperator = "move";
            movePointFeedback = new MovePointFeedbackClass();
            moveLineFeedback = new MoveLineFeedbackClass();
            movePolygonFeedback = new MovePolygonFeedbackClass();
            IPoint point=new PointClass();
            point.PutCoords(0,0);
            movePointFeedback.Display = activeView.ScreenDisplay;
            //feature = featureLayer.FeatureClass.GetFeature(0);
            movePointFeedback.Start(point, point);
            movePointFeedback.MoveTo(point);
            movePointFeedback.Stop();
            activeView.Refresh();
        }
        // load mxd
        private void simpleButton3_Click(object sender, EventArgs e)
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
            if (axMapControl1.LayerCount > 0)
            {
                string[] items = new string[axMapControl1.LayerCount];
                for (int i = 0; i < axMapControl1.LayerCount; i++)
                {
                    items[i] = axMapControl1.get_Layer(i).Name;
                }
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(items);
                comboBox1.SelectedIndex = items.Length - 1;
            }
        }
        //选择图层，选中图层 ID=0 的实体
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                for (int i = 0; i < this.axMapControl1.LayerCount; i++)
                {
                    ILayer layer = this.axMapControl1.get_Layer(i);
                    if (layer.Name == this.comboBox1.Text.ToString())
                    {
                        featureLayer = layer as IFeatureLayer;
                        feature = featureLayer.FeatureClass.GetFeature(0);

                        if (feature != null)
                        {
                            this.axMapControl1.Map.ClearSelection();
                            this.axMapControl1.Map.SelectFeature(featureLayer, feature);
                            this.axMapControl1.Refresh();
                        }
                        activeView = this.axMapControl1.ActiveView;
                        return;
                    }
                }
            }
        }
        #endregion

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (feature == null)
            {
                return;
            }
            IPointCollection pointCollection;
            if (feature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
            {
                pointCollection = new PolylineClass();
                IPolyline polyline = feature.Shape as IPolyline;
                pointCollection = polyline as IPointCollection;
                if (pointCollection.PointCount > 2)
                {
                    pointCollection.RemovePoints(pointCollection.PointCount - 1, 1);
                }
            }
            if (feature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
            {
                pointCollection = new PolygonClass();
                IPolygon polygon = feature.Shape as IPolygon;
                pointCollection = polygon as IPointCollection;
                if (pointCollection.PointCount > 3)
                {
                    pointCollection.RemovePoints(pointCollection.PointCount - 1, 1);
                }
            }
            IWorkspaceEdit workspaceEdit;
            IWorkspace workspace;
            IDataset dataset = featureLayer.FeatureClass as IDataset;
            workspace = dataset.Workspace;
            workspaceEdit = workspace as IWorkspaceEdit;
            // edit
            workspaceEdit.StartEditing(true);
            workspaceEdit.StartEditOperation();
            feature.Store();
            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);
            activeView.Refresh();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {

        }
    }
}
