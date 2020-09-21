using MySql.Data.MySqlClient;
using System;
using Web.Shared.Classes;

namespace Web.Server.Classes
{
    [Obsolete]
    public class ResetPassword
    {
        string trainerName;
        string enteredEmail;
        MySqlConnection connection;

        public ResetPassword(MySqlConnection con, string username, string email) // become an endpoint for api
        {
            connection = con;
            trainerName = username;
            enteredEmail = email;
            string lookupEmailByName = "SELECT email FROM sql3346222.userCredentials WHERE(TrainerName = '" + trainerName + "');";
            string returnedEmail = "0";

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

            //while (true)
            //{
            //    if (returnedEmail == enteredEmail)
            //    {
            //        var emailVerificationForReset = new EmailValidation(returnedEmail);
            //        if (emailVerificationForReset.EmailIsValid == true)
            //        {
            //            Console.WriteLine("Lets reset your password...");
            //            makeNewPassword();
            //            return; // return ActionResult Ok
            //        }
            //    }
            //    else
            //    {
            //        return; //return ActionResult Conflict
            //    }
            //}
        }

        private void makeNewPassword()
        {
            string newPass;

            Console.WriteLine("Enter new password: ");
            newPass = Console.ReadLine();

            string Hashedpass;
            var sendToHashPasswordAlg = new Encryption(newPass);
            Hashedpass = sendToHashPasswordAlg.EncryptedPassword;

            connection.Open();
            //INSERT query
            string plainTextQuery = "UPDATE sql3346222.userCredentials SET Password=('" + Hashedpass + "')" +
                " WHERE TrainerName = ('" + trainerName + "');";
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
