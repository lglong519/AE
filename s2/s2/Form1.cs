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

namespace s2
{
    public partial class Form1 : Form
    {
        // mxd 文件
        IMapDocument mapDocument;
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
                mapDocument.Open(filePath,"");
                if (axMapControl1.CheckMxFile(filePath))
                {
                    axMapControl1.MousePointer = esriControlsMousePointer.esriPointerHourglass;
                    axMapControl1.LoadMxFile(filePath, 0, Type.Missing);
                    axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
                    //loadEagleEyeDocument(filePath);
                    axMapControl1.Extent = axMapControl1.FullExtent;
                }
                else
                {
                    MessageBox.Show("INVALID_FILE_PATH: " + filePath);
                }
            }
            else
            {
                mapDocument=null;
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
                            //loadEagleEyeDocument(filePath);
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
                mapDocument=null;
            }
        }
        // 保存
        private void saveMapDocument()
        {
            if(mapDocument==null)
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
                        mapDocument.Save(mapDocument.UsesRelativePaths,true);
                        MessageBox.Show("成功保存至：" + mapDocument.DocumentFilename);
                    }
                    catch(Exception e)
                    {
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
                string map=mapDocument.DocumentFilename;
                int lastIndex = map.LastIndexOf("\\");
                if ( lastIndex> -1)
                {
                    string filename = map.Substring(lastIndex+1, map.Length - 2 - lastIndex);
                    string fullname = dialog.SelectedPath + "\\" + filename;
                    // 创建 mxd
                    // 另存 mxd
                    //mapDocument.SaveAs();
                    MessageBox.Show("SaveAs:" + fullname);
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

        }
        private void 移动ToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }
        // 添加图层文件
        private void addShapeFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Title = "打开图层文件", Filter = "map documents(*.shp)|*.shp" };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                
            }
        }
        #endregion
        #region 3.图形绘制
        #endregion
        #region 4.空间查询
        #endregion
    }
}
