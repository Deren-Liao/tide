using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackdriverLogging
{
    public static class RandomString
    {
        static Random rand = new Random();

        static readonly string[] sampleLogMessages = new string[]
        {
        "Shopping Apple IPhone app",

        "Android version timer",

        "Call BuyABook with argument FUN",

        "Contact Database for indexes"

        };


        public static string PickMessage()
        {

            int idx = rand.Next(sampleLogMessages.Length);

            return sampleLogMessages[idx];

        }

        public static string Next()
        {
            const string chars = "good like favour open detail for more logs you do";
            var splits = chars.Split();
            List<string> list = new List<string>();
            for (int i = 0; i < rand.Next(5, 15); ++i)
            {
                int idx = rand.Next(splits.Length);
                list.Add(splits[idx]);
            }

            return string.Join(" ", list);
        }
    }
}
