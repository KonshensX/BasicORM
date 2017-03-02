using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;

namespace BasicORM
{
    class Program
    {
        static void Main(string[] args)
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();

            //Post postObject = new Post();
            //postObject.Get(1);

            Category categoryObject = new Category();
            Console.WriteLine("Type is: {0}", categoryObject.GetType());
            
            Console.WriteLine("Time elapsed: " + sw.ElapsedMilliseconds + "ms");
            Console.ReadKey();
        }
    }
}
