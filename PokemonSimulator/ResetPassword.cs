using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using MimeKit;
using MailKit.Security;

namespace PokemonSimulator
{
    class ResetPassword
    {
        string verificationString;
        string trainerName;
        string enteredEmail;
        MySqlConnection connection;

        public ResetPassword(MySqlConnection con)
        {
            while (true)
            {
                verificationString = randoStringBuilder();
                connection = con;

                Console.WriteLine("lets reset your password\nFirst enter your trainer name: ");
                trainerName = Console.ReadLine();

                string lookupEmailByName = "SELECT email FROM sql3346222.userCredentials WHERE(TrainerName = '" + trainerName + "');";
                string returnedEmail = "0";

                Console.WriteLine("Enter the email attached to your account: ");
                enteredEmail = Console.ReadLine();

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

                if (returnedEmail.Length == 1)
                {
                    string newUser;
                    Console.WriteLine("No email found or user name incorrect!\nPlease try again or create new user");
                    Console.WriteLine("Would you like to make a new account?(y/n)");
                    newUser = Console.ReadLine();
                    while (true)
                    {
                        //Choice if user is new, takes them to create user
                        if (newUser == "y" || newUser == "Y")
                        {
                            var backToMakeNewAccount = new ConsoleOutputInput();
                            break;
                        }

                        //Choice if user enters, N not a new user, prompts login
                        if (newUser == "n" || newUser == "N")
                        {
                            break;
                        }

                        //if something other than y or n is entered user is prompted with choice again
                        Console.WriteLine("Invalid choice! Please type y or n");
                        Console.WriteLine("Make new account? (y/n)");
                        newUser = Console.ReadLine();
                    }
                    break;

                }
                while (true)
                {
                    if (returnedEmail == enteredEmail)
                    {
                        sendEmail(verificationString,returnedEmail);
                        Console.WriteLine("Email with verifiaction code has been sent, please check your email....");
                        if (validateEmailCode() == true)
                        {
                            Console.WriteLine("Email has been verified, lets reset your password");
                            makeNewPassword();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Incorrect verfication code! Lets try this again");
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

        private string randoStringBuilder()
        {
            int length = 6;

            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            return str_build.ToString();
        }

        private async void sendEmail(string verificationCode, string email)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("Pokemanz Password Reset", "pokemanz2450@gmail.com"));
            mailMessage.To.Add(new MailboxAddress("Trainer", email));
            mailMessage.Subject = "Password verification code";
            mailMessage.Body = new TextPart("plain")
            {
                Text = "Here is your verification code: \n\n" + verificationCode
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("pokemanz2450@gmail.com", "13juliet");
                await client.SendAsync(mailMessage);
                await client.DisconnectAsync(true);
            }
        }

        private Boolean validateEmailCode()
        {
            string codeEntered;
            Console.WriteLine("Enter email validation code: ");
            codeEntered = Console.ReadLine();
            if(codeEntered == verificationString)
            {
                return true;
            }else
            {
                return false;
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

            connection.Open();
            //INSERT query
            string plainTextQuery = "UPDATE sql3346222.userCredentials SET password=('"+Hashedpass+"')" +
                " WHERE TrainerName = ('"+trainerName+"');";
            //execute the query
            MySqlCommand query = new MySqlCommand(plainTextQuery, connection);
            MySqlDataReader rdr = query.ExecuteReader();

            while (rdr.Read())
            {
                Console.WriteLine(rdr[0] + " -- " + rdr[1]);
            }
            rdr.Close();
            connection.Close();

            Console.WriteLine("Password reset!");
        }
    }
}
