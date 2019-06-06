using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DsjToolbar;

namespace InstallAddIn
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string arcMapPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ArcGIS\AddIns\Desktop10.2\");
                const string addInId = "{84d377ba-04ee-4f27-9d8d-d92d794daf46}";
                string addInPath = Path.Combine(arcMapPath, addInId);

                string currentPath = System.Environment.CurrentDirectory;
                DirectoryInfo currentInfo = new DirectoryInfo(currentPath);
                FileInfo[] addInInfos = currentInfo.GetFiles();
                string addin = "";
                string desAddin = "";
                string addinName = "";
                foreach (FileInfo item in addInInfos)
                {
                    if (item.Extension == ".esriAddIn")
                    {
                        addin = item.FullName;
                        addinName = item.Name;
                        desAddin = Path.Combine(addInPath, addinName);
                    }
                }
                if (string.IsNullOrEmpty(addin))
                {
                    MessageBox.Show("当前目录未找到 esriAddIn 文件", "安装失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Console.Write("插件路径:");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine(arcMapPath);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("插件ID:");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine(addInId);
                Console.ForegroundColor = ConsoleColor.White;
                if (!Directory.Exists(addInPath))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("未检测到旧插件");
                }
                else
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(addInPath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();
                    if (fileInfos.Length > 0)
                    {
                        Console.WriteLine("卸载列表:");
                        for (int i = 0; i < fileInfos.Length; i++)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\t" + fileInfos[i].Name);
                        }
                    }
                    directoryInfo.Delete(true);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("卸载完成");
                }
                Directory.CreateDirectory(addInPath);
                File.Copy(addin, desAddin);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("安装完成: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(addinName);
                MessageBox.Show("安装完成");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(ex.Message);
                App.logIO(ex.ToString());
            }
        }
    }
}
