using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Threading;

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
        public static void Main(string[] args)
        {
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
            Console.ReadLine();
        }
        public void h()
        {
            f();
        }
        public void f()
        { }
    }
    interface IIfc
    {
        string Out(string str);
        int Innt(int num);
    }
    class S : IIfc
    {
        int n = 10;
        public string Out(string str)
        {
            const float num = 2.80F;
            int stt = ((int)num);
            return stt+num + str;
        }
        int IIfc.Innt(int num)
        {
            int a = 2;
            return ++a + num;
        }
        public static implicit operator string(S s)
        {
            return Convert.ToString(s.n);
        }
    }
}
