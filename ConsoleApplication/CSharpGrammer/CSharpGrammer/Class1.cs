using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpGrammer
{
    public class Class1
    {
        public string _s1;
        public string TestStringLambda => $"test with s1 {_s1}";

        public string TestStringGet { get; } 

        public Class1(string s)
        {
            _s1 = s;
            TestStringGet = $"test with s1 {_s1}";
        }
    }
}
