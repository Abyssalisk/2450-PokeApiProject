using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

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
            var connectionString = "Server=" + hostname +"; Port="+port+ "; Database=" + dbname + "; Uid=" + username + "; Pwd=" + password + ";";
            return connectionString;
        }
    }

    public class DBconnect
    {
        public DBconnect()
        {
            Console.WriteLine("connecting.....");
            var testQuery = "select * from sql3346222.userCredentials";
            var connectionString = Helpers.GetRDSConnectionString();

            using (MySqlConnection myConnection = new MySqlConnection())
            {
                myConnection.ConnectionString = connectionString;
                myConnection.Open();
                Console.WriteLine("Connected !!!!!!Version: " + myConnection.ServerVersion);
                // execute queries, etc
                var query = new MySqlCommand(testQuery, myConnection);

                myConnection.Close();
            }
        }
    }

    public class ConsoleOutputInput
    {
        public ConsoleOutputInput()
        {
            Console.WriteLine("Welcome to Pokemon Battle Simulator Console Version!");
            var connection = new DBconnect();

            Console.WriteLine("Please enter your username: ");
        }
    }
}
