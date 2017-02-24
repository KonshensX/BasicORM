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
            foreach (Post post in postObject.GetAll())
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}", post.ID, post.TableName, post.Title, post.Publisher);


                Console.WriteLine("###################");
            }

            Person person = new Person();

            person.GetAll();

            
            Console.WriteLine("Time elapsed: " + sw.ElapsedMilliseconds + "ms");
            Console.ReadKey();
        }
    }
}
