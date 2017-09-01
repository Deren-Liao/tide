using System;
using System.Threading.Tasks;

namespace GoogleCloudFunctions.ClassLibrary1
{
    delegate Task Function(UserEvent data);

    public class UserEvent
    {
        public string E1 { get; set; }
        public int E2 { get; set; }
        public bool E3 { get; set; }
    }

    public class DataType
    {
        public string F1 { get; set; }
        public int F2 { get; set; }

        public UserEvent F3 { get; set; }
    }

    public class Class1
    {
        //public static Task ProcessEvent(string data)
        //{
        //    Console.WriteLine($"string: {data}");
        //    return Task.FromResult(0);
        //}

        //public static Task ProcessEvent(int data)
        //{
        //    Console.WriteLine($"int: {data}");
        //    return Task.FromResult(0);
        //}

        //public static Task ProcessEvent(DataType data)
        //{
        //    Console.WriteLine($"DataType: {data}");
        //    return Task.FromResult(0);
        //}

        public static Task ProcessEvent(UserEvent data)
        {
            Console.WriteLine($"UserEvent: {data}");
            return Task.FromResult(0);
        }
    }
}
