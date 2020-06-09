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
        //Class fields
        string TrainerName;
        string password;
        string email;

        public CreateNewUser(MySqlConnection con)
        {
            //Database only take VARCHAR(100) to save on space, user inputs need to be less than 100 chars
            while (true)
            {
                //Makes sure new user name is less than 100 chars
                Console.WriteLine("Enter new desired trainer name");
                TrainerName = Console.ReadLine();
                if (TrainerName.Length > 99)
                { 
                    Console.WriteLine("User Name is to long, enter a shorter one!");
                }else
                {
                    break;
                }
            }

            while (true)
            { 
                //Makes sure new user name is less than 100 chars
                Console.WriteLine("Enter new password");
                password = Console.ReadLine();
                if(password.Length>99)
                {
                    Console.WriteLine("Password is to long, enter a shorter one!");
                }else
                {
                    break;
                }
            }

            while (true)
            {
                //Makes sure new email is less than 100 chars
                Console.WriteLine("Enter email address");
                email = Console.ReadLine();
                if (password.Length > 99)
                {
                    Console.WriteLine("Email is to long, choose a different one!");
                }
                else
                {
                    if (emailValidation(email) == false)
                    {
                        Console.WriteLine("Email is invalid!Try again");
                    }
                    else
                    {
                        break;
                    }
                }
            }
            password = UserPasswordHash(password);
            insertDBcredentials(TrainerName, password, email, con);
        }
        //This is a private helper method that uses hashing alg to hash the nwe password
        private string UserPasswordHash(string thePass)
        {
            string Hashedpass;
            var sendToHashPasswordAlg = new HashingAlg(thePass);
            Hashedpass = sendToHashPasswordAlg.getHash();
            return Hashedpass;
        }

        //Private method to verify email is in the correct form
        private Boolean emailValidation(string email)
        {
            string emailRegex;
            return false;
        }

        //This method inserts the user login credentials into the DB
        private void insertDBcredentials(String name, String passAfterItHashed,  String email, MySqlConnection connection)
        {
            //Opens a new connection to MySql DB
            connection.Open();
            //INSERT query
            string plainTextQuery = "INSERT INTO sql3346222.userCredentials(TrainerName, password, email) VALUES('" + name + 
                "','" + passAfterItHashed + "','"+ email+ "');";
            //execute the query
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
