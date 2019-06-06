using System;
using System.IO;
using System.Text;

namespace DsjToolbar
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
                string logFullPath = path + @"\DsjTools.log";
                FileStream fsWrite = new FileStream(logFullPath, FileMode.Append, FileAccess.Write);
                if (!fsWrite.CanWrite) return;
                byte[] bteData = Encoding.UTF8.GetBytes(string.Format("[{0}]{1}{2}", LocalTime.fullDate, str, "\n"));
                fsWrite.Write(bteData, 0, bteData.Length);
                fsWrite.Flush();
                fsWrite.Close();
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
}
