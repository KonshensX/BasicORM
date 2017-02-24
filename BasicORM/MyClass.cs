using System;
using System.Reflection;

namespace BasicORM
{
    class MyClass
    {
        public void DoStuff<T>(T t)
        {
            if (typeof(ITable).IsAssignableFrom(t.GetType()))
            {
                foreach (PropertyInfo property in typeof(T).GetProperties())
                {
                    Console.WriteLine("Property name: {0} -- Property Value: {1}", property.Name, property.GetValue(t, null));
                }
            }
        }
    }
}
