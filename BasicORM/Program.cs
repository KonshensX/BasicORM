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

            Post postObject = new Post();
            //postObject.Get(1);

            foreach (Post item in postObject.GetAll())
            {
                Console.WriteLine("Title: {0}, Category: {1}", item.Title, item.Category.Name);
            }
            //Category categoryObject = new Category();
            //Console.WriteLine("Type is: {0}", categoryObject.GetType());
            //Console.WriteLine(typeof(ITable).IsAssignableFrom(categoryObject.GetType()));
            Console.WriteLine("Time elapsed: " + sw.ElapsedMilliseconds + "ms");
            Console.ReadKey();
        }

        

    }
}
