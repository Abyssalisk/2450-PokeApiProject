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

            if (UserNameAlreadyTaken(TrainerName))
                return "username already taken";

            try
            {
                Controllers.PokemonController.CreateNewTrainer(model.Username);
                Password = EncryptPassword(Password);
                InsertDBcredentials(TrainerName, Password, Email);
               
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
        public static bool UserNameAlreadyTaken(string userName)
        {
            var query = $"SELECT Count(*) FROM Trainers WHERE TrainerHandle = '{userName}'";
            var usernameExists = DBConnect.ExecuteScalar(query) > 0;

            return usernameExists;
        }

        //This method inserts the user login credentials into the DB
        public static void InsertDBcredentials(string name, string encryptedPass, string email)
        {
            var query = $"INSERT INTO Users(UserName, Password, Email) VALUES('{name}', '{encryptedPass}', '{email}');";
            DBConnect.ExecuteNonQuery(query);
        }
    }
}
