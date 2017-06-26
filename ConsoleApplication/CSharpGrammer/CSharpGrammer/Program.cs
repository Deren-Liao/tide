using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpGrammer
{
    class Program
    {
        static void Main(string[] args)
        {

            Class1 A = new Class1("abc");
            for (int i = 0; i < 5; ++i)
            {
                A._s1 += i.ToString();
                Console.WriteLine(A.TestStringGet);


                string ss = null;
                Console.WriteLine(ss?.Contains("abc") != true);
                ss = "babc";
                Console.WriteLine(ss?.Contains("abc") != true);
            }
        }
    }
}
