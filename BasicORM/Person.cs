using System;
namespace BasicORM
{
    class Person : Table
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CIN { get; set; }

        public int Age { get; set; }


    }
}
