using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

using Google.Apis.Auth.OAuth2;

namespace GoogAuthTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = GoogleCredential.GetApplicationDefaultAsync();
            t.Wait();
            var credential = t.Result;
            WriteLine($"Google default application credential is null? {credential == null}");
            ReadKey();
        }
    }
}
