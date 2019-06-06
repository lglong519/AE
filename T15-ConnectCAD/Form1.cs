using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//acdbmgd
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.LayerManager;
//acmgd
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.ComponentModel;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.GraphicsSystem;
using Autodesk.AutoCAD.PlottingServices;
using Autodesk.AutoCAD.Publishing;
using Autodesk.AutoCAD.Windows;

using Autodesk.AutoCAD;
using Autodesk.AutoCAD.Interop;
using System.Runtime.InteropServices;

namespace T15_ConnectCAD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        AcadApplication acApp
        {
            get
            {
                // "AutoCAD.Application.17" uses 2007 or 2008,
                //   whichever was most recently run
                // "AutoCAD.Application.17.1" uses 2008, specifically
                const string progID = "AutoCAD.Application.17.1";
                AcadApplication acApp = null;
                try
                {
                    acApp = (AcadApplication)Marshal.GetActiveObject(progID);
                }
                catch
                {
                    try
                    {
                        Type acType = Type.GetTypeFromProgID(progID);
                        acApp = (AcadApplication)Activator.CreateInstance(acType, true);
                    }
                    catch
                    {
                        //MessageBox.Show("Cannot create object of type \"" + progID + "\"");
                        Console.WriteLine(String.Format("Cannot create object of type \"{0}\"", progID));
                    }
                }
                if (acApp != null)
                {
                    // By the time this is reached AutoCAD is fully
                    // functional and can be interacted with through code
                    acApp.Visible = true;
                }
                return acApp;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (acApp != null)
            {
                // By the time this is reached AutoCAD is fully
                // functional and can be interacted with through code
                acApp.Visible = true;
                //acApp.ActiveDocument.SendCommand("ddptype ");
                for (int i = 0; i < 10; i++)
                {
                    new System.Threading.Timer((a) =>
                    {
                        acApp.ActiveDocument.SendCommand("point " + string.Format("{0},{0} ", int.Parse(a.ToString()) * 10));
                    }, i, 1000, 0);
                }
            }
        }
    }
}
