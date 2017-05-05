using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace credential_helper
{
    public class appendlog
    {
        private string _file;

        public appendlog(string file)
        {
            _file = file;
        }

        public void Append(string text)
        {
            using (var sw = new StreamWriter(_file, append: true))
            {
                string line = $"{DateTime.Now:O} {text}";
                sw.WriteLine(line);
            }
        }
    }
}
