using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using DsjToolbar;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;

namespace T08_FixMxd
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            new System.Threading.Timer(updateComboxLayers, null, 0, 1000);
            //comboBox1.Items.Add("123");
        }
        private IMap map
        {
            get
            {
                return axMapControl1.Map;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

            int layerCount = map.LayerCount;
            for (int i = 0; i < layerCount; i++)
            {
                if (layerCount != map.LayerCount) return;
                ILayer layer = map.get_Layer(i);
                if (layer != null)
                {
                    IFeatureLayer featureLayer = layer as IFeatureLayer;
                    IDataLayer2 pDataLayer = featureLayer as IDataLayer2;
                    if (featureLayer.FeatureClass == null)
                    {
                        IDatasetName datasetName = pDataLayer.DataSourceName as IDatasetName;
                        string pathName = datasetName.WorkspaceName.PathName;
                        if (pathName != "")
                        {
                            //string[] paths = pathName.Split(new char[] { "\\" });
                            string[] sArray = pathName.Split('\\');
                            Console.WriteLine(pathName);
                            Console.WriteLine(sArray);
                            Console.WriteLine(sArray.Length);
                            string[] paths = new string[sArray.Length];
                            string path = "";
                            for (int n = 0; n < sArray.Length; n++)
                            {
                                if (n == 0)
                                {
                                    path = sArray[n];
                                    paths[n] = path + "\\";
                                }
                                else
                                {
                                    path += "\\" + sArray[n];
                                    paths[n] = path;
                                }
                            }
                            if (paths.Length != 0)
                            {
                                comboBox2.Items.Clear();
                                comboBox2.Items.AddRange(paths);
                            }
                            return;
                        }
                    }
                    else
                    {
                        //pDataLayer.Disconnect();
                        //获取原始名称  
                        IDatasetName pDN = pDataLayer.DataSourceName as IDatasetName;
                        //Console.WriteLine("start");
                        //Console.WriteLine(pDN.Category);
                        //Console.WriteLine(pDN.Name);
                        //Console.WriteLine(pDN.SubsetNames);
                        //Console.WriteLine(pDN.Type);
                        //Console.WriteLine(pDN.WorkspaceName.BrowseName);
                        //Console.WriteLine(pDN.WorkspaceName.Category);
                        //Console.WriteLine(pDN.WorkspaceName.ConnectionProperties.Count);
                        //Console.WriteLine(pDN.WorkspaceName.PathName);
                        //Console.WriteLine(pDN.WorkspaceName.Type);
                        //Console.WriteLine(pDN.WorkspaceName.WorkspaceFactory.get_WorkspaceDescription(true));
                        //Console.WriteLine(pDN.WorkspaceName.WorkspaceFactory.get_WorkspaceDescription(false));
                        //Console.WriteLine(pDN.WorkspaceName.WorkspaceFactory.WorkspaceType);
                        //Console.WriteLine(pDN.WorkspaceName.WorkspaceFactoryProgID);
                        //return;
                    }
                }
            }
        }
        int layerCount = 0;
        //更新下拉图层列表,清空属性列表
        private void updateComboxLayers(object state)
        {
            try
            {
                layerCount = map.LayerCount;
                if (layerCount == 0)
                {
                    if (comboBox1.Items == null) return;
                    comboBox1.Items.Clear();
                    return;
                }
                string[] layers = new string[layerCount];
                bool updateFlag = false;
                for (int i = 0; i < layerCount; i++)
                {
                    ILayer layer = map.get_Layer(i);
                    if (layer != null)
                    {
                        layers[i] = layer.Name;
                        if (comboBox1.Items.IndexOf(layer.Name) != i)
                        {
                            updateFlag = true;
                        }
                    }
                }
                if (!updateFlag) return;
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(layers);
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception e)
            {
                App.logIO(e.ToString());
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //FrmFixMxd frmFixMxd = new FrmFixMxd();
            //frmFixMxd.Show();
            //frmFixMxd.Focus();
            try
            {
                Console.WriteLine(axMapControl1.DocumentFilename);
                string docFullPath = axMapControl1.DocumentFilename;
                int index = docFullPath.LastIndexOf("\\");
                string path = docFullPath.Substring(0, index);
                int success = 0;
                int layerCount = map.LayerCount;
                for (int i = 0; i < layerCount; i++)
                {
                    if (layerCount != map.LayerCount) return;
                    ILayer layer = map.get_Layer(i);
                    if (layer != null)
                    {
                        IFeatureLayer featureLayer = layer as IFeatureLayer;
                        if (featureLayer == null) continue;
                        IDataLayer2 pDataLayer = featureLayer as IDataLayer2;
                        IDatasetName datasetName = pDataLayer.DataSourceName as IDatasetName;
                        if (featureLayer.FeatureClass == null)
                        {
                            string copyPath = datasetName.WorkspaceName.PathName;
                            datasetName.WorkspaceName.PathName = datasetName.WorkspaceName.PathName.Replace(comboBox2.Text, path);
                            try
                            {
                                pDataLayer.Connect(datasetName as IName);
                                success++;
                            }
                            catch (Exception ex)
                            {
                                datasetName.WorkspaceName.PathName = copyPath;
                                Console.WriteLine("Connect Err:" + ex.ToString());
                            }
                        }
                    }
                }
                MessageBox.Show("修复数量：" + success);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
