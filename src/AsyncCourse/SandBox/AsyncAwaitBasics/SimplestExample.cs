using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBox.AsyncAwaitBasics
{
    public class SimplestExample
    {
        public int Method() => 0;

        public async Task<int> MethodAsync() => 0;

        public async Task<int> OtherMethodAsync() => await MethodAsync();

        public async Task<int> FooAsync()
        {
            return await Task.Run(() =>
            {
                Console.WriteLine("Started foo async");
                for (int i = 0; i < 20; i++)
                {
                    Thread.Sleep(500);
                    Console.WriteLine(i);
                }

                Console.WriteLine("Ended foo async");
                return 0;
            });
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Started run async");
            var task = FooAsync();
            Console.WriteLine("Started waiting");
            Thread.Sleep(2000);
            Console.WriteLine("Ended waiting");
            var result = await task;
            Console.WriteLine("Ended run async");
        }
    }
}
