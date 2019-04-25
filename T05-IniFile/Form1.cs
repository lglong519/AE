using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.IO;
using System.Text.RegularExpressions;

namespace T05_IniFile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IniFile ini = new IniFile("cookie");
            ini.set(textBox1.Text, textBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IniFile ini = new IniFile("cookie");
            MessageBox.Show(ini.get(textBox1.Text));
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        private void button3_Click(object sender, EventArgs e)
        {
            //StringBuilder temp = new StringBuilder(500);
            //string inipath = @"C:\Users\lglong519\Documents\ArcGIS\cookie.ini";
            //int res = GetPrivateProfileString("default", "444", "", temp, 500, inipath);
            //MessageBox.Show(res.ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //string inipath = null;
            //MessageBox.Show(inipath.ToString());
        }
    }
    class IniFile
    {
        string path = "";
        public IniFile(string iniFile)
        {
            path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ArcGIS\";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += (iniFile.EndsWith(".ini") ? iniFile : iniFile + ".ini");
        }
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        public void set(string key, object value, string section = "default")
        {
            if (section == "")
            {
                section = "default";
            }
            long res = WritePrivateProfileString(section, key, value.ToString(), path);
            if (res==0)
            {
                //MessageBox.Show("写入失败");
            }
        }
        public string get(string key, string section = "default")
        {
            if (section == "")
            {
                section = "default";
            }
            StringBuilder temp = new StringBuilder(10);
            long res = GetPrivateProfileString(section, key, "", temp, 10, path);
            if (res == 0)
            {
                //MessageBox.Show("读取失败");
            }
            return temp.ToString();
        }
    }
    class IniFile2
    {
        string path = "";
        string text = "";
        public IniFile2(string iniFile)
        {
            path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ArcGIS\";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += (iniFile.EndsWith(".ini") ? iniFile : iniFile + ".ini");
            text = File.ReadAllText(path);
        }
        public void set(string key, object value)
        {
            Regex reg = new Regex(key + "=" + ".*");
            if (reg.IsMatch(text))
            {
                string newValue = reg.Replace(text, key + "=" + value.ToString());
                System.IO.FileStream fs = new System.IO.FileStream(path, FileMode.Create, FileAccess.Write);
                byte[] bteData = System.Text.Encoding.UTF8.GetBytes(newValue);
                fs.Write(bteData, 0, bteData.Length);
                fs.Flush();
                fs.Dispose();
            }
            else
            {
                string newValue = key + "=" + value.ToString() + "\n";
                System.IO.FileStream fs = new System.IO.FileStream(path, FileMode.Append, FileAccess.Write);
                byte[] bteData = System.Text.Encoding.UTF8.GetBytes(newValue);
                fs.Write(bteData, 0, bteData.Length);
                fs.Flush();
                fs.Dispose();
            }
        }
        public string get(string key)
        {
            Regex reg = new Regex(key + "=" + ".*");
            if (reg.IsMatch(text))
            {
                return reg.Match(text).Groups[0].Value.Replace(key + "=", "");
            }
            else
            {
                return "";
            }
        }
    }
}
