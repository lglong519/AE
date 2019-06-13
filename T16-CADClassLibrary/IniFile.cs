using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace T16_CADClassLibrary
{
    delegate void Err(string message);
    class IniFile
    {
        string path = "";
        public Err err = new Err(a => { });
        string section = "default";
        public IniFile(string iniFile, string section = "")
        {
            path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ArcGIS\";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += (iniFile.EndsWith(".ini") ? iniFile : iniFile + ".ini");
            if (!string.IsNullOrEmpty(section.Trim()))
            {
                this.section = section.Trim();
            }
        }
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        public void set(string key, object value, string section = "")
        {
            string localSection = section.Trim();
            if (string.IsNullOrEmpty(localSection))
            {
                localSection = this.section;
            }
            long res = WritePrivateProfileString(localSection, key, value.ToString().Trim(), path);
            if (res == 0)
            {
                err("ini写入失败");
            }
        }
        public string get(string key, string section = "")
        {
            string localSection = section.Trim();
            if (string.IsNullOrEmpty(localSection))
            {
                localSection = this.section;
            }
            StringBuilder temp = new StringBuilder(10);
            long res = GetPrivateProfileString(localSection, key, "", temp, 10, path);
            if (res == 0)
            {
                err("ini写入失败");
            }
            return temp.ToString().Trim();
        }
    }
}
