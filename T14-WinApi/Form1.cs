using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using Autodesk.AutoCAD.Interop;

namespace T14_WinApi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            init();
        }
        bool LeftTag = false;
        bool RightTag = false;
        Point p1 = new Point(0, 0);
        Point p2 = new Point(0, 0);
        MouseHook mh;
        KeyboardHook k_hook;
        List<Axis> linePoints = new List<Axis>();

        private void init()
        {
            mh = new MouseHook();
            mh.SetHook();
            mh.MouseDownEvent += mh_MouseDownEvent;
            mh.MouseUpEvent += mh_MouseUpEvent;
            Disposed += new EventHandler((a, b) =>
            {
                mh.UnHook();
                k_hook.Stop();
            });
            k_hook = new KeyboardHook();
            //k_hook.KeyDownEvent += new System.Windows.Forms.KeyEventHandler(hook_KeyDown);//钩住键按下 
            k_hook.KeyPressEvent += K_hook_KeyPressEvent;
            k_hook.Start();
            nextClipboardViewer = (IntPtr)SetClipboardViewer((int)Handle);
        }
        private void mh_MouseDownEvent(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                LeftTag = true;
                //richTextBox1.AppendText("按下了左键\n");
            }
            if (e.Button == MouseButtons.Right)
            {
                RightTag = true;
                // richTextBox1.AppendText("按下了右键\n");
            }
            p1 = e.Location;
        }
        private void mh_MouseUpEvent(object sender, MouseEventArgs e)
        {
            p2 = e.Location;
            double value = Math.Sqrt(Math.Abs(p1.X - p2.X) * Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y) * Math.Abs(p1.Y - p2.Y));
            if (e.Button == MouseButtons.Left)
            {
                //richTextBox1.AppendText("松开了左键\n");
                if (LeftTag && value == 0)
                {
                    new System.Threading.Timer((a) =>
                    {
                        try
                        {
                            copyMessage();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }, null, 100, 0);
                    label1.Text = (String.Format("[{0},{1}]", e.Location.X, e.Location.Y));
                }
            }

            RightTag = false;
            LeftTag = false;
            p1 = new Point(0, 0);
            p2 = new Point(0, 0);
        }

        private void K_hook_KeyPressEvent(object sender, KeyPressEventArgs e)
        {
            int i = (int)e.KeyChar;
            //MessageBox.Show(i.ToString());
        }

        private const int WM_KEYUP = 0X105;
        private const int WM_KEYDOWN = 0X104;
        private const int WM_A = 0x41;
        private const int WM_C = 0x43;
        private const int WM_CTRL = 0x11;
        private const int BM_CLICK = 0xF5;
        private void copyMessage()
        {
            IntPtr mwh = Win32Api.FindMainWindowHandle(null, "Measurements", 100, 10);
            if (mwh != IntPtr.Zero)
            {
                Point screenPoint = Control.MousePosition;
                int x = screenPoint.X;
                int y = screenPoint.Y;

                IntPtr mwhMain = Win32Api.GetParent(mwh);
                Win32Api.RECT rectMain = new Win32Api.RECT();
                Win32Api.GetWindowRect(mwhMain, ref rectMain);
                Console.WriteLine("{0},{1},{2},{3},{4},{5}", x, y, rectMain.Left, rectMain.Left, rectMain.Top, rectMain.Bottom);
                if (x < rectMain.Left || x > rectMain.Right || y < rectMain.Top || y > rectMain.Bottom)
                {
                    return;
                }

                Win32Api.SetWindowPos(mwh, -1, 0, 0, 0, 0, 1 | 2);
                Win32Api.RECT rect = new Win32Api.RECT();
                Win32Api.GetWindowRect(mwh, ref rect);
                Win32Api.SetActiveWindow(mwh);
                bool fore = Win32Api.SetForegroundWindow(mwh);

                //Win32Api.SetCursorPos((rect.Right + rect.Left) / 2, (rect.Bottom + rect.Top) / 2); //设置鼠标位置
                //Win32Api.mouse_event(0x0002, 0, 0, 0, 0); //鼠标按下
                //Win32Api.mouse_event(0x0004, 0, 0, 0, 0);//鼠标松开

                //Win32Api.SetCursorPos(x, y);

                //Clipboard.Clear();
                //Ctrl+A
                Win32Api.keybd_event(WM_CTRL, 0, 0, 0);
                Win32Api.keybd_event(WM_A, 0, 0, 0);
                Win32Api.keybd_event(WM_CTRL, 0, 2, 0);
                Win32Api.keybd_event(WM_A, 0, 2, 0);
                //Ctrl+C
                Win32Api.keybd_event(WM_CTRL, 0, 0, 0);
                Win32Api.keybd_event(WM_C, 0, 0, 0);
                Win32Api.keybd_event(WM_CTRL, 0, 2, 0);
                Win32Api.keybd_event(WM_C, 0, 2, 0);
            }
        }

        //
        IntPtr nextClipboardViewer;
        /// <summary>
        /// 要处理的 WindowsSystem.Windows.Forms.Message。
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            // defined in winuser.h
            const int WM_DRAWCLIPBOARD = 0x308;
            const int WM_CHANGECBCHAIN = 0x030D;

            switch (m.Msg)
            {
                case WM_DRAWCLIPBOARD:
                    DisplayClipboardData();
                    SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;
                case WM_CHANGECBCHAIN:
                    if (m.WParam == nextClipboardViewer)
                        nextClipboardViewer = m.LParam;
                    else
                        SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        struct Axis
        {
            public double x;
            public double y;
            public double z;
            public bool err;
        }
        /// <summary>
        /// 显示剪贴板内容
        /// </summary>
        public void DisplayClipboardData()
        {
            try
            {
                IDataObject iData = new DataObject();
                iData = Clipboard.GetDataObject();

                if (iData.GetDataPresent(DataFormats.Text))
                {
                    string data = Regex.Replace(((string)iData.GetData(DataFormats.Text)).Trim(), @"\s+", " ");
                    Axis axis = axisParser(data);
                    if (axis.err)
                    {
                        label2.Text = "null";
                    }
                    else
                    {
                        label2.Text = String.Format("坐标 x:{0},y:{1},z:{2}", axis.x, axis.y, axis.z);
                        if (axis.err)
                        {
                            label2.Text = "err";
                            return;
                        }
                        if (radioPoint.Checked)
                        {
                            sendCommand(String.Format("point {0},{1},{2}", axis.x, axis.y, axis.z));
                        }
                        if (radioLine.Checked)
                        {
                            linePoints.Add(axis);
                        }
                    }
                }
                else
                {
                    label2.Text = "";
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        private Axis axisParser(string data)
        {
            string str = data.Trim();
            Axis axis = new Axis() { x = 0, y = 0, z = 0, err = false };
            if (string.IsNullOrEmpty(str))
            {
                axis.err = true;
                return axis;
            }
            Regex reg = new Regex(@"^\w+:\s+([\d.]+)\s+([\d.]+)\s+([\d.]+)m$", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"\s+", " ");
            if (reg.IsMatch(str))
            {
                Match match = reg.Match(str);
                axis.x = double.Parse(match.Groups[1].Value);
                axis.y = double.Parse(match.Groups[2].Value);
                axis.z = double.Parse(match.Groups[3].Value);
            }
            return axis;
        }
        /// <summary>
        /// 关闭程序，从观察链移除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            ChangeClipboardChain(Handle, nextClipboardViewer);
        }

        #region WindowsAPI
        /// <summary>
        /// 将CWnd加入一个窗口链，每当剪贴板的内容发生变化时，就会通知这些窗口
        /// </summary>
        /// <param name="hWndNewViewer">句柄</param>
        /// <returns>返回剪贴板观察器链中下一个窗口的句柄</returns>
        [DllImport("User32.dll")]
        protected static extern int SetClipboardViewer(int hWndNewViewer);

        /// <summary>
        /// 从剪贴板链中移出的窗口句柄
        /// </summary>
        /// <param name="hWndRemove">从剪贴板链中移出的窗口句柄</param>
        /// <param name="hWndNewNext">hWndRemove的下一个在剪贴板链中的窗口句柄</param>
        /// <returns>如果成功，非零;否则为0。</returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        /// <summary>
        /// 将指定的消息发送到一个或多个窗口
        /// </summary>
        /// <param name="hwnd">其窗口程序将接收消息的窗口的句柄</param>
        /// <param name="wMsg">指定被发送的消息</param>
        /// <param name="wParam">指定附加的消息特定信息</param>
        /// <param name="lParam">指定附加的消息特定信息</param>
        /// <returns>消息处理的结果</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
        #endregion

        private void sendCommand(string command)
        {
            if (!command.EndsWith(" "))
            {
                command += " ";
            }
            new System.Threading.Timer((a) =>
            {
                if (acApp == null) return;
                acApp.ActiveDocument.SendCommand(command);
            }, null, 100, 0);
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
            linePoints.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (linePoints.Count < 2)
            {
                return;
            }
            string command = "pl ";
            foreach (Axis item in linePoints)
            {
                command += String.Format("{0},{1} ", item.x, item.y);
            }
            command += "C";
            sendCommand(command);
            linePoints.Clear();
        }
    }
}
