using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace T04AutoSave
{
    public class TestBar : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public TestBar()
        {
        }

        protected override void OnClick()
        {
            App.log("test");
        }

        protected override void OnUpdate()
        {
        }
    }
}
