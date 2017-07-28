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

            foreach (PropertyInfo property in postObject.GetType().GetProperties())
            {
                // This is the type i'm trynna check against
                Type myType = property.PropertyType;
                TypeFilter myFilter = new TypeFilter(InterfaceFilter);
                String[] myInterfaceList = new String[1] { typeof(ITable).FullName };

                // This is the interface fullname to check if it exists or not
                String interfaceName = typeof(ITable).FullName;

                for (int index = 0; index < myInterfaceList.Length; index++)
                {
                    Type[] myInterfaces = myType.FindInterfaces(myFilter,
                        myInterfaceList[index]);
                    if (myInterfaces.Length > 0)
                    {
                        // The property of type ITable
                        // Get the foreign ID from the database 
                        
                        // Get the item from the database 
                    }
                    else
                        Console.WriteLine(
                            "\n{0} does not implement the interface {1}.",
                            myType, myInterfaceList[index]);
                }

                Console.WriteLine("###############");
            }

            //Category categoryObject = new Category();
            //Console.WriteLine("Type is: {0}", categoryObject.GetType());
            //Console.WriteLine(typeof(ITable).IsAssignableFrom(categoryObject.GetType()));
            Console.WriteLine("Time elapsed: " + sw.ElapsedMilliseconds + "ms");
            Console.ReadKey();
        }

        

    }
}
