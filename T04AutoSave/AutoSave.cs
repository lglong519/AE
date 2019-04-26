using System;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Editor;
using ESRI.ArcGIS.Geodatabase;
using System.Runtime.InteropServices;

namespace T04AutoSave
{
    public class AutoSave : ESRI.ArcGIS.Desktop.AddIns.ComboBox
    {
        private System.Threading.Timer timer;
        public AutoSave()
        {
            IniFile ini=new IniFile("cookie");
            for (int i = 1; i < 31; i++)
            {
                Add(i.ToString());
            }
            Add("45");
            Add("60");
            Enabled = true;
            iniCookies();
            startTimer();
        }
        //1.获取cookie
        //2.如果cookie不存在，设置默认cookie为items[4]的cookie
        //3.使用cookie设置选中项
        private void iniCookies()
        {
            IniFile ini = new IniFile("cookie");
            string cookieStr = ini.get("cookie");
            int defaultCookie = items[4].Cookie;
            if (cookieStr == "")
            {
                ini.set("cookie", defaultCookie);
                // 非鼠标事件不会触发 OnSelChange,Value 为空
                Select(defaultCookie);
            }
            else
            {
                int cookie;
                int.TryParse(cookieStr, out cookie);
                if (GetItem(cookie) == null)
                {
                    ini.set("cookie", defaultCookie);
                    cookie = defaultCookie;
                }
                Select(cookie);
            }
        }
        //手动选择保存时间时：更新 cookie、minutes、重启定时器
        protected override void OnSelChange(int cookie)
        {
            IniFile ini = new IniFile("cookie");
            //updateCookie
            ini.set("cookie", cookie);
            ini.set("minutes", Value);
            startTimer();
        }
        //启动定时保存
        public void startTimer()
        {
            IniFile ini = new IniFile("cookie");
            uint minutes;
            uint.TryParse(ini.get("minutes"), out minutes);
            if (minutes < 1)
            {
                minutes = 5;
                ini.set("minutes", minutes);
            }
            App.log("自动保存时间：" + minutes + "分钟");
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
            timer = new System.Threading.Timer(state =>
            {
                if (ini.get("active") == "0")
                {
                    App.log(DateTime.Now, "自动保存未启动");
                    return;
                }
                try
                {
                    IEditor editor = ArcMap.Application.FindExtensionByName("ESRI Object Editor") as IEditor;
                    if (editor == null) return;
                    IWorkspaceEdit workspaceEdit = editor.EditWorkspace as IWorkspaceEdit;
                    if (workspaceEdit == null) return;
                    workspaceEdit.StopEditing(true);
                    workspaceEdit.StartEditing(true);
                    App.log(DateTime.Now, "保存成功");
                }
                catch (Exception e)
                {
                    App.log(e.Message);
                }
            }, null, minutes * 60000, minutes * 60000);
        }
    }
    class App
    {
        public static void log(params object[] strs)
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
            ArcMap.Application.StatusBar.set_Message(0, str);
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
                App.log("ini写入失败");
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
                App.log("ini读取失败");
            }
            return temp.ToString();
        }
    }
}
