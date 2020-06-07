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


}
