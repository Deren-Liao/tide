using System;
using System.Diagnostics;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Stopwatch watch = Stopwatch.StartNew();

            Console.WriteLine();
            Console.WriteLine("Press C to stop");

            Console.WriteLine($"watch.Elapsed:{watch.Elapsed}");

            while (Console.ReadKey().Key != ConsoleKey.C) { }
        }
    }
}