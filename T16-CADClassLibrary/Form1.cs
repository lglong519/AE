using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TTimer = System.Threading.Timer;

using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Interop;
using Microsoft.Win32;
using System.Reflection;

using CADApplication = Autodesk.AutoCAD.ApplicationServices.Application;

namespace T16_CADClassLibrary
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            int x, y;
            if (int.TryParse(iniFile.get("FrmSmart3dX"), out x) && int.TryParse(iniFile.get("FrmSmart3dY"), out y))
            {
                SetDesktopLocation(process(new int[2] { x, y })[0], process(new int[2] { x, y })[1]);
            }
            else
            {
                StartPosition = FormStartPosition.CenterScreen;
            }
            Move += new EventHandler(Form1_Move);
            Disposed += new EventHandler((a, b) =>
            {
                if (timer != null)
                {
                    timer.Dispose();
                    timer = null;
                }
            });
        }
        private void Form1_Move(object sender, EventArgs e)
        {
            updatePosTimer(Location.X, Location.Y);
        }
        TTimer timer;
        IniFile iniFile = new IniFile("DsjTools", "CAD");
        private void updatePosTimer(int x, int y)
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = new TTimer(posHandle, new int[2] { x, y }, 300, 0);
                return;
            }
            timer = new TTimer(posHandle, new int[2] { x, y }, 300, 0);
        }

        private void posHandle(object pos)
        {
            iniFile.set("FrmSmart3dX", process(pos)[0]);
            iniFile.set("FrmSmart3dY", process(pos)[1]);
            timer = null;
        }

        private int[] process(object pos)
        {
            int[] obj = pos as int[];
            if (obj[0] < 0)
            {
                obj[0] = 0;
            }
            if (obj[1] < 0)
            {
                obj[1] = 0;
            }
            if (obj[0] > Screen.PrimaryScreen.WorkingArea.Size.Width - Width)
            {
                obj[0] = Screen.PrimaryScreen.WorkingArea.Size.Width - Width;
            }
            if (obj[1] > Screen.PrimaryScreen.WorkingArea.Size.Height - Height)
            {
                obj[1] = Screen.PrimaryScreen.WorkingArea.Size.Height - Height;
            }
            return obj;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Line line1 = new Line();//声明一个直线对象
                Point3d startPoint3D = new Point3d(100, 100, 0);//创建两个点
                Point3d endPoint3D = new Point3d(100, 200, 0);
                LibCAD.setLayer(line1, "shape", true);
                line1.StartPoint = startPoint3D;//设置属性
                line1.EndPoint = endPoint3D;
                //声明图形数据库对象
                Document doc = CADApplication.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;

                Line line2 = new Line(new Point3d(100, 200, 0), new Point3d(200, 200, 0));
                LibCAD.setLayer(line2, "shape", true);
                Line line3 = new Line(new Point3d(200, 200, 0), new Point3d(200, 100, 0));
                LibCAD.setLayer(line3, "shape", true);
                Line line4 = new Line(new Point3d(200, 100, 0), new Point3d(100, 100, 0));
                LibCAD.setLayer(line4, "shape", true);
                LibCAD.lockDocument();
                //开启事务处理
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);//打开块表
                    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);//打开块表记录
                    btr.AppendEntity(line1);//加直线到块表记录
                    btr.AppendEntity(line2);//加直线到块表记录
                    btr.AppendEntity(line3);//加直线到块表记录
                    btr.AppendEntity(line4);//加直线到块表记录
                    trans.AddNewlyCreatedDBObject(line1, true);//更新数据
                    trans.AddNewlyCreatedDBObject(line2, true);//更新数据
                    trans.AddNewlyCreatedDBObject(line3, true);//更新数据
                    trans.AddNewlyCreatedDBObject(line4, true);//更新数据
                    trans.Commit();//提交事务
                }
            }
            catch (System.Exception ex)
            {
                App.logIO(ex.ToString());
            }
            LibCAD.unlockDocument();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Document doc = CADApplication.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            LibCAD.createLayerByName("zz", db);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LibCAD.panTo(0, 0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LibCAD.panTo(100, 100);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LibCAD.panTo(-100, -100);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                Document doc = CADApplication.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;

                DBPoint point0 = new DBPoint(new Point3d(0, 0, 0));
                DBPoint point1 = new DBPoint(new Point3d(50, 50, 50));
                DBPoint point2 = new DBPoint(new Point3d(120, 120, 120));
                LibCAD.setLayer(point0, "point", true);
                LibCAD.setLayer(point1, "point", true);
                LibCAD.setLayer(point2, "point", true);

                LibCAD.lockDocument();
                //开启事务处理
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);//打开块表
                    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);//打开块表记录
                    btr.AppendEntity(point0);
                    btr.AppendEntity(point1);
                    btr.AppendEntity(point2);
                    trans.AddNewlyCreatedDBObject(point0, true);
                    trans.AddNewlyCreatedDBObject(point1, true);
                    trans.AddNewlyCreatedDBObject(point2, true);
                    trans.Commit();//提交事务
                }

            }
            catch (System.Exception ex)
            {
                App.logIO(ex.ToString());
            }
            LibCAD.unlockDocument();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Document doc = CADApplication.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            Point3dCollection points = new Point3dCollection();
            points.Add(new Point3d(0, 0, 0));
            points.Add(new Point3d(50, 50, 50));
            points.Add(new Point3d(50, -50, 10));
            Polyline3d polyline = LibCAD.createPolyline3dByPoints(points, true);
            LibCAD.setLayer(polyline, "polyline", true);

            LibCAD.lockDocument();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);//打开块表
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);//打开块表记录
                btr.AppendEntity(polyline);
                trans.AddNewlyCreatedDBObject(polyline, true);
                trans.Commit();//提交事务
            }
        }
    }
}
