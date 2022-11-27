﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBox.ExtensionMethods
{
    public static class StringExtensions
    {
        public static void DumpThread(this string value)
        {
            Console.WriteLine($"[{DateTime.Now:hh:mm:ss.fff}] {value}: TID:{Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
