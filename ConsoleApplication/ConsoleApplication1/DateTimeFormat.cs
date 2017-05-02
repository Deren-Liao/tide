using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public static class DateTimeFormat
    {
        public static void ToLocalCultureLocalTime()
        {
            var now = DateTime.UtcNow;
            Console.WriteLine(now.ToString("F"));
            string s = now.ToLocalTime().ToString("F");
            Console.WriteLine(s);
            Console.WriteLine($"f format, { now.ToLocalTime().ToString("f")}");
        }
    }
}
