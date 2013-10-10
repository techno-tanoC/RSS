using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var tes = new DelegateTest();
            tes.d += a;
            tes.Test();
            Console.ReadLine();
        }
        static void a()
        {
            Console.WriteLine("hello");
        }
    }

    class DelegateTest
    {
        public delegate void del();
        public event del d;
        public void Test()
        {
            if (d != null)
                d();
        }
    }
}
