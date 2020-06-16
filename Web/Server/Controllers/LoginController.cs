using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Web.Client.Pages;
using Web.Shared.Models;

namespace Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // GET api/<LoginController>/validate?username=adf&password=34523n
        [HttpGet("validate")]
        public void Get([FromQuery] string email, [FromQuery] string code)
        {
            SendEmail(email, code);
        }

        //POST api/<LoginController>
        [HttpGet("createaccount")]
        public string Get([FromQuery] string name, [FromQuery] string pass, [FromQuery] string email)
        {
            var cam = new CreateAccountModel()
            {
                Username = name,
                Password = pass,
                Email = email
            };

            return CreateNewUser.DoUserCreation(cam);
        }

        [HttpGet]
        public string Login([FromQuery] string username, [FromQuery] string password)
        {
            var accountIsAuth = Validate(username, password, new DBConnect().myConnection);

            if (accountIsAuth)
                return "true";
            else
                return "false";
        }

        [HttpGet("sendlogin/{email}")]
        public void SendLogin(string email)
        {
            SendEmail(email.Replace("email=", string.Empty));
        }

        public void SendEmail(string email, [Optional] string code)
        {
            try
            {
                var mailMessage = new MimeMessage();
                mailMessage.From.Add(new MailboxAddress("Pokemanz", "pokemanz2450@gmail.com"));
                mailMessage.To.Add(new MailboxAddress("Trainer", email));
                

                var textpart = new TextPart("plain");

                if (!string.IsNullOrEmpty(code))
                {
                    mailMessage.Subject = "PokeMans Verification Code";
                    textpart.Text = $"Your PokeManz verification code is: \n\n" + code;
                }
                else // send login creds/forgot password
                {
                    var LoginModel = new LoginModel();

                    var con = new DBConnect().myConnection;
                    string lookupEmailByName = $"SELECT TrainerName, Password FROM sql3346222.userCredentials WHERE(Email='{email}') LIMIT 1;";

                    con.Open();
                    MySqlCommand query = new MySqlCommand(lookupEmailByName, con);
                    MySqlDataReader rdr = query.ExecuteReader();

                    //reading returned query
                    while (rdr.Read())
                    {
                        LoginModel.Username = rdr[0].ToString(); // username
                        LoginModel.Password = new Encryption().Decrypt(rdr[1].ToString()); // password hashed + secret
                    }
                    rdr.Close();
                    con.Close();

                    mailMessage.Subject = "PokeManz Credentials";
                    textpart.Text = $"Your login credentials are\n\nUsername: {LoginModel.Username}\nPassword: {LoginModel.Password}";
                }

                mailMessage.Body = textpart;
                Task.Run(() => DoSending(mailMessage)); // fire off and forget about it
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task DoSending(MimeMessage mailMessage)
        {
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("pokemanz2450@gmail.com", "13juliet");
                await client.SendAsync(mailMessage);
                await client.DisconnectAsync(true);
            }
        }

        public bool Validate(string username, string password, MySqlConnection con)
        {
            LoginModel login = new LoginModel()
            {
                Username = username,
                Password = password
            };

            string lookupByName = "SELECT `UserID`,Password FROM sql3346222.userCredentials WHERE(TrainerName = '" + login.Username + "');";
            string decryptedPass = string.Empty;
            int userID = 0;

            //opens new DB connection with MySql and pulls hashed password from userCredentials table
            con.Open();
            MySqlCommand query = new MySqlCommand(lookupByName, con);
            MySqlDataReader rdr = query.ExecuteReader();

            while (rdr.Read())
            {
                userID = Convert.ToInt32(rdr[0].ToString());
                decryptedPass = new Encryption().Decrypt(rdr[1].ToString());
            }
            rdr.Close();
            con.Close();
            if (userID == 0)
            {
                return false; // account not found
            }

            return decryptedPass.Equals(password);
        }
    }


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

    public class DBConnect
    {
        public MySqlConnection myConnection;
        public DBConnect()
        {
            using (MySqlConnection connect = new MySqlConnection())
            {
                try
                {
                    connect.ConnectionString = Helpers.GetRDSConnectionString();
                    connect.Open();
                }
                catch { }
                finally
                {
                    if (connect != null || connect.State == System.Data.ConnectionState.Open)
                        myConnection = connect;
                }
            }
        }
    }

    public class Helpers
    {
        public static string GetRDSConnectionString()
        {
            var dbname = "sql3346222";

            if (string.IsNullOrEmpty(dbname)) return null;

            string username = "sql3346222";
            string password = "wTvU3pVa7f";
            string hostname = "sql3.freemysqlhosting.net";
            string port = "3306";
            var connectionString = "Server=" + hostname + "; Port=" + port + "; Database=" + dbname + "; Uid=" + username + "; Pwd=" + password + ";";
            return connectionString;
        }
    }
    public class Encryption
    {
        public string EncryptedPassword { get; set; }

        public Encryption()
        {
            
        }
        public Encryption(string password)
        {
            EncryptedPassword = Encrypt(password);
        }

        public string Encrypt(string password)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            return System.Convert.ToBase64String(bytes);
        }
        public string Decrypt(string encryptedPassword)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(encryptedPassword);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }

    public class CreateNewUser
    {
        public static string DoUserCreation(CreateAccountModel model)
        {
            var Password = model.Password;
            var TrainerName = model.Username;
            var Email = model.Email;

            var con = new DBConnect().myConnection;
            if (UserNameValidation(TrainerName, con) == false)
                return "username already taken"; // username taken

            try
            {
                Password = UserPasswordHash(Password);
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
        public static string UserPasswordHash(string thePass)
        {
            string Hashedpass;
            var sendToHashPasswordAlg = new Encryption(thePass);
            Hashedpass = sendToHashPasswordAlg.EncryptedPassword;
            return Hashedpass;
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
        public static void InsertDBcredentials(string name, string passAfterItHashed, string Email, MySqlConnection connection)
        {
            //Opens a new connection to MySql DB
            connection.Open();
            //INSERT query
            string plainTextQuery = "INSERT INTO sql3346222.userCredentials(TrainerName, Password, Email) VALUES('" + name +
                "','" + passAfterItHashed + "','" + Email + "');";
            //execute the query
            MySqlCommand query = new MySqlCommand(plainTextQuery, connection);
            MySqlDataReader rdr = query.ExecuteReader();

            while (rdr.Read())
            {
                Console.WriteLine(rdr[0] + " -- " + rdr[1]);
            }
            rdr.Close();
            connection.Close();
        }
    }


}
