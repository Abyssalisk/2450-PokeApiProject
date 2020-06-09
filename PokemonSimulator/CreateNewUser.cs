using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using RestSharp.Validation;
using System.Text.RegularExpressions;

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
                    Console.WriteLine("Trainer name is to long, enter a shorter one!");
                }else
                {
                    if (userNameValidation(TrainerName,con) == false)
                    {
                        Console.WriteLine("Trainer name is already taken!Try again");
                    }
                    else
                    {
                        break;
                    }
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
                    //validates if the entered email is in supported format
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
            Boolean isValid = Regex.IsMatch(email,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");
            return isValid;
        }

        //checks to see if username is already taken
        private Boolean userNameValidation(string userName, MySqlConnection con)
        {
            con.Open();
            //INSERT query
            string plainTextQuery = "SELECT TrainerName FROM sql3346222.userCredentials WHERE(TrainerName = '"+userName+"');";
            string returnedQuery = "";
            //execute the query
            MySqlCommand query = new MySqlCommand(plainTextQuery, con);
            MySqlDataReader rdr = query.ExecuteReader();

            while (rdr.Read())
            {
                returnedQuery=(rdr[0]).ToString();
            }
            rdr.Close();
            con.Close();

            if(returnedQuery==userName)
            {
                return false;
            }

            return true;
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
