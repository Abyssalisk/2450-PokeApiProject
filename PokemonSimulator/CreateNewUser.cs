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
