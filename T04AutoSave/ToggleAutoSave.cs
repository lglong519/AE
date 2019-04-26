using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace T04AutoSave
{
    public class ToggleAutoSave : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public ToggleAutoSave()
        {
            checkState();
        }

        protected override void OnClick()
        {
            // ArcMap.Application.CurrentTool = null;
            IniFile ini = new IniFile("cookie");
            if (ini.get("active") == "0")
            {
                ini.set("active", 1);
            }
            else
            {
                ini.set("active", 0);
            }
            checkState();
        }
        protected override void OnUpdate()
        {
        }
        private void checkState()
        {
            IniFile ini = new IniFile("cookie");
            if (ini.get("active") == "0")
            {
                Checked = false;
                //Caption = "已关闭";
                Tooltip = "已关闭,";
                Message = "点击重新启动自动保存";
            }
            else
            {
                Checked = true;
                //Caption = "已开启";
                Tooltip = "已开启";
                Message = "点击关闭自动保存";
            }
            App.log("自动保存[" + Tooltip + "]");
        }
    }
}
