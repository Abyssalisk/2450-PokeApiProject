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
using Web.Server.Classes;
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

            return NewUser.DoUserCreation(cam);
        }

        [HttpGet]
        public string Login([FromQuery] string username, [FromQuery] string password)
        {
            var accountIsAuth = Validate(username, password, new DBInterface().MyConnection);

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

                    var con = new DBInterface().MyConnection;
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
}
