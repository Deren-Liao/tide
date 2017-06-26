using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleCloudSamplesEReport
{
    public static class EReport
    {
        public static void TestXReport()
        {
            var ex = ExceptionGenerator.GenerateException(
                () => ExceptionGenerator.LoopThenException(3, "Error Reporting inner exception test"));
            throw new ErrorReportingApiTestException("Test Error Repoting API", ex);
        }
    }
}