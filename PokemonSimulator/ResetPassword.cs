using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;


namespace PokemonSimulator
{
    public class ResetPassword
    {
        public string TrainerName { get; set; }
        public string EnteredEmail { get; set; }
        public MySqlConnection Connection { get; set; }

        public ResetPassword(MySqlConnection con)
        {
            while (true)
            {
                Connection = con;

                Console.WriteLine("lets reset your password\nFirst enter your trainer name: ");
                TrainerName = Console.ReadLine();

                string lookupEmailByName = "SELECT email FROM sql3346222.userCredentials WHERE(TrainerName = '" + TrainerName + "');";
                string returnedEmail = "0";

                Console.WriteLine("Enter the email attached to your account: ");
                EnteredEmail = Console.ReadLine();

                con.Open();
                MySqlCommand query = new MySqlCommand(lookupEmailByName, con);
                MySqlDataReader rdr = query.ExecuteReader();

                //reading returned query
                while (rdr.Read())
                {
                    returnedEmail = rdr[0].ToString();
                }
                rdr.Close();
                con.Close();

                //if an email is returned from DB this is skipped
                if (returnedEmail.Length <= 1)
                {
                    string newUser;
                    Console.WriteLine("No email found or user name incorrect!\nPlease try again or create new user");
                    Console.WriteLine("Would you like to make a new account?(y/n)");
                    newUser = Console.ReadLine();
                    while (true)
                    {
                        //Choice if user is new, takes them to create user
                        if (newUser.ToLower().Equals("y"))
                        {
                            var backToMakeNewAccount = new ConsoleOutputInput();
                            break;
                        }

                        //Choice if user enters, N not a new user, prompts login
                        if (newUser.ToLower().Equals("n"))
                        {
                            break;
                        }

                        //if something other than y or n is entered user is prompted with choice again
                        Console.WriteLine("Invalid choice! Please type y or n");
                        Console.WriteLine("Make new account? (y/n)");
                        newUser = Console.ReadLine();
                    }
                }

                while (true)
                {
                    if (returnedEmail == EnteredEmail)
                    {
                        var emailVerificationForReset = new EmailValidation(returnedEmail);
                        if(emailVerificationForReset.EmailIsValid==true)
                        {
                            Console.WriteLine("Lets reset your password...");
                            makeNewPassword();
                            var backToLogin = new ConsoleOutputInput();
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Emails do not match! Let's try this again");
                        break;
                    }
                }
                break;
            }
        }

        private void makeNewPassword()
        {
            string newPass;

            Console.WriteLine("Enter new password: ");
            newPass = Console.ReadLine();

            string Hashedpass;
            var sendToHashPasswordAlg = new HashingAlg(newPass);
            Hashedpass = sendToHashPasswordAlg.getHash();
            Hashedpass = sendToHashPasswordAlg.addSecret(Hashedpass);

            Connection.Open();
            //INSERT query
            string plainTextQuery = "UPDATE sql3346222.userCredentials SET Password=('"+Hashedpass+"')" +
                " WHERE TrainerName = ('"+TrainerName+"');";
            //execute the query
            MySqlCommand query = new MySqlCommand(plainTextQuery, Connection);
            MySqlDataReader rdr = query.ExecuteReader();

            while (rdr.Read())
            {
                Console.WriteLine(rdr[0] + " -- " + rdr[1]);
            }
            rdr.Close();
            Connection.Close();

            Console.WriteLine("Password reset!");
        }
    }
}
