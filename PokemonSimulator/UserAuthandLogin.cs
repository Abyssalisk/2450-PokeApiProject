using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using RestSharp.Validation;
using System.Dynamic;

namespace PokemonSimulator
{

    public class UserAuthAndLogin
    {
        public DBconnect Connection { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string TrainerName { get; set; }
        public int UserID { get; set; }

        public UserAuthAndLogin(bool test)
        {
            // placeholder constructor to access methods for testing
        }

        public UserAuthAndLogin()
        {
            UserID = 0;
            LoginName = string.Empty;
            Password = string.Empty;
            TrainerName = string.Empty;
            Connection = new DBconnect();

            // Begin Console Program
            Console.WriteLine("Welcome to Pokemon Battle Simulator Console Version!");

            Console.WriteLine("Are you a new trainer? (y/n)");
            var newUser = Console.ReadLine();

            while (true)
            {
                //Choice if user is new, takes them to create user
                if (newUser.ToLower().Equals("y"))
                {
                    var createNewTrainer = new CreateNewUser(Connection.myConnection);
                    Login(); //once new user is made prompts for login
                    break;
                }

                //Choice if user enters, N not a new user, prompts login
                if (newUser.ToLower().Equals("n"))
                {
                    Login();
                    break;
                }

                //if something other than y or n is entered user is prompted with choice again
                Console.WriteLine("Invalid choice! Please type y or n");
                Console.WriteLine("Are you a new trainer? (y/n)");
                newUser = Console.ReadLine();
            }
        }

        //Logs user in, validates by looking up hashed Password
        public void Login()
        {
            Console.WriteLine("Please login! Trainer name: ");
            LoginName = Console.ReadLine();

            Console.WriteLine("enter your Password: ");
            Password = Console.ReadLine();

            //validates Password
            var accountIsAuthorized = Validate(LoginName, Password, Connection.myConnection);
            if (accountIsAuthorized == false)
            {
                Login();
            }
            else
            {
                return;
            }
        }

        //Validates Password, if Password is false gives option to reset Password
        public Boolean Validate(string userName, string pass, MySqlConnection con)
        {
            var lookupByName = "SELECT `UserID`,Password FROM sql3346222.userCredentials WHERE(TrainerName = '" + userName + "');";
            var correctPassword = string.Empty;

            //opens new DB Connection with MySql and pulls hashed Password from userCredentials table
            con.Open();
            var cmd = new MySqlCommand(lookupByName, con);
            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    UserID = Convert.ToInt32(rdr[0].ToString());
                    correctPassword = rdr[1].ToString();
                }
            }
            con.Close();

            if (UserID == 0)
            {
                Console.WriteLine("account not found!");
                return false;
            }

            var sendToHashPasswordAlg = new HashingAlg(pass);
            var attemptedPassword = sendToHashPasswordAlg.getHash();
            correctPassword = sendToHashPasswordAlg.reomveSecret(correctPassword);

            //checks the hashed Password the user entered agaisnt the hashedpass from DB
            if (correctPassword == attemptedPassword)
            {
                TrainerName = userName;
                Console.WriteLine("Welcome " + userName);
                Console.WriteLine("-------------------------------------------------------------------");
                return true;
            }
            else //failed login attempt
            {
                Console.WriteLine("Username or Password incorrect! Please try again!");
                Console.WriteLine("Do you need to reset your Password? (y/n)");
                var resetPasswrodYorN = Console.ReadLine();

                while (true)
                {
                    if (resetPasswrodYorN.ToLower().Equals("y"))
                    {
                        var reset = new ResetPassword(con);
                        return true;
                    }
                    if (resetPasswrodYorN.ToLower().Equals("n"))
                    {
                        return false;
                    }

                    Console.WriteLine("Invalid choice, please eneter y to reset Password or n to reattempt login!");
                    Console.WriteLine("Do you need to reset your Password? (y/n)");
                    resetPasswrodYorN = Console.ReadLine();
                }
            }
        }
    }
}
