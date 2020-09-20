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
using System.Linq;
using System.Net;
using System.Net.Mail;

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
            var accountIsAuth = Validate(username, password);

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
                mailMessage.From.Add(new MailboxAddress("Poke", "pokemanz.project@gmail.com"));
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

                    var query = $"SELECT TOP 1 UserName, Password FROM Users WHERE Email='{email}';";
                    var DS = DBConnect.GetDataSet(query);

                    if (DS.Tables[0].Rows.Count > 0)
                    {
                        LoginModel.Username = DS.Tables[0].Rows[0]["UserName"].ToString();
                        LoginModel.Password = new Encryption().Decrypt(DS.Tables[0].Rows[0]["Password"].ToString());

                        mailMessage.Subject = "PokeManz Credentials";
                        textpart.Text = $"Your login credentials are\n\nUsername: {LoginModel.Username}\nPassword: {LoginModel.Password}";
                        
                    }
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
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTlsWhenAvailable);
                await client.AuthenticateAsync("pokemanz.srosy@gmail.com", "GaviSpe64!");
                await client.SendAsync(mailMessage);
                await client.DisconnectAsync(true);
            }
        }

        public bool Validate(string username, string password)
        {
            string query = $"SELECT [Password] FROM Users WHERE UserName = '{username}';";
            var data = DBConnect.GetSingleString(query);

            if (string.IsNullOrEmpty(data)) return false;
            var decryptedPass = new Encryption().Decrypt(data);
            return password.Equals(decryptedPass);
        }
    }
}
