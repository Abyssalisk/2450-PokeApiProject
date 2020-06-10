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
        DBconnect connection;
        string loginName = "";
        string password = "";

        String TrainerName;
        int userID = 0;

        public ConsoleOutputInput()
        {
            Console.WriteLine("Welcome to Pokemon Battle Simulator Console Version!");
            connection = new DBconnect();

            string newUser;

            Console.WriteLine("Are you a new trainer? (y/n)");
            newUser = Console.ReadLine();

            while (true)
            {
                //Choice if user is new, takes them to create user
                if (newUser.ToLower().Equals("y"))
                {
                    var createNewTrainer = new CreateNewUser(connection.myConnection);
                    Login();//once new user is made pompts for login
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

        //Method to log user in, validates by looking up hashed password
        private void Login()
        {
            Console.WriteLine("Please login! Trainer name: ");
            loginName = Console.ReadLine();

            Console.WriteLine("enter your password: ");
            password = Console.ReadLine();

            //validates password
            Boolean accountIsAuth = Validate(loginName, password, connection.myConnection);
            if(accountIsAuth==false)
            {
                Login();
            }else
            {
                return;
            }
        }

        //Private method to validate password, if password is false gives option to reset password
        private Boolean Validate(string userName, string pass, MySqlConnection con)
        {
            string lookupByName = "SELECT `UserID`,Password FROM sql3346222.userCredentials WHERE(TrainerName = '" + userName+"');";
            string correctPassword = "";
            string resetPasswrodYorN = "";

            //opens new DB connection with MySql and pulls hashed password from userCredentials table
            con.Open();
            MySqlCommand query = new MySqlCommand(lookupByName, con);
            MySqlDataReader rdr = query.ExecuteReader();

            //reading returned query
            while (rdr.Read())
            {
                userID = Convert.ToInt32(rdr[0].ToString());
                correctPassword =rdr[1].ToString();
            }
            rdr.Close();
            con.Close();
            if(userID==0)
            {
                Console.WriteLine("account not found!");
                return false;
            }

            string attemptedPassword;
            var sendToHashPasswordAlg = new HashingAlg(pass);
            attemptedPassword = sendToHashPasswordAlg.getHash();

            correctPassword = sendToHashPasswordAlg.reomveSecret(correctPassword);

            //checks the hashed password the user entered agaisnt the hashedpass from DB
            if (correctPassword==attemptedPassword)
            {
                TrainerName = userName;
                Console.WriteLine("Welcome "+userName);
                Console.WriteLine("-------------------------------------------------------------------");
                return true;
            }else //failed login attempt
            {
                Console.WriteLine("Username or password incorrect! Please try again!");
                Console.WriteLine("Do you need to reset your password? (y/n)");
                resetPasswrodYorN = Console.ReadLine();

                while(true)
                {
                    if (resetPasswrodYorN.ToLower().Equals("y"))
                    {
                        var reset = new ResetPassword(con);
                        return true;
                        break;
                    }
                    if(resetPasswrodYorN.ToLower().Equals("n"))
                    {
                        break;
                    }

                    Console.WriteLine("Invalid choice, please eneter y to reset password or n to reattempt login!");
                    Console.WriteLine("Do you need to reset your password? (y/n)");
                    resetPasswrodYorN = Console.ReadLine();
                }
                return false;
            }

        }

    }


}
