using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackdriverLogging
{
    static class GenerateLogSample
    {
        static Random rand = new Random();

        public static void WriteRandomSeverityLog(ILog log)
        {
            string message = RandomString.PickMessage();
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
                    log.Info(RandomString.Next());
                    break;
            }
        }

        public static void WriteException(ILog logger)
        {
            logger.Error(RandomString.Next(), new Exception(RandomString.Next()));
        }

        static void Main(string[] args)
        {
            XmlConfigurator.Configure(File.OpenRead("log4net.config.xml"));
            ILog log = LogManager.GetLogger(typeof(GenerateLogSample));

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