using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using log4net;

namespace Private_Gax_Log4Net_AspNetMvc
{
    public class Class1JustTime
    {
        public static void WriteLog(ILog logger)
        {
            logger.Error("This is not error. This is correct. Monday.");
        }
    }
}