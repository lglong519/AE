using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace T16_CADClassLibrary
{
    class App
    {
        /// <summary>
        /// 写入本地日志
        /// </summary>
        /// <param name="strs"></param>
        public static void logIO(params object[] strs)
        {
            try
            {
                string str = stringParser(strs).Trim(); ;
                if (str == "") return;
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ArcGIS";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string logFullPath = path + @"\DsjToolsCAD.log";
                FileStream fsWrite = new FileStream(logFullPath, FileMode.Append, FileAccess.Write);
                if (!fsWrite.CanWrite) return;
                byte[] bteData = Encoding.UTF8.GetBytes(string.Format("[{0}]{1}\n", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), str));
                fsWrite.Write(bteData, 0, bteData.Length);
                fsWrite.Flush();
                fsWrite.Dispose();
            }
            catch (Exception e)
            {

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
}
