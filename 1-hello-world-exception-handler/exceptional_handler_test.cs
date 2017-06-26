using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleCloudSamplesEReport
{
    public static class exceptional_handler_test
    {
        public static void TestThrow()
        {
            throw new ArgumentNullException("Test exception handler");
        }
    }
}