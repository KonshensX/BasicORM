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

            Person personObject = new Person();

            personObject.Get(1);

            personObject.Update();

            personObject.Remove();

            personObject.GetAll();

            personObject.Save();

            
            Console.WriteLine("Time elapsed: " + sw.ElapsedMilliseconds + "ms");
            Console.ReadKey();
        }
    }
}
