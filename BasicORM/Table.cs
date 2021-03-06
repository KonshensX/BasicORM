﻿using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Reflection;
using System.Diagnostics;

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

        /// <summary>
        /// Save the record
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string myQuery = this.GenerateInsertQuery();

            using (MySqlCommand myCommand = new MySqlCommand(myQuery, this.DatabaseObject.GetConnection()))
            {
                this.DatabaseObject.OpenConnection();
                foreach (PropertyInfo property in this.GetType().GetProperties())
                {
                    myCommand.Parameters.AddWithValue(("@" + property.Name.ToLower()), property.GetValue(this, null));

                }

                var result = (myCommand.ExecuteNonQuery() == 1)? true : false;
            }

            
            sw.Stop();
            
            Console.WriteLine("Time Elapsed: {0} ms", sw.ElapsedMilliseconds);
            return true;
        }

        /// <summary>
        /// Get all records 
        /// </summary>
        /// <returns></returns>
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
                                    if (this.InterfaceImplentationChecker(property))
                                    {
                                        // TODO: 
                                        /**/
                                        // Get the constructor and create an instance of MagicClass 
                                        ConstructorInfo classConstructor = property.PropertyType.GetConstructor(Type.EmptyTypes);
                                        object magicClassObject = classConstructor.Invoke(new object[]{});

                                        // Get the Get method and invoke with a parameter value from the reader
                                        MethodInfo magicMethod = property.PropertyType.GetMethod("Get");

                                        // Get the value from the reader
                                        string name = property.Name.ToLower();
                                        int foreignId = (int)myReader[String.Format("{0}_id", name)];
                                        // This is what'what's returned from the function 
                                        // I need to pass the value from the reader
                                        object magicValue = magicMethod.Invoke(magicClassObject, new object[]{foreignId});

                                        // Set the value to the property 
                                        property.SetValue(this, magicValue);
                                        
                                    }
                                    else
                                    {
                                        //TODO: Set the value of the properties. DONE!!!!
                                        property.SetValue(this, myReader[property.Name.ToLower()]);
                                    }
                                }
                            }
                            yield return this;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the object coresponding to the given ID
        /// </summary>
        /// <param name="ID">ID of the record</param>
        /// <returns></returns>
        public ITable Get(int ID)
        {
            //TODO: Working on foreign keys 
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
                                // I don't think this can go to the function
                                foreach (PropertyInfo property in this.GetType().GetProperties())
                                {
                                    
                                    if (this.InterfaceImplentationChecker(property))
                                    {
                                        // The property implements the ITable interface
                                        // Get the foreign ID from the database 

                                        // Get the item from the database 
                                    }
                                    else
                                    {
                                        // Get the data directly from the database and set it to the property
                                        property.SetValue(this, myReader[property.Name.ToLower()]);
                                    }
                                }
                            }
                        }
                    }
                }
                
            }
            return this;
        }
        /// <summary>
        /// Update the record
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            string myQuery = this.GenerateUpdateQuery();
            Console.WriteLine(myQuery);

            using (MySqlCommand myCommand = new MySqlCommand(myQuery, this.DatabaseObject.GetConnection()))
            {
                this.DatabaseObject.OpenConnection();
                foreach (PropertyInfo property in this.GetType().GetProperties())
                {
                    myCommand.Parameters.AddWithValue(String.Format("@{0}", property.Name.ToLower()), property.GetValue(this, null));
                }

                return (myCommand.ExecuteNonQuery() == 1) ? true : false;
            }
        }

        /// <summary>
        /// Remove the record 
        /// </summary>
        /// <returns></returns>
        public bool Remove()
        {
            string myQuery = this.GenerateDeleteQuery();

            using (MySqlCommand myCommand = new MySqlCommand(myQuery, this.DatabaseObject.GetConnection()))
            {
                this.DatabaseObject.OpenConnection();
                myCommand.Parameters.AddWithValue("@id", this.GetType().GetProperty("ID").GetValue(this, null));

                return (myCommand.ExecuteNonQuery() == 1) ? true : false;
            }
        }

        /// <summary>
        /// Gets the last ID in the records
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
            var name = this.GetType().Name;
            //Check if the last char is an y.
            if (name[name.Length - 1] == 'y')
            {
                
                return name.TrimEnd('y').ToLower() + "ies";
            }
            //Generate the last name
            return name.ToLower() + "s";
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

        /// <summary>
        /// Generates the insert query for the object
        /// </summary>
        /// <returns></returns>
        private string GenerateInsertQuery()
        {
            //TODO: Generate the query first
            string myQuery = "INSERT INTO " + TableName + " (";
            int propertiesArrayLength = this.GetType().GetProperties().GetLength(0);

            for (int count = 0; count < propertiesArrayLength; count++)
            {
                myQuery += ("`" + this.GetType().GetProperties()[count].Name.ToLower() + "`");
                if (count != propertiesArrayLength - 1)
                    myQuery += ",";
            }

            myQuery += ") VALUES (";

            for (int count = 0; count < propertiesArrayLength; count++)
            {
                myQuery += ("@" + this.GetType().GetProperties()[count].Name.ToLower());
                if (count != propertiesArrayLength - 1)
                    myQuery += ",";
            }
            return myQuery += ");";
        }

        private string GenerateDeleteQuery()
        {
            string myQuery = String.Format("DELETE FROM {0} WHERE id = @id", this.TableName);

            return myQuery;
        }

        private string GenerateUpdateQuery()
        {
            string myQuery = "UPDATE " + TableName + " SET ";
            int propertiesArrayLength = this.GetType().GetProperties().GetLength(0);

            for (int count = 0; count < propertiesArrayLength; count++)
            {
                if (this.GetType().GetProperties()[count].Name == "ID")
                    continue;

                var tempName = this.GetType().GetProperties()[count].Name.ToLower();
                myQuery += String.Format("{0} = @{1}", tempName, tempName);
                if (count != propertiesArrayLength - 2)
                    myQuery += ", ";
            }
            return myQuery += " WHERE id = @id";
        }

        private static bool InterfaceFilter(Type typeObject, Object critereaObject)
        {
            if (typeObject.ToString() == critereaObject.ToString())
                return true;
            return false;
        }

        /// <summary>
        /// Checks if the property implements the ITable interface
        /// </summary>
        /// <param name="property">the property that will be checked</param>
        /// <returns>Boolean</returns>
        private bool InterfaceImplentationChecker(PropertyInfo property)
        {
            String myInterfaceString = typeof(ITable).FullName;
             
            Type[] myInterfaces = property.PropertyType.FindInterfaces
                (
                    new TypeFilter(InterfaceFilter),
                    myInterfaceString
                ); 

            return myInterfaces.Length > 0;
        }
    }
}
