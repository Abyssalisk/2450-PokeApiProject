using MySql.Data.MySqlClient;
using System;
using Web.Shared.Models;

namespace Web.Server.Classes
{
    public class NewUser
    {
        public static string DoUserCreation(CreateAccountModel model)
        {
            var Password = model.Password;
            var TrainerName = model.Username;
            var Email = model.Email;

            var con = new DBConnect().MyConnection;
            if (UserNameValidation(TrainerName, con) == false)
                return "username already taken"; // username taken

            try
            {
                Password = EncryptPassword(Password);
                InsertDBcredentials(TrainerName, Password, Email, con);
                return "account created";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "error";
            }
        }

        //This is a private helper method that uses hashing alg to hash the nwe Password
        public static string EncryptPassword(string thePass)
        {
            return new Encryption(thePass).EncryptedPassword;
        }
        //checks to see if username is already taken
        public static bool UserNameValidation(string userName, MySqlConnection con)
        {
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

            return !returnedQuery.Equals(userName);
        }

        //This method inserts the user login credentials into the DB
        public static void InsertDBcredentials(string name, string encryptedPass, string email, MySqlConnection connection)
        {
            connection.Open();
            var query = $"INSERT INTO sql3346222.userCredentials(TrainerName, Password, Email, HighScore) VALUES('{name}', '{encryptedPass}', '{email}', 0);";
            new MySqlCommand(query, connection).ExecuteNonQuery();
            connection.Close();
        }
    }
}
