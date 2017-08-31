using System;
using System.Reflection;
using Newtonsoft.Json;
using System.IO;
using System.ComponentModel;
namespace Reflection
{
    class Program
    {


        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string text = File.ReadAllText("embed-type.json");
            dynamic json = JsonConvert.DeserializeObject(text);
            Console.WriteLine($"{json.data}");
            var data = json.data;


            Assembly asm1 = Assembly.LoadFile(@"C:\git\tide\DotNetCore\Reflection\ClassLibrary1\bin\Debug\netcoreapp2.0\ClassLibrary1.dll");
            Console.WriteLine($"{asm1.FullName}");

            Type t = asm1.GetType("GoogleCloudFunctions.ClassLibrary1.Class1");
            MethodInfo mInfo;
            try
            {
                mInfo = t.GetMethod("ProcessEvent");
            }
            catch (AmbiguousMatchException)
            {
                Console.Error.WriteLine($"Got AmbiguousMatchException.  More than one method is found with the specified name and matching the specified binding constraints.");
                while (Console.ReadKey().Key != ConsoleKey.C) { }
                return;
            }

            var pars = mInfo.GetParameters();
            if (pars.Length != 1)
            {
                Console.Error.WriteLine($"Expect to have only one input parameters.");
                while (Console.ReadKey().Key != ConsoleKey.C) { }
                return;
            }

            ParameterInfo pInfo = pars[0];
            Type propType = pInfo.ParameterType;
            object propValue = null;
            if (data.GetType() == typeof(string))
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(propType);
                propValue = typeConverter.ConvertFromString(data);
            }
            else
            {
                propValue = JsonConvert.DeserializeObject(data.ToString(), pInfo.ParameterType);
            }

            mInfo.Invoke(null, new object[] { propValue });

            while (Console.ReadKey().Key != ConsoleKey.C) { }
        }
    }
}
