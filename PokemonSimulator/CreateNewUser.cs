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
        public string TrainerName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }


        public CreateNewUser(bool test)
        {
            //testing constructor that doesn't utilize console
        }

        public CreateNewUser(MySqlConnection con)
        {
            DoUserCreation(con);
        }

        private void DoUserCreation(MySqlConnection con)
        {
            //Database only take VARCHAR(100) to save on space, user inputs need to be less than 100 chars
            while (true)
            {
                //Makes sure new user name is less than 100 chars
                Console.WriteLine("Enter new desired trainer name");
                TrainerName = Console.ReadLine();
                if (TrainerName.Length > 50)
                {
                    Console.WriteLine("Trainer name is to long, enter a shorter one!");
                }
                else
                {
                    if (UserNameValidation(TrainerName, con))
                    {
                        if (!Grand.alphaNumeric.IsMatch(TrainerName))
                        {
                            Console.WriteLine("Trainer names can contain only letters, numbers, and underscores!");
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("That trainer name is already taken! Try another username.");
                    }
                }
            }

            while (true)
            {
                //Makes sure new user name is less than 100 chars
                Console.WriteLine("Enter new Password");
                Password = Console.ReadLine();
                if (Password.Length > 50)
                {
#warning This input is not yet being vetted to mitigate SQL injections.
                    Console.WriteLine("Password is to long, enter a shorter one!");
                }
                else
                {
                    break;
                }
            }

            while (true)
            {
                //Makes sure new Email is less than 100 chars
                Console.WriteLine("Enter Email address");
                Email = Console.ReadLine();
                if (Password.Length > 99)
                {
                    Console.WriteLine("Email is to long, choose a different one!");
                }
                else
                {
                    var EmailSetup = new EmailValidation(Email);
                    //validates if the entered Email is in supported format
                    if (EmailSetup.EmailIsInCorrectForm == false)
                    {
                        Console.WriteLine("Email is in invalid form! Try again");
                    }
                    if (EmailSetup.EmailIsValid == false)
                    {
                        Console.WriteLine("Email could not be validated! Try again");
                    }
                    else
                    {
                        break;
                    }
                }
            }
            Password = UserPasswordHash(Password);
            InsertDBcredentials(TrainerName, Password, Email, con);
        }

        //This is a private helper method that uses hashing alg to hash the nwe Password
        public string UserPasswordHash(string thePass)
        {
            string Hashedpass;
            var sendToHashPasswordAlg = new HashingAlg(thePass);
            Hashedpass = sendToHashPasswordAlg.getHash();
            Hashedpass = sendToHashPasswordAlg.addSecret(Hashedpass);
            return Hashedpass;
        }

        //Private method to verify Email is in the correct form

        //checks to see if username is already taken
        public bool UserNameValidation(string userName, MySqlConnection con)
        {
            if (!Grand.alphaNumeric.IsMatch(userName))
            {
                return false;
            }
            con.Open();
            //INSERT query
            string plainTextQuery = "SELECT TrainerName FROM sql3346222.userCredentials WHERE(TrainerName = '" + userName + "');";
            string returnedQuery = "";
            //execute the query
            MySqlCommand query = new MySqlCommand(plainTextQuery, con);
            MySqlDataReader rdr = query.ExecuteReader();

            while (rdr.Read())
            {
                returnedQuery = (rdr[0]).ToString();
            }
            rdr.Close();
            con.Close();

            if (returnedQuery == userName)
            {
                return false;
            }

            return true;
        }
        //This method inserts the user login credentials into the DB
        public void InsertDBcredentials(String name, String passAfterItHashed, String email, MySqlConnection connection)
        {
            //Opens a new connection to MySql DB
            connection.Open();
            //INSERT query
            string plainTextQuery = "INSERT INTO sql3346222.userCredentials(TrainerName, Password, Email) VALUES(@Name,@Pass,@Email);";
            //execute the query
            MySqlCommand query = new MySqlCommand(plainTextQuery, connection);
            query.Parameters.Add(@"@Name", MySqlDbType.VarChar);
            query.Parameters.Add(@"@Pass", MySqlDbType.Text);
            query.Parameters.Add(@"@Email", MySqlDbType.VarChar);
            query.Parameters[@"@Name"].Value = name;
            query.Parameters[@"@Pass"].Value = passAfterItHashed;
            query.Parameters[@"@Email"].Value = email;
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
