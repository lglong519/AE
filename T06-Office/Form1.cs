using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using System.Collections;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace T06_Office
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Helper().
        }

        //
        public class Helper
        {
            public bool Execute(string excelPath, string docPath, string shpPath, out string message)
            {
                return ExecuteEX(excelPath, docPath, shpPath, out message);
            }

            /// <summary>
            /// 执行函数
            /// </summary>
            /// <param name="excelPath">excel路径</param>
            /// <param name="docPath">文档路径</param>
            /// <param name="shpPath">shp文件路径</param>
            /// <returns></returns>
            private bool ExecuteEX(string excelPath, string docPath, string shpPath, out string message)
            {
                try
                {
                    if (!judgeInOrOutFile(excelPath, docPath, shpPath))
                    {
                        message = "文件输入不正确！";
                        return false;
                    }
                    //判断excel文件是否存在,若存在删除
                    if (File.Exists(excelPath))
                    {
                        File.Delete(excelPath);
                    }
                    string docResultPath = Path.Combine(Path.GetDirectoryName(docPath), "data.doc");
                    //判断doc文件是否存在，若存在删除
                    if (File.Exists(docResultPath))
                    {
                        File.Delete(docResultPath);
                    }
                    //拷贝一份word文档
                    File.Copy(docPath, docResultPath);
                    //打开shp
                    string folder = Path.GetDirectoryName(shpPath);
                    IWorkspaceFactory pWSF = new ShapefileWorkspaceFactory();
                    IFeatureWorkspace pWS = (IFeatureWorkspace)pWSF.OpenFromFile(folder, 0);
                    IFeatureClass pFeatureClass = pWS.OpenFeatureClass(Path.GetFileNameWithoutExtension(shpPath));
                    //获得shp属性表并创建dataTable
                    IFeatureCursor featureCursor = pFeatureClass.Search(null, false);
                    IFeature feature = featureCursor.NextFeature();
                    DataTable dt = NewDataTable();
                    string value = null;
                    while (feature != null)
                    {
                        //新建行
                        DataRow dr = dt.NewRow();
                        //从shp属性表中获得city属性
                        value = feature.get_Value(pFeatureClass.FindField("City")).ToString();
                        //转换为汉字
                        string strvalue = GetVlue(value);
                        //赋值
                        dr["City"] = strvalue;
                        value = feature.get_Value(pFeatureClass.FindField("affcountyN")).ToString();
                        dr["affcountyN"] = Math.Round(TODouble(value), 2);
                        //datatable添加此行
                        dt.Rows.Add(dr);
                        feature = featureCursor.NextFeature();
                    }
                    //创建一个wordApplication
                    WordOper wordOper = new WordOper();
                    wordOper.OpenAndActive(docResultPath, false, false);
                    //表格赋值
                    wordOper.TableValue(dt);
                    wordOper.Save();
                    wordOper.Close();
                    //改变表格列名
                    dt.Columns["City"].ColumnName = "地区";
                    dt.Columns["affcountyN"].ColumnName = "直接经济损失(万元) ";
                    //另存表格
                    ExcelOper excel = new ExcelOper();
                    Hashtable hashTable = new Hashtable();
                    hashTable.Add("直接经济损失分布产品", dt);
                    bool issucess = false;
                    if (excel.WriteExcel(excelPath, hashTable, out message))
                    {
                        message = "数据生成成功！";
                        issucess = true;
                    }
                    else
                    {
                        message = "统计数据生成失败！";
                        issucess = false;
                    }
                    return issucess;
                }
                catch (Exception ex)
                {
                    message = "失败！";
                    return false;
                }
            }
            /// <summary>
            /// 新建datatable
            /// </summary>
            /// <returns>返回datatable</returns>
            private DataTable NewDataTable()
            {
                //新建datatable
                DataTable dt = new DataTable();
                //新建列
                DataColumn dCol = null;
                dCol = new DataColumn();
                // 指定列名和类型
                dCol.ColumnName = "City";
                dCol.DataType = typeof(string);
                dt.Columns.Add(dCol);
                dCol = new DataColumn();
                dCol.ColumnName = "affcountyN";
                dCol.DataType = typeof(double);
                //  datatable添加列
                dt.Columns.Add(dCol);
                return dt;
            }
            /// <summary>
            /// 判断输入输出格式是否正确
            /// </summary>
            /// <param name="excelPath">excel路径</param>
            /// <param name="docPath">文档路径</param>
            /// <param name="shpPath">shp文件</param>
            /// <returns></returns>
            private bool judgeInOrOutFile(string excelPath, string docPath, string shpPath)
            {
                //判断excel文件名称是否正确,不正确则运行失败
                if (!excelPath.EndsWith(".xls") && !excelPath.EndsWith(".xlsx"))
                {
                    MessageBox.Show(excelPath + "表格名称输入不正确！");
                    return false;
                }
                //判断doc文件名称是否正确,不正确则运行失败
                if (!File.Exists(docPath))
                {
                    MessageBox.Show(docPath + "不存在！");
                    return false;
                }
                //判断shp文件是否存在,不存在则运行失败
                if (!File.Exists(shpPath))
                {
                    MessageBox.Show(shpPath + "不存在！");
                    return false;
                }
                return true;
            }
            /// <summary>
            /// string 转换为double
            /// </summary>
            /// <param name="value">string 值</param>
            /// <returns></returns>
            private double TODouble(string value)
            {
                try
                {
                    double number = Convert.ToDouble(value);
                    return number;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
            /// <summary>
            /// 转换为汉字
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            private string GetVlue(string name)
            {
                byte[] temp = Encoding.GetEncoding("ISO8859-1").GetBytes(name);
                string value2 = Encoding.Default.GetString(temp);
                return value2;
            }
        }
        //word文档代码：
        public class WordOper
        {
            #region 私有成员
            private Microsoft.Office.Interop.Word.ApplicationClass _wordApplication;
            private Microsoft.Office.Interop.Word.Document _wordDocument;
            object missing = System.Reflection.Missing.Value;
            #endregion
            #region  公开属性
            /// <summary>
            /// ApplciationClass
            /// </summary>
            public Microsoft.Office.Interop.Word.ApplicationClass WordApplication
            {
                get
                {
                    return _wordApplication;
                }
            }
            /// <summary>
            /// Document
            /// </summary>
            public Microsoft.Office.Interop.Word.Document WordDocument
            {
                get
                {
                    return _wordDocument;
                }
            }
            #endregion
            #region  构造函数
            public WordOper()
            {
                _wordApplication = new Microsoft.Office.Interop.Word.ApplicationClass();
            }
            public WordOper(Microsoft.Office.Interop.Word.ApplicationClass wordApplication)
            {
                _wordApplication = wordApplication;
            }
            #endregion
            #region 基本操作（新建、打开、保存、关闭）
            /// <summary>
            /// 打开指定文件
            /// </summary>
            /// <param name="FileName">文件名（包含路径）</param>
            /// <param name="IsReadOnly">打开后是否只读</param>
            /// <param name="IsVisibleWin">打开后是否可视</param>
            /// <returns>打开是否成功</returns>
            public bool OpenAndActive(string FileName, bool IsReadOnly, bool IsVisibleWin)
            {
                if (string.IsNullOrEmpty(FileName))
                {
                    return false;
                }
                try
                {
                    _wordDocument = OpenOneDocument(FileName, missing, IsReadOnly, missing, missing, missing, missing, missing, missing, missing, missing, IsVisibleWin, missing, missing, missing, missing);
                    _wordDocument.Activate();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            /// <summary>
            /// 关闭
            /// Closes the specified document or documents.
            /// </summary>
            public void Close()
            {
                if (_wordDocument != null)
                {
                    //垃圾回收
                    _wordDocument.Close(ref missing, ref missing, ref missing);
                    ((Microsoft.Office.Interop.Word._Application)_wordApplication).Application.Quit(ref missing, ref missing, ref missing);
                    GC.Collect();
                }
            }
            /// <summary>
            /// 保存
            /// </summary>
            public void Save()
            {
                if (_wordDocument == null)
                {
                    _wordDocument = _wordApplication.ActiveDocument;
                }
                _wordDocument.Save();
            }
            /// <summary>
            /// 打开一个已有文档
            /// </summary>
            /// <param name="FileName"></param>
            /// <param name="ConfirmConversions"></param>
            /// <param name="ReadOnly"></param>
            /// <param name="AddToRecentFiles"></param>
            /// <param name="PasswordDocument"></param>
            /// <param name="PasswordTemplate"></param>
            /// <param name="Revert"></param>
            /// <param name="WritePasswordDocument"></param>
            /// <param name="WritePasswordTemplate"></param>
            /// <param name="Format"></param>
            /// <param name="Encoding"></param>
            /// <param name="Visible"></param>
            /// <param name="OpenAndRepair"></param>
            /// <param name="DocumentDirection"></param>
            /// <param name="NoEncodingDialog"></param>
            /// <param name="XMLTransform"></param>
            /// <returns></returns>
            public Microsoft.Office.Interop.Word.Document OpenOneDocument(object FileName, object ConfirmConversions, object ReadOnly,
                object AddToRecentFiles, object PasswordDocument, object PasswordTemplate, object Revert,
                object WritePasswordDocument, object WritePasswordTemplate, object Format, object Encoding,
                object Visible, object OpenAndRepair, object DocumentDirection, object NoEncodingDialog, object XMLTransform)
            {
                try
                {
                    return _wordApplication.Documents.Open(ref FileName, ref ConfirmConversions, ref ReadOnly, ref AddToRecentFiles,
                        ref PasswordDocument, ref PasswordTemplate, ref Revert, ref WritePasswordDocument, ref WritePasswordTemplate,
                        ref Format, ref Encoding, ref Visible, ref OpenAndRepair, ref DocumentDirection, ref NoEncodingDialog, ref XMLTransform);
                }
                catch
                {
                    return null;
                }
            }
            #endregion
            #region 查找、替换
            /// <summary>
            /// 在文档中写表格
            /// </summary>
            /// <param name="dt"></param>
            /// <returns></returns>
            public bool TableValue(System.Data.DataTable dt)
            {
                try
                {
                    Word.Table table = _wordDocument.Tables[1] as Word.Table;
                    if (table.Rows.Count == 1)
                        table.Rows.Add(missing);
                    for (int col = 1; col < table.Columns.Count; col++)
                    {
                        for (int row = 0; row < dt.Rows.Count; row++)
                        {
                            table.Cell(row + 2, col).Range.Text = dt.Rows[row][col].ToString();
                            if (row != dt.Rows.Count - 1)
                                table.Rows.Add(missing);
                        }
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            #endregion
        }
        //excel相关代码：
        public class ExcelOper
        {
            private Excel.Application _excelApp = null;
            public ExcelOper()
            {
            }
            #region 写Excel
            /// <summary>
            /// 创建Excel进程
            /// </summary>
            /// <returns></returns>
            private Excel.Application GetExcelApp()
            {

                Excel.Application excelApp = new Excel.Application();
                excelApp.Application.Workbooks.Add(true);
                _excelApp = excelApp;
                return excelApp;
            }
            /// <summary>
            /// 把DataTable的内容写入Excel
            /// </summary>
            /// <param name="strExcelPath">excel文件的路径</param>
            /// <param name="htDataTable">key：sheetName，value：DataTable</param>
            /// <returns></returns>
            public bool WriteExcel(string strExcelPath, Hashtable htDataTable, out string message)
            {
                if (htDataTable == null || htDataTable.Count == 0)
                {
                    message = "数据表为空";
                    return false;
                }
                bool writeRst = false;
                try
                {
                    if (_excelApp == null)
                    {
                        GetExcelApp();
                    }
                    //依次写入Sheet页
                    int countNum = 1;
                    foreach (DictionaryEntry de in htDataTable)
                    {
                        string sheetName = de.Key.ToString();
                        System.Data.DataTable dtTable = (System.Data.DataTable)de.Value;
                        Excel.Worksheet excelSheet = null;
                        if (countNum == 1)
                        {
                            excelSheet = (Excel.Worksheet)_excelApp.Worksheets[countNum];
                        }
                        else
                        {
                            excelSheet = (Excel.Worksheet)_excelApp.Worksheets.Add(Type.Missing, Type.Missing, 1, Type.Missing);
                        }
                        excelSheet.Name = sheetName;
                        bool sheetRst = writeSheet(excelSheet, dtTable);
                        if (!sheetRst)
                        {
                            throw new Exception(sheetName + "创建失败！");
                        }
                        countNum++;
                    }
                    //保存
                    _excelApp.Visible = false;
                    _excelApp.DisplayAlerts = false;
                    _excelApp.AlertBeforeOverwriting = false;
                    _excelApp.ActiveWorkbook.SaveAs(strExcelPath, Type.Missing, null, null, false, false, Excel.XlSaveAsAccessMode.xlNoChange, null, null, null, null, null);
                    message = "数据输出成功！";
                    writeRst = true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    //LogHelper.Error.Append(ex);
                    writeRst = false;
                }
                finally
                {
                    //关闭Excel进程
                    object missing = System.Reflection.Missing.Value;
                    _excelApp.ActiveWorkbook.Close(missing, missing, missing);
                    _excelApp.Quit();
                    _excelApp = null;
                    //垃圾回收
                    GC.Collect();
                }
                return writeRst;
            }
            /// <summary>
            /// 写Sheet页
            /// </summary>
            /// <param name="excelSheet"></param>
            /// <param name="dtTable"></param>
            /// <returns></returns>
            private bool writeSheet(Excel.Worksheet excelSheet, System.Data.DataTable dtTable)
            {
                if (excelSheet == null || dtTable == null)
                {
                    return false;
                }
                //列名
                for (int i = 0; i < dtTable.Columns.Count; i++)
                {
                    DataColumn dtColumn = dtTable.Columns[i];
                    string caption = dtColumn.Caption;
                    excelSheet.Cells[1, i + 1] = caption;
                }
                //写入值
                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dtTable.Columns.Count; j++)
                    {
                        object objValue = dtTable.Rows[i][j];
                        excelSheet.Cells[2 + i, j + 1] = objValue;
                    }
                }
                excelSheet.Columns.AutoFit();
                return true;
            }
            #endregion
        }
    }
}
