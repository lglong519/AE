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
using ESRI.ArcGIS.esriSystem;

namespace DsjToolbar
{
    public partial class FrmFixMxd : Form
    {
        public FrmFixMxd()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            ToolTip comboBox1Tip = new ToolTip();
            comboBox1Tip.SetToolTip(comboBox1, "选择或输入 mxd 原来的所在的路径");
            ToolTip button1Tip = new ToolTip();
            button1Tip.SetToolTip(button1, "选择或输入 mxd 原来的所在的路径");
            getOldPaths();
        }
        private ESRI.ArcGIS.Framework.IApplication application
        {
            get
            {
                return null;
            }
        }
        private IMap map
        {
            get
            {
                return null;
            }
        }
        private void getOldPaths()
        {
            if (map == null) return;
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
                            string[] sArray = pathName.Split('\\');
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
                                comboBox1.Items.Clear();
                                comboBox1.Items.AddRange(paths);
                            }
                            return;
                        }
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (application.Templates.Count < 1) return;
                string docFullPath = application.Templates.get_Item(application.Templates.Count - 1);
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
                        IDataLayer2 pDataLayer = featureLayer as IDataLayer2;
                        if (featureLayer.FeatureClass == null)
                        {
                            IDatasetName datasetName = pDataLayer.DataSourceName as IDatasetName;
                            string copyPath = datasetName.WorkspaceName.PathName;
                            datasetName.WorkspaceName.PathName = datasetName.WorkspaceName.PathName.Replace(comboBox1.Text, path);
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
