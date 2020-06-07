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
            var connectionString = "Server=" + hostname +"; Port="+port+ "; Database=" + dbname + "; Uid=" + username + "; Pwd=" + password + ";";
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
                connect.ConnectionString = connectionString;
                connect.Open();
                Console.WriteLine("Connected !!!!!!Version: " + connect.ServerVersion);
                myConnection = connect;
                connect.Close();
            }
        }
    }

    public class ConsoleOutputInput
    {
        public ConsoleOutputInput()
        {
            Console.WriteLine("Welcome to Pokemon Battle Simulator Console Version!");
            var connection = new DBconnect();
            string newUser;

            Console.WriteLine("Are you a new trainer? (y/n)");
            newUser = Console.ReadLine();
            if(newUser == "y"||newUser == "Y")
            {
                var createNewTrainer = new CreateNewUser(connection.myConnection);
            }
            Console.WriteLine("Please login! Trainer name: ");
            var loginName = Console.ReadLine();

            Console.WriteLine("enter your password: ");
            var password = Console.ReadLine();

            Validate(loginName, password, connection.myConnection);
        }
        private void Validate(string userName, string pass, MySqlConnection con)
        {
            string lookupByName = "SELECT password FROM sql3346222.userCredentials WHERE(TrainerName = '"+userName+"');";
            string correctPassword = "";

            con.Open();
            MySqlCommand query = new MySqlCommand(lookupByName, con);
            MySqlDataReader rdr = query.ExecuteReader();

            while (rdr.Read())
            {
                correctPassword=rdr[0].ToString();
            }
            rdr.Close();

            string attemptedPassword;
            var sendToHashPasswordAlg = new HashingAlg(pass);
            attemptedPassword = sendToHashPasswordAlg.getHash();
            if (correctPassword==attemptedPassword)
            {
                Console.WriteLine("Welcome "+userName);
            }

        }
}

    public class CreateNewUser
    {
        string TrainerName;
        string password;

        public CreateNewUser(MySqlConnection con)
        {
            Console.WriteLine("Enter new desired trainer name");
            TrainerName = Console.ReadLine();

            Console.WriteLine("Enter new password");
            password = Console.ReadLine();
            password = UserPasswordHash(password);

            insertDBcredentials(TrainerName, password, con);
        }

        private string UserPasswordHash(string thePass)
        {
            string Hashedpass;
            var sendToHashPasswordAlg = new HashingAlg(thePass);
            Hashedpass = sendToHashPasswordAlg.getHash();
            return Hashedpass;
        }

        private void insertDBcredentials(String name, String passAfterItHashed, MySqlConnection connection)
        {
            connection.Open();
            string plainTextQuery = "INSERT INTO sql3346222.userCredentials(TrainerName, password) VALUES('" + name + "','" + passAfterItHashed + "');";
            MySqlCommand query = new MySqlCommand(plainTextQuery, connection);
            MySqlDataReader rdr = query.ExecuteReader();

            while (rdr.Read())
            {
                Console.WriteLine(rdr[0] + " -- " + rdr[1]);
            }
            rdr.Close();
            connection.Close();

            Console.WriteLine("new trainer added!");

        }

    }
}
