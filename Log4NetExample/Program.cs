using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4NetSample
{
    class Program
    {
        static Random rand = new Random();



        static readonly string[] sampleLogMessages = new string[]
        {
            "Shopping Apple IPhone app",

            "Android version timer",

            "Call BuyABook with argument FUN",

            "Contact Database for indexes"

        };


        static string PickMessage()
        {

            int idx = rand.Next(sampleLogMessages.Length);

            return sampleLogMessages[idx];

        }


        static void WriteRandomSeverityLog(ILog log)
        {
            string message = PickMessage();
            switch (rand.Next(6))
            {
                case 0:
                    log.Info(message);
                    break;
                case 1:
                    log.Debug(message);
                    break;
                case 2:
                    log.Error(message);
                    break;
                case 3:
                    log.Fatal(message);
                    break;
                case 4:
                    log.Warn(message);
                    break;
                case 5:
                    log.Info(RandomString());
                    break;
            }
        }

        public static string RandomString()
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

        private static void WriteException(ILog logger)
        {
            logger.Error(RandomString(), new Exception(RandomString()));
        }

        static void Main(string[] args)
        {
            XmlConfigurator.Configure(File.OpenRead("log4net.config.xml"));
            ILog log = LogManager.GetLogger(typeof(Program));

            while (true)
            {
                System.Threading.Thread.Sleep(millisecondsTimeout: 1000);
                using (log4net.NDC.Push(Guid.NewGuid().ToString()))
                {
                    WriteRandomSeverityLog(log);
                }
            }
        }
    }
}
