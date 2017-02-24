using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace BasicORM
{
    class Database
    {
        private const int _MAX_POOL_SIZE = 100;
        private MySqlConnection Connection;
        private string Server;
        private string DatabaseName;
        private int Port;
        private string UId;
        private string Password;

        public Database()
        {
            try
            {
                this.Server = "localhost"; //Local Server 
                this.UId = "root";
                this.Password = "";
                this.Port = 3306;
                this.DatabaseName = "basicorm";
                string connectionString = "Server=" + Server + ";Port= " + Port + ";" + "Database=" + DatabaseName + ";" + "Uid=" + UId + ";" + "Pwd=" + Password + ";Max Pool Size= " + _MAX_POOL_SIZE;

                this.Connection = new MySqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }


        #region Methodes
        //open the connection
        public bool OpenConnection()
        {
            try
            {
                if (!(this.Connection.State == System.Data.ConnectionState.Open))
                {
                    this.Connection.Open();
                }
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server, Contact admin");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid credentials, please try again");
                        break;

                }
                return false;
            }
        }

        //Close the connection

        public bool CloseConnection()
        {
            try
            {
                if (!(this.Connection.State == System.Data.ConnectionState.Closed))
                {
                    this.Connection.Close();
                }
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public MySqlConnection GetConnection()
        {
            return this.Connection;
        }
        #endregion

        #region Query Methodes

        #endregion
    }
}
