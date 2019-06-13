using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Win32;
using System.Reflection;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

namespace T17_RegisterCADDll
{
    class Program
    {
        //按需加载.NET应用程序
        [CommandMethod("RegisterTestCAD")]
        static void Main(string[] args)
        {
            //获取AutoCAD的Applications键
            //2012版使用RegistryProductRootKey属性
            //2014版使用MachineRegistryProductRootKey属性
            string sProdKey = HostApplicationServices.Current.RegistryProductRootKey;
            string sAppName = "TestCAD";
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
