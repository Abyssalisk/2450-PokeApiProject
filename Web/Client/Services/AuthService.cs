using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Web.Shared.Models;

namespace Web.Client.Services
{

    public interface IAuthService
    {
        string RandomCode { get; set; }
        string Email { get; set; }
        bool CodeSent { get; set; }
    }

    public class AuthService : IAuthService
    {
        public string RandomCode { get; set; }
        public string Email { get; set; }
        public bool CodeSent { get; set; }


        public AuthService() { }

        public async Task<string> TryLogin(HttpClient client, LoginModel login)
        {
            var result = await client.GetStringAsync($"api/login?username={login.Username}&password={login.Password}");
            return result;
        }

        public void SendLogin(HttpClient client, SendLoginModel model)
        {
            Task.Run(() => client.GetStringAsync($"api/login/sendlogin/email={model.Email}"));
        }

        public void CreateAccount(HttpClient client, CreateAccountModel createAccount)
        {
            Task.Run(() => client.GetStringAsync($"api/login/createaccount?name={createAccount.Username}&pass={createAccount.Password}&email={createAccount.Email}"));
        }

        public void SendEmail(HttpClient client, string email, string code)
        {
            Task.Run(() => client.GetStringAsync($"https://srosy.azurewebsites.net/api/login/validate?email={email}&code={code}"));
        }

        public static bool EmailFormCheck(string email)
        {
            Boolean isValid = System.Text.RegularExpressions.Regex.IsMatch(email,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-0-9a-zA-Z]*[0-9a-zA-Z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");
            return isValid;
        }

        public string GenRandomString()
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
