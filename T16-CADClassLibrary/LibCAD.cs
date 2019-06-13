using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Interop;
using Autodesk.AutoCAD.Windows;
using System.Windows.Forms;
using CADApplication = Autodesk.AutoCAD.ApplicationServices.Application;

using Autodesk.AutoCAD.Customization;

namespace T16_CADClassLibrary
{
    class LibCAD
    {
        /// <summary>
        /// 创建图层
        /// </summary>
        /// <param name="layerName">图层名</param>
        /// <param name="db">数据库</param>
        /// <param name="color">颜色</param>
        /// <param name="linetype">线形</param>
        /// <returns></returns>
        [CommandMethod("createLayerByName")]
        public static ObjectId createLayerByName(string layerName, Database db, Color color = null, string linetype = null)
        {
            ObjectId layerId = ObjectId.Null;
            lockDocument();
            try
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    LayerTable layerTable = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForWrite);
                    //创建图层
                    if (!layerTable.Has(layerName))
                    {
                        LayerTableRecord layerTableRecord = new LayerTableRecord()
                        {
                            Name = layerName,
                        };
                        if (color != null)
                        {
                            layerTableRecord.Color = color;
                        }
                        layerTableRecord.LineWeight = LineWeight.LineWeight005;

                        layerId = layerTable.Add(layerTableRecord);
                        trans.AddNewlyCreatedDBObject(layerTableRecord, true);
                    }
                    if (!string.IsNullOrEmpty(linetype))
                    {
                        //设置线形
                        LayerTableRecord layerTableRecord = trans.GetObject(layerTable[layerName],
        OpenMode.ForRead) as LayerTableRecord;
                        LinetypeTable linetypeTable = trans.GetObject(db.LinetypeTableId, OpenMode.ForRead) as LinetypeTable;
                        if (linetypeTable.Has(linetype))
                        {
                            layerTableRecord.UpgradeOpen();
                            layerTableRecord.LinetypeObjectId = linetypeTable[linetype];
                        }
                    }
                    trans.Commit();
                }
            }
            catch (System.Exception ex)
            {
                App.logIO(ex.ToString());
            }
            unlockDocument();
            return layerId;
        }
        /// <summary>
        /// 删除图层
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="db"></param>
        [CommandMethod("removeLayerByName")]
        public static void removeLayerByName(string layerName, Database db)
        {
            try
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    LayerTable layerTable = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForWrite);

                    LayerTableRecord currentLayer = trans.GetObject(db.Clayer, OpenMode.ForRead) as LayerTableRecord;
                    if (currentLayer.Name.ToLower() == layerName.ToLower())
                    {
                        CADApplication.ShowAlertDialog("不能删除当前图层");
                        return;
                    }
                    if (!layerTable.Has(layerName))
                    {
                        CADApplication.ShowAlertDialog(String.Format("没有此图层:[{0}]", layerName));
                        return;
                    }
                    LayerTableRecord layerTableRecord = trans.GetObject(layerTable[layerName], OpenMode.ForWrite) as LayerTableRecord;
                    if (layerTableRecord.IsErased)
                    {
                        CADApplication.ShowAlertDialog(String.Format("此图层已经被删除:[{0}]", layerName));
                        return;
                    }
                    ObjectIdCollection idCol = new ObjectIdCollection();
                    idCol.Add(layerTableRecord.ObjectId);
                    db.Purge(idCol);
                    if (idCol.Count > 0)
                    {
                        layerTableRecord.Erase();
                        trans.Commit();
                        CADApplication.ShowAlertDialog(String.Format("成功删除图层:[{0}]", layerName));
                    }
                }
            }
            catch (System.Exception ex)
            {
                CADApplication.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.ToString());
            }
        }
        public static bool layerExists(string layerName)
        {
            Document doc = CADApplication.DocumentManager.MdiActiveDocument;
            try
            {
                Database db = doc.Database;
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    LayerTable layerTable = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                    if (!layerTable.Has(layerName))
                    {
                        return false;
                    }
                    LayerTableRecord layerTableRecord = trans.GetObject(layerTable[layerName], OpenMode.ForRead) as LayerTableRecord;
                    if (layerTableRecord.IsErased)
                    {
                        return false;
                    }
                }
            }
            catch (System.Exception ex)
            {
                doc.Editor.WriteMessage(ex.Message);
                App.logIO(ex.ToString());
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获得当前视口(只读)
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static ViewportTableRecord getCurrentViewport(Database db)
        {
            //ViewportTableRecord vptr = new ViewportTableRecord();
            ViewportTableRecord vptr = null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                ViewportTable viewportTable = trans.GetObject(db.ViewportTableId, OpenMode.ForWrite) as ViewportTable;
                vptr = trans.GetObject(viewportTable["*Active"], OpenMode.ForWrite) as ViewportTableRecord;
                trans.Commit();
            }
            return vptr;
        }
        /// <summary>
        /// 获得当前视图
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static ViewTable getCurrentView(Database db)
        {
            ViewTable viewTable = null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                viewTable = trans.GetObject(db.ViewTableId, OpenMode.ForWrite) as ViewTable;
                trans.Commit();
            }
            return viewTable;
        }
        /// <summary>
        /// 两点创造直线
        /// </summary>
        /// <param name="pointStart"></param>
        /// <param name="pointEnd"></param>
        /// <returns></returns>
        public static Line createLineByPoint(Point3d pointStart, Point3d pointEnd)
        {
            return new Line(pointStart, pointEnd);
        }
        /// <summary>
        /// 两坐标创造直线
        /// </summary>
        /// <param name="axisStart"></param>
        /// <param name="axisEnd"></param>
        /// <returns></returns>
        public static Line createLineByAxis(double[] axisStart, double[] axisEnd)
        {
            return new Line(new Point3d(axisStart[0], axisStart[1], axisStart[2]), new Point3d(axisEnd[0], axisEnd[1], axisEnd[2]));
        }
        /// <summary>
        /// 点集合创建带高程多段线
        /// </summary>
        /// <param name="points">点集合</param>
        /// <param name="isClosed">是否闭合</param>
        /// <returns></returns>
        public static Polyline3d createPolyline3dByPoints(Point3dCollection points, bool isClosed)
        {
            return new Polyline3d(Poly3dType.SimplePoly, points, isClosed);
        }
        public static PaletteSet createPalette(string title)
        {
            //Guid gd = new Guid("AA5464B1-6565-45E6-B987-AFAAFBDE7BB1");
            //PaletteSet paletteSet = new PaletteSet(title, gd);
            PaletteSet paletteSet = new PaletteSet(title);
            UserControl1 palette = new UserControl1();
            paletteSet.Add("Smart3d0", palette);
            paletteSet.Size = new System.Drawing.Size(90, 100);
            paletteSet.Dock = DockSides.Top;
            paletteSet.Visible = true;
            return paletteSet;
        }
        public static void createCui()
        {
            //自定义的组名
            string strMyGroupName = "MyGroup";
            //保存的CUI文件名（从CAD2010开始，后缀改为了cuix）
            string strCuiFileName = "MyMenu.cui";

            //创建一个自定义组（这个组中将包含我们自定义的命令、菜单、工具栏、面板等）
            CustomizationSection myCSection = new CustomizationSection();
            myCSection.MenuGroupName = strMyGroupName;

            //创建自定义命令组
            MacroGroup mg = new MacroGroup("MyMethod", myCSection.MenuGroup);
            MenuMacro mm1 = new MenuMacro(mg, "打开文件", "OF", "");
            MenuMacro mm2 = new MenuMacro(mg, "打开模板", "OM", "");
            MenuMacro mm3 = new MenuMacro(mg, "保存", "SV", "");

            //声明菜单别名
            StringCollection scMyMenuAlias = new StringCollection();
            scMyMenuAlias.Add("MyPop1");
            scMyMenuAlias.Add("MyTestPop");

            //菜单项（将显示在项部菜单栏中）
            PopMenu pmParent = new PopMenu("我的菜单", scMyMenuAlias, "我的菜单", myCSection.MenuGroup);

            //子项的菜单（多级）
            PopMenu pm1 = new PopMenu("打开", new StringCollection(), "", myCSection.MenuGroup);
            PopMenuRef pmr1 = new PopMenuRef(pm1, pmParent, -1);
            PopMenuItem pmi1 = new PopMenuItem(mm1, "文件", pm1, -1);
            PopMenuItem pmi2 = new PopMenuItem(mm2, "模板", pm1, -1);

            //子项的菜单（单级）
            PopMenuItem pmi3 = new PopMenuItem(mm3, "保存(&S)", pmParent, -1);

            // 最后保存文件
            myCSection.SaveAs(strCuiFileName);
        }
        #region 为编辑锁定文档(切勿嵌套)
        static DocumentLock m_DocumentLock;
        public static void lockDocument()
        {
            unlockDocument();
            m_DocumentLock = CADApplication.DocumentManager.MdiActiveDocument.LockDocument();
        }
        public static void unlockDocument()
        {
            if (m_DocumentLock != null)
            {
                m_DocumentLock.Dispose();
                m_DocumentLock = null;
            }
        }
        #endregion
        /// <summary>
        /// 修改线图形所在的图层
        /// </summary>
        /// <param name="shape">图形</param>
        /// <param name="layerName">图层名</param>
        /// <param name="autoCreateLayer">图层不存在是否创建</param>
        /// <returns></returns>
        public static bool setLayer(Line shape, string layerName, bool autoCreateLayer = false)
        {
            if (layerExists(layerName))
            {
                shape.Layer = layerName;
                return true;
            }
            if (autoCreateLayer)
            {
                Document doc = CADApplication.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                createLayerByName(layerName, db, Color.FromRgb(255, 255, 0));
                shape.Layer = layerName;
                return true;
            }
            return false;
        }
        public static bool setLayer(DBPoint shape, string layerName, bool autoCreateLayer = false)
        {
            if (layerExists(layerName))
            {
                shape.Layer = layerName;
                return true;
            }
            if (autoCreateLayer)
            {
                Document doc = CADApplication.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                createLayerByName(layerName, db, Color.FromRgb(255, 0, 255));
                shape.Layer = layerName;
                return true;
            }
            return false;
        }
        public static bool setLayer(Polyline3d shape, string layerName, bool autoCreateLayer = false)
        {
            if (layerExists(layerName))
            {
                shape.Layer = layerName;
                return true;
            }
            if (autoCreateLayer)
            {
                Document doc = CADApplication.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                createLayerByName(layerName, db, Color.FromRgb(40, 226, 61));
                shape.Layer = layerName;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 平移视图到指定坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void panTo(double x, double y)
        {
            Document doc = CADApplication.DocumentManager.MdiActiveDocument;
            ViewTableRecord vptr = doc.Editor.GetCurrentView();
            vptr.CenterPoint = new Point2d(x, y);
            doc.Editor.SetCurrentView(vptr);
        }
    }
}
