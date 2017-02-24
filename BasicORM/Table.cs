using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Reflection;

namespace BasicORM
{
    class Table : ITable
    {

        private Database DatabaseObject;
        public int ID { get; set; }

        public string TableName;

        public Table()
        {
            DatabaseObject = new Database();
            TableName = GetTableName();
            ID = GetLastID() + 1;
        }

        public bool Save(ITable table)
        {
            //TODO: Fix the query
            foreach (PropertyInfo property in typeof(ITable).GetProperties())
            {
                Console.WriteLine("Property name: {0} -- Property Value: {1}", property.Name, property.GetValue(table, null));
            }
            return true;
        }

        //TODO
        public IEnumerable<ITable> GetAll()
        {
            string query = "SELECT * FROM " + TableName;

            using (MySqlCommand myCommand = new MySqlCommand(query, this.DatabaseObject.GetConnection()))
            {
                this.DatabaseObject.OpenConnection();
                using (MySqlDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            if (typeof(ITable).IsAssignableFrom(this.GetType()))
                            {
                                foreach (PropertyInfo property in this.GetType().GetProperties())
                                {
                                    property.SetValue(null, myReader[property.Name.ToLower()]);
                                }
                            }
                            //Iterate thorugh the properties of this

                            yield return this;
                        }
                    }
                }
            }
        }


        //TODO
        public ITable Get(int ID)
        {
            string query = "SELECT * FROM " + TableName + " WHERE id = @id";
            using (MySqlCommand myCommand = new MySqlCommand(query, this.DatabaseObject.GetConnection()))
            {
                this.DatabaseObject.OpenConnection();
                myCommand.Parameters.AddWithValue("@id", ID);

                using (MySqlDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            if (typeof(ITable).IsAssignableFrom(this.GetType()))
                            {
                                foreach (PropertyInfo property in this.GetType().GetProperties())
                                {
                                    Console.WriteLine(property.Name + " From the data reader " + myReader[property.Name.ToLower()]);
                                }
                            }
                        }
                    }
                }
                
            }

            Console.WriteLine("This is the ID ou entered: {0}", ID);

            return this;
        }

        public bool Update(ITable table)
        {
            throw new NotImplementedException();
        }

        public bool Remove(ITable table)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the last ID in the table
        /// </summary>
        /// <returns></returns>
        public int GetLastID()
        {
            //TODO : Fix the query to get the tables name 
            string query = "SELECT MAX(id) FROM " + TableName;

            using (MySqlCommand myCommand = new MySqlCommand(query, this.DatabaseObject.GetConnection()))
            {
                this.DatabaseObject.OpenConnection();
                using (MySqlDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            if (myReader[0] is DBNull)
                                return 0;
                            return Convert.ToInt32(myReader[0]);
                        }
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Gets the table name based on the current class name
        /// </summary>
        public string GetTableName()
        {
            return this.GetType().Name.ToLower() + "s";
        }

        /// <summary>
        /// This generates the tables in the database based on the Class properties 
        /// </summary>
        /// <param name="myType"></param>
        public void CreateEntity<T>(T t)
        {
            if (typeof(ITable).IsAssignableFrom(t.GetType()))
            {
                foreach (PropertyInfo property in typeof(ITable).GetProperties())
                {
                    Console.WriteLine("Property name: {0} -- Property Value: {1}", property.Name, property.GetValue(t, null));
                }
            }
            
        }
    }
}
