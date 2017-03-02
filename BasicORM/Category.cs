using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicORM
{
    class Category : Table
    {
        public string Name { get; set; }

        public Category()
        {
            Name = "Cateogory Name";
        }
    }
}
