using System;

namespace BasicORM
{
    class Post : Table
    {
        public string Title { get; set; }
        public string Publisher { get; set; }

        public Category Category { get; set; }

        public Post()
        {
            Title = "Empty title";
            Publisher = "No publisher";
        }
    }
}
