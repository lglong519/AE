using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Test
{

    class Program
    {
        delegate int Del(int a);
        int? n = 0;
        public void cb(object a)
        {
            Console.WriteLine("timer a {0}, n {1}", a, n++);
        }
        [DllImport("psapi.dll")]
        static extern int EmptyWorkingSet(IntPtr hwProc);
        public static void Main(string[] args)
        {
            /*
            var s = new S();
            s.Out("O");
            IIfc ifc = (IIfc)s;
            ifc.Innt(1);
            string st = s;
            Console.WriteLine(string.Concat(st, "123"));
            Console.WriteLine(st.Contains("1"));
            Console.WriteLine(st.Insert(2,"nn"));
            var p=new Program();
            p.h();
            Del del = x => x + 1;
            int[] arr = { 41,4,5,6,8};
            IEnumerator enumerator = arr.GetEnumerator();
            enumerator.MoveNext();
            enumerator.MoveNext();
            XDocument dom = new XDocument(new XElement("text", "Glen"));
            Console.WriteLine("cc{0}{1}", enumerator.Current,dom);
            //for(int i=0;i<100;i++){Console.WriteLine(i);}
            Parallel.For(0, 10, i => { Console.WriteLine(i); });
            Timer timer = new Timer(p.cb, 123, 1000, 0);
            //timer.Dispose();
            //DateTime date = DateTime.Now;
            DateTime date = DateTime.Parse("2018-05-15");
            //DateTime date = DateTime.Today;
            date = DateTime.FromFileTime(45644646);
            Console.WriteLine(date.ToString());
            */
            Console.WriteLine(Math.Round(1.1534848, 2).ToString());
            Console.WriteLine(Math.Round(1.5).ToString());
            Console.WriteLine(Math.Round(-1.5).ToString());
            Console.WriteLine(Math.Round(-2.5).ToString());
            Console.WriteLine((Math.Asin(0.5) * 180 / Math.PI).ToString());
            DateTime now = DateTime.Now;
            Console.WriteLine(now.ToString().Replace("/", "-"));
            //Console.WriteLine(now.Kind);
            //Console.WriteLine(now.Year);
            //Console.WriteLine(now.Date);
            //Console.WriteLine(now.Day);
            //Console.WriteLine(now.DayOfWeek);
            //Console.WriteLine(now.DayOfYear);
            //Console.WriteLine(now.TimeOfDay);
            //Console.WriteLine(now.Month);
            //Console.WriteLine(now.Hour);
            //Console.WriteLine(now.Minute);
            //Console.WriteLine(now.Millisecond);
            //Console.WriteLine(now.Second);
            //Console.WriteLine(now.Ticks);
            //Console.WriteLine(now.ToFileTime());
            //Console.WriteLine(now.ToFileTimeUtc());
            //Console.WriteLine(now.ToLocalTime());
            //Console.WriteLine(now.ToLongDateString());
            //Console.WriteLine(now.ToLongTimeString());
            //Console.WriteLine(now.ToOADate());
            //Console.WriteLine(now.ToShortDateString());
            //Console.WriteLine(now.ToShortTimeString());
            //Console.WriteLine(now.ToUniversalTime());
            /*
            try
            {
                string databaseName = System.Configuration.ConfigurationManager.AppSettings["NodeServer"];
                //string databaseName = ConfigurationManager.ConnectionStrings["NodeServer"].ConnectionString.ToString();
                string ur = databaseName + url;
                WebRequest request = WebRequest.Create(ur);
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;

                //Uri uri = new Uri(ur);
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);//此处只是URL不带参数
                //request.Method = "POST";
                //byte[] buffer = Encoding.ASCII.GetBytes(param);//requestString是你的参数
                //request.ContentLength = buffer.Length;//设置数据长度
                //Stream stream = request.GetRequestStream();
                //stream.Write(buffer, 0, buffer.Length);
                //stream.Close();

                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Display the status.
                Console.WriteLine(response.StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                Console.WriteLine(responseFromServer);
                // Cleanup the streams and the response.
                reader.Close();
                dataStream.Close();
                response.Close();
                return responseFromServer;
            }
            catch (Exception e)
            {
                return;
            }
            */
            string b = Dns.GetHostName();
            string d = Dns.GetHostEntry("localhost").HostName;
            Console.WriteLine(b + ":" + d);
            string bb = " ";
            string cc = string.Empty;
            if (string.IsNullOrWhiteSpace(bb))
            {
                Console.WriteLine("IsNullOrWhiteSpace:bb");
            }
            if (string.IsNullOrEmpty(bb))
            {
                Console.WriteLine("IsNullOrEmpty:bb");
            }
            if (string.IsNullOrWhiteSpace(cc))
            {
                Console.WriteLine("IsNullOrWhiteSpace:cc");
            }
            if (string.IsNullOrEmpty(cc))
            {
                Console.WriteLine("IsNullOrEmpty:cc");
            }
            Regex reg = new Regex("{h1}|{h2}|{p13}|{p23}|{p15}|{p25}", RegexOptions.IgnoreCase);
            MatchCollection collection = reg.Matches("{h1}fdsaf|{h2}fdsaf|{pfdsaf13}|{pfdas23}|{p1fsa5}|{p25}fda");
            if (collection.Count > 0)
            {
            }
            foreach (var item in collection)
            {
                Console.WriteLine(item.ToString());
            }
            //Console.WriteLine(double.Parse("405502.13164708368"));
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //Process[] processes = Process.GetProcesses();
            //foreach (Process item in processes)
            //{
            //    try
            //    {
            //        EmptyWorkingSet(item.Handle);
            //    }
            //    catch (Exception)
            //    {
            //    }
            //}
            int part =3;
            div(10, part);
            div(50, part);
            div(460, part);
            div(5555, part);
            div(13500, part);
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            Console.ReadLine();
        }
        public void h()
        {
            f();
        }
        public void f()
        { }
        public static void div(int featureCount, int part)
        {
            int div=featureCount / part;
            if (div < 10)
            {
                Console.WriteLine(div);
                return;
            }
            if (div < 100)
            {
                int section = div - (div % 10);
                Console.WriteLine(part.ToString() + ":" + section);
                return;
            }
            if (div < 1000)
            {
                int section = div - (div % 100);
                Console.WriteLine(part.ToString() + ":" + section);
                return;
            }
            if (div < 10000)
            {
                int section = div - (div % 1000);
                Console.WriteLine(part.ToString() + ":" + section);
                return;
            }
        }
    }
    interface IIfc
    {
        string Out(string str);
        int Innt(int num);
    }
    partial class S : IIfc
    {
        int n = 10;
        public string Out(string str)
        {
            const float num = 2.80F;
            int stt = ((int)num);
            return stt + num + str;
        }
        int IIfc.Innt(int num)
        {
            int a = 2 + s;
            return ++a + num;
        }
        public static implicit operator string(S s)
        {
            return Convert.ToString(s.n);
        }
    }
    partial class S
    {
        int s = 1;
    }
}
