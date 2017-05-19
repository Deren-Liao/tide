using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.IO;

namespace ReflectionTest
{
    public class IsVsSolutionOptionAttribute : Attribute
    {
        public string KeyName { get; }
        public IsVsSolutionOptionAttribute(string uniqueKeyName)
        {
            KeyName = uniqueKeyName;
        }
    }
    
    public class DebuggerOptions
    {
        [IsVsSolutionOption(")]
        public string StringOption { get; set; }
        [IsVsSolutionOption]
        public string StringOption2 { get; set; }
        [IsVsSolutionOption]
        public int IntOption { get; set; }
        [IsVsSolutionOption]
        public bool BoolOption { get; set; }

        [IsVsSolutionOption]
        public string StringOptionNoSet { get; private set; }
        [IsVsSolutionOption]
        public int IntOptionNoGet { private get; set; }

        public string StringNotOption { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            DebuggerOptions options = new DebuggerOptions()
            {
                StringOption = "this is a string",
                BoolOption = true,
                IntOption = 123,
                StringOption2 = "second string"
            };
            var dict = GetAuthors(options);

            using (var sw = new StreamWriter(@"c:\tmp\writerTest.txt", append: false))
            {

                foreach (var kv in dict)
                {
                    Console.WriteLine($"{options.GetType().FullName}.{kv.Key}");
                    sw.WriteLine(kv.Value.GetValue(options));
                }
            }

            DebuggerOptions newOptions = new DebuggerOptions();

            using (var sr = new StreamReader(@"c:\tmp\writerTest.txt"))
            {
                foreach (var kv in dict)
                {
                    var value = sr.ReadLine();
                    kv.Value.SetValue(newOptions, value);
                }
            }
        }

        //public string EscapeLineEndings(string s)
        //{
        //    List<char> charList = new List<char>();
        //    //var subs = s.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        //    foreach (char c in s)
        //    {
        //        if (c == '\\')
        //        {
        //            charList.Add('\\');
        //            charList.Add('\\');
        //        }
        //        else
        //        {
        //            charList.Add(c);
        //        }
        //    }
        //    return (new string(charList.ToArray())).Replace(Environment.NewLine, @"\\n");
        //}

        //public string DeEscapeLineEndings(string s)
        //{

        //}

        public static Dictionary<string, PropertyInfo> GetAuthors(object obj)
        {
            Dictionary<string, PropertyInfo> _dict = new Dictionary<string, PropertyInfo>();

            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => Attribute.IsDefined(prop, typeof(IsVsSolutionOptionAttribute)) &&
                prop.CanRead && prop.CanWrite &&
                prop.GetGetMethod(nonPublic: false) != null &&
                prop.GetSetMethod(nonPublic:false) != null &&
                prop.PropertyType == typeof(string)).ToList();

            // PropertyInfo[] props = obj.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                var optionAttribute = prop.GetCustomAttribute<IsVsSolutionOptionAttribute>();
                _dict.Add(optionAttribute.KeyName, prop);
            }

            return _dict;
        }
    }
}
