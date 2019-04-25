using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using ESRI.ArcGIS.Editor;
using ESRI.ArcGIS.Geodatabase;

namespace T04AutoSave
{
    public class AutoSave : ESRI.ArcGIS.Desktop.AddIns.ComboBox
    {
        private System.Threading.Timer timer;
        public AutoSave()
        {
            for (int i = 1; i < 31; i++)
            {
                Add(i.ToString());
            }
            Add("45");
            Add("60");
            Enabled = true;
            Select(35502);
            updateSaveTime(null);
        }
        protected override void OnUpdate()
        {
        }

        protected override void OnSelChange(int cookie)
        {
            uint minutes;
            uint.TryParse(Value, out minutes);
            recCookie(cookie);
            updateSaveTime(minutes);
        }
        //启动定时保存
        private void startTimer(uint minutes)
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
            timer = new System.Threading.Timer(state =>
            {
                try
                {
                    IEditor editor = ArcMap.Application.FindExtensionByName("ESRI Object Editor") as IEditor;
                    if (editor == null) return;
                    IWorkspaceEdit workspaceEdit = editor.EditWorkspace as IWorkspaceEdit;
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
        //选择保存时间
        private void updateSaveTime(uint? minutes)
        {
            try
            {
                string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ArcGIS";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string ini = path + @"\addin.ini";

                if (minutes == null)
                {
                    string str = File.ReadAllText(ini).Trim();
                    uint minutes2;
                    uint.TryParse(str, out minutes2);
                    minutes = minutes2;
                }
                if (minutes < 1)
                {
                    minutes = 5;
                }
                App.log("自动保存时间：" + minutes + "分钟");
                System.IO.FileStream fsWrite = new System.IO.FileStream(ini, FileMode.Create, FileAccess.Write);
                byte[] bteData = System.Text.Encoding.UTF8.GetBytes(minutes.ToString());
                fsWrite.Write(bteData, 0, bteData.Length);
                fsWrite.Flush();
                fsWrite.Dispose();
                startTimer((uint)minutes);
            }
            catch (Exception ex)
            {
                App.log(ex.Message);
            }
        }
        //cookie
        private void recCookie(int cookie)
        {
            try
            {
                string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ArcGIS";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string ini = path + @"\cookie.ini";
                System.IO.FileStream fsWrite = new System.IO.FileStream(ini, FileMode.Append, FileAccess.Write);
                byte[] bteData = System.Text.Encoding.UTF8.GetBytes(cookie + "\r\n");
                fsWrite.Write(bteData, 0, bteData.Length);
                fsWrite.Flush();
                fsWrite.Dispose();
            }
            catch (Exception ex)
            {
                App.log(ex.Message);
            }
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
}
