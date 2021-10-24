using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryPattern.Extensions
{
    public static class StringExtensions
    {
        public static string Dump(this string value)
        {
            Console.WriteLine(value);

            return value;
        }
    }
}
