using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using RestSharp.Validation;

namespace PokemonSimulator
{
    public class Helpers
    {
        public static string GetRDSConnectionString()
        {

            var dbname = "sql3346222";

            if (string.IsNullOrEmpty(dbname)) return null;

            string username = "sql3346222";
            string password = "wTvU3pVa7f";
            string hostname = "sql3.freemysqlhosting.net";
            string port = "3306";
            var connectionString = "Server=" + hostname + "; Port=" + port + "; Database=" + dbname + "; Uid=" + username + "; Pwd=" + password + ";";
            return connectionString;
        }
    }

    public class DBconnect
    {
        public MySqlConnection myConnection;
        public DBconnect()
        {
            Console.WriteLine("connecting.....");
            var connectionString = Helpers.GetRDSConnectionString();

            using (MySqlConnection connect = new MySqlConnection())
            {
                try
                {
                    connect.ConnectionString = connectionString;
                    connect.Open();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e.ToString());
                    Console.WriteLine("Failed to connect, check your internet connection!");
                }

                finally
                {
                    if (connect != null || connect.State == System.Data.ConnectionState.Open)
                        myConnection = connect;
                    connect.Close();
                }
            }
        }
    }
}
