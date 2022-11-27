using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBox.TaskAPI
{
    public class ChildTask
    {
        public void ExecuteDetached()
        {
            var parent = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Parent running");
                var child = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Child started");
                    Thread.Sleep(2000);
                    Console.WriteLine("Child ended");
                });
                Console.WriteLine("Parent ended");
            });

            parent.Wait();
            Console.WriteLine("The end");
            Console.ReadLine();
        }

        public void ExecuteAttached()
        {
            var parent = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Running parent");
                var child = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Child started");
                    Thread.Sleep(2000);
                    Console.WriteLine("Child ended");
                }, TaskCreationOptions.AttachedToParent);
                Console.WriteLine("Parent ended");
            });

            parent.Wait();
            Console.WriteLine("The end");
            Console.ReadLine();
        }
    }
}
