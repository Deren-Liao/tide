using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class AsyncTask
    {
        public static async Task StartTask()
        {
            await Task.Delay(1);
            Console.WriteLine("StartTask");
        }

        public static async void runTaskTest()
        {
            Task t = StartTask();
            Func<Task> tFunc = () => t;
            for (int i = 0; i < 10; ++i)
            {
                // This execute StartTask once
                await tFunc();
            }

            Console.WriteLine("after .... ");

            Func<Task> tFunc2 = StartTask;
            for (int i = 0; i < 5; ++i)
            {
                await tFunc2();
            }
        }
    }
}
