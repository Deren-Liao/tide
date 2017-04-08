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
        static void Main(string[] args)
        {
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
