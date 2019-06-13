using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Interop;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Reflection;

using CADApplication = Autodesk.AutoCAD.ApplicationServices.Application;

[assembly: CommandClass(typeof(T16_CADClassLibrary.CommandClass))]
namespace T16_CADClassLibrary
{
    public class CommandClass : IExtensionApplication
    {
        public CommandClass()
        {
            //Control.CheckForIllegalCrossThreadCalls = false;
            try
            {
                RegisterDll();
            }
            catch (System.Exception ex)
            {
                App.logIO(ex.ToString());
            }
        }
        public void Initialize()
        {
            try
            {
                /*
                //修改cad标题
                Autodesk.AutoCAD.Windows.Window mainWindow = CADApplication.MainWindow;
                mainWindow.Text = "init1";
                //CADApplication.ShowAlertDialog("Alert");
                Document doc = CADApplication.DocumentManager.MdiActiveDocument;
                DocumentCollection docCollection = CADApplication.DocumentManager;
                //docCollection.Open(@"M:\Documents\任务\测试数据\smart3d\test.dwg");
                doc.Editor.WriteMessage("test.dwg 已加载0");
                Database db = doc.Database;
                LibCAD.createLayerByName("zz", db);
                //LibCAD.removeLayerByName("zz", db);
                //LibCAD.getCurrentViewport(db).GridEnabled = true;
                LibCAD.createLineByAxis(new double[3] { 0, 0, 0 }, new double[3] { 100, 100, 100 });
                */
                LibCAD.createPalette("DSJ工具箱");
            }
            catch (System.Exception ex)
            {
                App.logIO(ex.ToString());
            }
        }
        public void Terminate()
        {
            MessageBox.Show("init");
        }
        //调试必需
        [CommandMethod("RegisterDll")]
        private void RegisterDll()
        {
            //获取AutoCAD的Applications键
            //2012版使用RegistryProductRootKey属性
            //2014版使用MachineRegistryProductRootKey属性
            string sProdKey = HostApplicationServices.Current.RegistryProductRootKey;
            string sAppName = "T16-CADClassLibrary";
            RegistryKey regAcadProdKey =
            Registry.CurrentUser.OpenSubKey(sProdKey);
            RegistryKey regAcadAppKey = regAcadProdKey.OpenSubKey("Applications", true);
            //检查”TestCAD”键是否存在
            string[] subKeys = regAcadAppKey.GetSubKeyNames();
            foreach (string subKey in subKeys)
            {
                //如果应用程序已经注册，就退出
                if (subKey.Equals(sAppName))
                {
                    regAcadAppKey.Close();
                    return;
                }
            }
            //获取本模块的位置(注册本程序自己)
            string sAssemblyPath = Assembly.GetExecutingAssembly().Location;
            //注册应用程序
            RegistryKey regAppAddInKey = regAcadAppKey.CreateSubKey(sAppName);
            regAppAddInKey.SetValue("DESCRIPTION", sAppName, RegistryValueKind.String);
            regAppAddInKey.SetValue("LOADCTRLS", 14, RegistryValueKind.DWord);
            regAppAddInKey.SetValue("LOADER", sAssemblyPath, RegistryValueKind.String);
            regAppAddInKey.SetValue("MANAGED", 1, RegistryValueKind.DWord);
            regAcadAppKey.Close();
        }
    }
}
