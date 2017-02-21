using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WriteTraceToFile
{
    public static class TextTrace
    {
        static TextTrace()
        {
            string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            s_root = Path.GetDirectoryName(path);
            s_path = Path.Combine(s_root, TraceFile);
        }

        private const string TraceFile = @"my_test_trace.txt";
        private static readonly string s_root;
        private static readonly string s_path;
        private static readonly object s_lockObj = new object();


        public static void TestTrace(string message)
        {
            lock (s_lockObj)
            {
                using (var w = new StreamWriter(s_path, append: true))
                {
                    w.WriteLine(message);
                }
            }
        }
    }
}
