using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using static GoogleCloudExtension.StackdriverLogsViewer.LoggerTooltipSource;

namespace ConsoleApplication1
{
    public static class Lazy
    {
        public static void TestLazyIsValueCreated()
        {
            Lazy<List<string>> lazy = new Lazy<List<string>>();
            WriteLine($"lazy.IsValueCreated {lazy.IsValueCreated}");

            WriteLine($"{TooltipSource.SourceLine}");

        }
    }
}
