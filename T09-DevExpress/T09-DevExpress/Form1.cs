using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Helpers;


namespace T09_DevExpress
{
    public partial class Form1 : RibbonForm
    {
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
            InitSkinGallery();
        }
        void InitSkinGallery()
        {
            //SkinHelper.InitSkinGallery(rgbiSkins, true);
            ribbonControl.SelectedPageChanged += new EventHandler((object sender, EventArgs e) => {
                Console.WriteLine(ribbonControl.SelectedPage.Text);
                Console.WriteLine(ribbonControl.SelectedPage.Name);
                Console.WriteLine(ribbonControl.SelectedPage.PageIndex);
                Console.WriteLine(ribbonControl.SelectedPage.Visible);
            });
        }

        private void iOpen_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "打开地图文档文件";
            openFileDialog.Filter = "map documents(*.mxd)|*.mxd";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                axMapControl1.LoadMxFile(filePath, 0, Type.Missing);
                axMapControl1.Extent = axMapControl1.FullExtent;
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            Console.WriteLine(xtraTabControl1.TabIndex.ToString());
            Console.WriteLine(xtraTabControl1.SelectedTabPageIndex.ToString());
        }
    }
}