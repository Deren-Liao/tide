using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AwaitTask
{
    class Program
    {
        static void ThrowExceptionAfter(TimeSpan timespan)
        {
            Thread.Sleep(timespan);
            throw new Exception("From ThrowExceptionAfter");
        }

        static async void AsyncCaller()
        {
            try
            {
                await Task.Run(() => ThrowExceptionAfter(TimeSpan.FromSeconds(2)));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
            }
        }

        static async void AsyncCallerNoExceptionHandler()
        {
            Task.Run(() => ThrowExceptionAfter(TimeSpan.FromSeconds(2)));
        }


        static async Task HereIsATask()
        {
            Console.WriteLine("HereIsATask is executed");
            await Task.Delay(500);
        }


        static void Main(string[] args)
        {
            //AsyncCaller();

            AsyncCallerNoExceptionHandler();

            Task aTask = HereIsATask();

            ConsoleKey k;
            while ((k = Console.ReadKey().Key) != ConsoleKey.X)
            {
                Console.WriteLine(k);
            }
        }
    }
}
