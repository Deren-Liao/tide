using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EncryptStringSample.StringCipher;
using static System.Console;
using System.IO;

namespace ConsoleApplication2
{
    class Program
    {
        static async Task DoTask()
        {
            if (2 == 2)
            {
                return;
            }
            else
            {
                await Task.Delay(10);
                return;
            }
        }

        struct strA
        {
            public int a;
        }

        static void LinqEmptyTest()
        {
            List<string> l = new List<string>();
            var lq = l.Where(x => x == "aa");
            WriteLine(lq);
        }

        static void ObjectAsString()
        {
            bool a = true;
            object x;
            x = a as object;
            WriteLine(x as string);
            WriteLine(x == null);
            x = 5 as object;
            WriteLine(x as string);
            WriteLine(x == null);
            var b = new strA { a = 5 };
            x = b as object;
            WriteLine(x as string);
            WriteLine(x == null);
            strA c = (strA)x;
            WriteLine(c.a);
            x = a as object;
            c = (strA)x;
        }

        static void Main(string[] args)
        {
            ObjectAsString();
            return;
            //LinqEmptyTest();

            do
            {
                string sourceString = ReadLine();
                if (String.IsNullOrWhiteSpace(sourceString))
                {
                    return;
                }
                WriteLine($"{sourceString.GetHashCode():X}");
            } while (true);

            bool ? a = null;
            if (a == true)
            {
                WriteLine("a is true");
            }

            var task = DoTask();
            task.Wait();


            Program2.Mainx();

            var sp = "".Split(';');

            string plain = "I can see";

            using (var sr = new StreamReader(@"C:\Users\derenl\Documents\tmpa\test.json"))
            {
                plain = sr.ReadToEnd();
            }

            string pass = "pass111";
            string encrypted = Encrypt(plain, pass);

            using (var sw = new StreamWriter(@"C:\Users\derenl\Documents\tmpa\cherry.json", append:false))
            {
                sw.Write(encrypted);
            }


            using (var sr = new StreamReader(@"C:\Users\derenl\Documents\tmpa\cherry.json"))
            {
                encrypted = sr.ReadToEnd();
            }

            string decrypted = Decrypt(encrypted, pass);

            WriteLine(plain);
            WriteLine(encrypted);
            WriteLine(decrypted);
            ReadKey();


        }
    }
}
