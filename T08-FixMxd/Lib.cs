using System;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Editor;
using ESRI.ArcGIS.Geodatabase;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DsjToolbar
{
    // For Arcgis Engine
    class ArcMap
    {
        public static ESRI.ArcGIS.Framework.IApplication Application
        {
            get
            {
                return null;
            }
        }
        public static ESRI.ArcGIS.ArcMapUI.IMxDocument Document;
    }
    class App
    {
        public static void log(params object[] strs)
        {
            ArcMap.Application.StatusBar.set_Message(0, stringParser(strs));
        }
        public static void logDate(params object[] strs)
        {
            ArcMap.Application.StatusBar.set_Message(0, String.Format("[{0}]{1}", LocalTime.fullDate, stringParser(strs)));
        }
        public static void logIO(params object[] strs)
        {
            try
            {
                string str = stringParser(strs).Trim(); ;
                if (str == "") return;
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ArcGIS";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string logFullPath = path + @"\DsjTools.log";
                FileStream fsWrite = new FileStream(logFullPath, FileMode.Append, FileAccess.Write);
                if (!fsWrite.CanWrite) return;
                byte[] bteData = Encoding.UTF8.GetBytes(string.Format("[{0}]{1}{2}", LocalTime.fullDate, str, "\n"));
                fsWrite.Write(bteData, 0, bteData.Length);
                fsWrite.Flush();
                fsWrite.Dispose();
            }
            catch (Exception e)
            {
                App.logDate(e.Message);
            }
        }
        // 传入的参数转换格式为:str + \t + str + \t
        static string stringParser(params object[] strs)
        {
            string str = "";
            foreach (var item in strs)
            {
                if (str == "")
                {
                    str += item.ToString();
                    continue;
                }
                str += "\t" + item.ToString();
            }
            return str;
        }
    }
    class LocalTime
    {
        public static string fullDate
        {
            get
            {
                return DateTime.Now.ToString().Replace("/", "-");
            }
        }
    }
    class IniFile
    {
        string path = "";
        public IniFile(string iniFile)
        {
            path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ArcGIS\";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += (iniFile.EndsWith(".ini") ? iniFile : iniFile + ".ini");
        }
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        public void set(string key, object value, string section = "default")
        {
            if (section == "")
            {
                section = "default";
            }
            long res = WritePrivateProfileString(section, key, value.ToString(), path);
            if (res == 0)
            {
                App.logDate("ini写入失败");
            }
        }
        public string get(string key, string section = "default")
        {
            if (section == "")
            {
                section = "default";
            }
            StringBuilder temp = new StringBuilder(10);
            long res = GetPrivateProfileString(section, key, "", temp, 10, path);
            if (res == 0)
            {
                App.logDate("ini读取失败");
            }
            return temp.ToString();
        }
    }
    class Progress
    {
        ProgressBar progressBar;
        private double progressValue = 0;
        public Progress(System.Windows.Forms.ProgressBar progressBar)
        {
            this.progressBar = progressBar;
        }
        public void start()
        {
            progressValue = 0;
            if (progressBar == null) return;
            progressBar.Visible = true;
        }
        public void calc(int count)
        {
            if (progressBar == null) return;
            progressBar.Value = (int)Math.Ceiling(++progressValue / count * 100.0);
        }
        public void calc(double count)
        {
            if (progressBar == null) return;
            progressBar.Value = (int)Math.Ceiling(++progressValue / count * 100.0);
        }
        public void stop()
        {
            progressValue = 0;
            if (progressBar == null) return;
            progressBar.Visible = false;
        }
    }
    enum CheckMode
    {
        silent = 0,
        alert = 1,
    };
    class Common
    {
        public static bool testArcMap
        {
            get
            {
                try
                {
                    return ArcMap.Document != null && ArcMap.Document.FocusMap != null;
                }
                catch (Exception ex)
                {
                    App.logIO(ex.ToString());
                    return false;
                }
            }
        }
        /// <summary>
        /// 判断图层是否为空,如果为空弹窗提示
        /// </summary>
        /// <param name="mode">检查模式:[alert]弹窗,silent不弹窗</param>
        /// <returns></returns>
        public static bool testLayerCount(CheckMode mode = CheckMode.alert)
        {
            if (testArcMap && ArcMap.Document.FocusMap.LayerCount > 0)
            {
                return true;
            }
            if (mode == CheckMode.alert) MessageBox.Show("图层为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        //判断图层是否有效
        public static bool testFeatureClass(int index)
        {
            if (!Common.testLayerCount(CheckMode.silent)) return false;
            IFeatureLayer featureLayer = ArcMap.Document.FocusMap.get_Layer(index) as IFeatureLayer;
            if (featureLayer == null) return false;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            if (featureClass == null) return false;
            return true;
        }
        // 判断字段是否存在
        public static bool testFields(IFeatureClass featureClass, string[] fields, CheckMode mode = CheckMode.alert)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (featureClass.FindField(fields[i]) < 0)
                {
                    if (mode == CheckMode.alert) MessageBox.Show(String.Format("缺少字段：[{0}]", fields[i]));
                    return false;
                }
            }
            return true;
        }
    }
}
