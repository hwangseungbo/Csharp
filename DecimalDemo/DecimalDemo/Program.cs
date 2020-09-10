using System;
using System.Net.Http;

namespace DecimalDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            decimal d = 12.34m;

            Console.WriteLine(d);

            decimal.TryParse("12.3456", out d);

            Console.WriteLine(d);
        }
    }
}
