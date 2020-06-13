using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MySql.Data.MySqlClient;
using Web.Shared.Models;

namespace Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // GET api/<LoginController>?username=adf&password=34523n
        [HttpGet]
        public ActionResult Get([FromQuery] string username, [FromQuery] string password)
        {
            //Login(username, password);
            return Ok();
        }

        // GET api/<LoginController>?username=adf&password=34523n
        [HttpGet("validate/{email}")]
        public async Task<Validator> Get(string email)
        {
            var validator = new Validator() { RandomCode = "error" };
            var evee = new EmailValidation(email);
            if (evee.EmailIsValid)
            {
                var success = await EmailValidation.SendEmail(evee.Email, evee.RandomCode);
                await Task.Delay(2000);
                if (success)
                {
                    validator.RandomCode = evee.RandomCode.ToUpper();
                }
            }
            return validator;
        }

        // POST api/<LoginController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<LoginController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LoginController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private ActionResult Login(string loginName, string password)
        {
            var login = new Shared.Models.Login();
            login.Username = loginName;
            login.Password = password;
            var connection = new DBConnect();

            //validates password
            Boolean accountIsAuth = Validate(loginName, password, connection.myConnection);
            if (accountIsAuth == false)
            {
                return Conflict("Invalid username and/or password");
            }
            else
            {
                return Ok();
            }
        }

        //Private method to validate password, if password is false gives option to reset password
        public Boolean Validate(string userName, string pass, MySqlConnection con)
        {
            string lookupByName = "SELECT `UserID`,Password FROM sql3346222.userCredentials WHERE(TrainerName = '" + userName + "');";
            string correctPassword = "";
            int userID = 0;

            //opens new DB connection with MySql and pulls hashed password from userCredentials table
            con.Open();
            MySqlCommand query = new MySqlCommand(lookupByName, con);
            MySqlDataReader rdr = query.ExecuteReader();

            //reading returned query
            while (rdr.Read())
            {
                userID = Convert.ToInt32(rdr[0].ToString());
                correctPassword = rdr[1].ToString();
            }
            rdr.Close();
            con.Close();
            if (userID == 0)
            {
                return false; // account not found
            }

            string attemptedPassword;
            var sendToHashPasswordAlg = new HashingAlg(pass);
            attemptedPassword = sendToHashPasswordAlg.getHash();
            correctPassword = sendToHashPasswordAlg.reomveSecret(correctPassword);

            //checks the hashed password the user entered agaisnt the hashedpass from DB
            if (correctPassword == attemptedPassword)
            {
                //TrainerName = userName;
            }
            else //failed, reset password
            {
                var reset = new ResetPassword(con, userName, "testEmail@email.com");
                // var reset = new ResetPassword(con, userName, "testEmail@email.com"); => Change to api call 
            }
            return true;
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

            while (true)
            {
                if (returnedEmail == enteredEmail)
                {
                    var emailVerificationForReset = new EmailValidation(returnedEmail);
                    if (emailVerificationForReset.EmailIsValid == true)
                    {
                        Console.WriteLine("Lets reset your password...");
                        makeNewPassword();
                        return; // return ActionResult Ok
                    }
                }
                else
                {
                    return; //return ActionResult Conflict
                }
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
    public class HashingAlg
    {
        //add SHA256 secret, seperate with -, split on
        string hashString;
        public HashingAlg(string password)
        {
            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] bytes = Encoding.Unicode.GetBytes(password);
                byte[] hashValue = mySHA256.ComputeHash(bytes);
                foreach (byte x in hashValue)
                {
                    hashString += String.Format("{0:x2}", x);
                }
            }

        }
        public string getHash()
        {
            return hashString;
        }

        public string addSecret(string alreadyHashed)
        {
            alreadyHashed += "-";
            //Secret
            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] bytes = Encoding.Unicode.GetBytes("pokemon");
                byte[] hashValue = mySHA256.ComputeHash(bytes);
                foreach (byte x in hashValue)
                {
                    alreadyHashed += String.Format("{0:x2}", x);
                }
            }
            return alreadyHashed;
        }

        public string reomveSecret(string alreadyHashed)
        {
            int splitOn = alreadyHashed.IndexOf("-");
            alreadyHashed = alreadyHashed.Substring(0, splitOn);
            return alreadyHashed;
        }
    }
    public class EmailValidation
    {
        public string Email { get; set; }
        public bool EmailIsValid { get; set; }
        public string RandomCode { get; set; }
        public static bool SendSuccess { get; set; }

        public EmailValidation(string email)
        {
            Email = email;
            RandomCode = RandoStringBuilder();
            EmailIsValid = EmailFormCheck(email);
        }
        public static async Task<bool> SendEmail(string email, string code)
        {
            try
            {
                var mailMessage = new MimeMessage();
                mailMessage.From.Add(new MailboxAddress("Pokemanz Password Reset", "pokemanz2450@gmail.com"));
                mailMessage.To.Add(new MailboxAddress("Trainer", email));
                mailMessage.Subject = "Password verification code";
                mailMessage.Body = new TextPart("plain")
                {
                    Text = "Here is your verification code: \n\n" + code
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync("pokemanz2450@gmail.com", "13juliet");
                    await client.SendAsync(mailMessage);
                    await client.DisconnectAsync(true);
                }
                SendSuccess = true;
            }
            catch
            {
                SendSuccess = false;
            }
            return SendSuccess;
        }

        public static bool EmailFormCheck(string email)
        {
            Boolean isValid = System.Text.RegularExpressions.Regex.IsMatch(email,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-0-9a-zA-Z]*[0-9a-zA-Z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");
            return isValid;
        }

        public string RandoStringBuilder()
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
    }
}
