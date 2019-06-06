using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace T12_AddInVersionRelease
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //Console.WriteLine(Application.ProductName);
            //Process[] processes = Process.GetProcessesByName("VersionRelease");
            //if (processes.Length > 0)
            //{
            //    MessageBox.Show("程序已运行");
            //    this.Close();
            //    Application.Exit();
            //}
            bool ret;
            Mutex mutex = new Mutex(true, Application.ProductName, out ret);
            if (!ret)
            {
                MessageBox.Show("程序已运行");
                this.Close();
            }
            Regex reg = new Regex(@"<Version>(\d+\.\d+(.\d+)?)+</Version>", RegexOptions.IgnoreCase);
            string path = System.Environment.CurrentDirectory;
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] fileInfos = dirInfo.GetFiles();
            if (fileInfos.Length < 1)
            {
                MessageBox.Show("Cannot find any version");
                return;
            }
            bool hasV = false;
            foreach (var item in fileInfos)
            {
                if (!item.Name.EndsWith("cs") && !item.Name.EndsWith("esriaddinx"))
                {
                    continue;
                }
                if (item.Name.Contains("FrmChangeLog"))
                {
                    continue;
                }
                FileStream fs = item.Open(FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("utf-8"));
                string line = sr.ReadLine();
                while (line != null)
                {
                    if (reg.IsMatch(line))
                    {
                        GroupCollection collection = reg.Match(line).Groups;
                        textBox1.Text = collection[1].ToString();
                        textBox2.Text = textBox1.Text;
                        hasV = true;
                        break;
                    }
                    line = sr.ReadLine();
                }
                sr.Close();
                fs.Close();
                if (hasV)
                {
                    break;
                }
            }
            if (!hasV)
            {
                MessageBox.Show("Cannot find any version");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string oldV = textBox1.Text.Trim();
            string newV = textBox2.Text.Trim();
            if (string.IsNullOrEmpty(oldV) || string.IsNullOrEmpty(newV))
            {
                MessageBox.Show("Invalid Version");
                return;
            }
            if (oldV == newV)
            {
                MessageBox.Show("Version Exists");
                return;
            }
            string path = System.Environment.CurrentDirectory;
            try
            {
                updateVersion(path);
                MessageBox.Show("完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }
        private void updateVersion(string path)
        {
            string oldV = textBox1.Text.Trim();
            string newV = textBox2.Text.Trim();
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            DirectoryInfo[] dirInfos = dirInfo.GetDirectories();
            foreach (var item in dirInfos)
            {
                updateVersion(item.FullName);
            }
            FileInfo[] fileInfos = dirInfo.GetFiles();
            foreach (var item in fileInfos)
            {
                if (!item.Name.EndsWith("cs") && !item.Name.EndsWith("esriaddinx"))
                {
                    continue;
                }
                if (item.Name.Contains("FrmChangeLog"))
                {
                    continue;
                }
                string allText = File.ReadAllText(item.FullName);
                if (!allText.Contains(oldV))
                {
                    allText = null;
                    continue;
                }
                allText = allText.Replace(oldV, newV);
                Regex dateReg = new Regex("<Date>.*?</Date>", RegexOptions.IgnoreCase);
                if (dateReg.IsMatch(allText))
                {
                    allText = dateReg.Replace(allText, String.Format("<Date>{0:yyyy/MM/dd}</Date>", DateTime.Now));
                }
                FileStream fs = item.Open(FileMode.Open, FileAccess.Write);
                byte[] bteData = Encoding.UTF8.GetBytes(allText);
                fs.Write(bteData, 0, bteData.Length);
                fs.Flush();
                fs.Close();
                /*
                FileStream fs = item.Open(FileMode.Open, FileAccess.Read);
                StringBuilder sb = new StringBuilder();
                StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("utf-8"));
                string line = sr.ReadLine();
                while (line != null)
                {
                    sb.AppendLine(line.Replace(oldV, newV));
                    line = sr.ReadLine();
                }
                sr.Close();
                fs.Close();
                fs = item.Open(FileMode.Open, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("utf-8"));
                sw.Write(sb.ToString());
                sw.Close();
                fs.Close();
                 */
            }
        }
    }
}
