using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{

    public class Prop
    {
        public string text = " text";
        public Prop()
        {
            Console.WriteLine("Init Prop");
        }
    }

    public class AutoPropertyInitializer
    {
        public static Prop PropObj { get; } = new Prop();
    }
}
