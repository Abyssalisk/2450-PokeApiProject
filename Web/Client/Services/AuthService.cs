using MailKit.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Web.Shared.Models;

namespace Web.Client.Services
{
    public class AuthService
    {
        public string RandomCode { get; set; }
        public string Email { get; set; }

        [Inject]
        HttpClient Client { get; set; }

        public AuthService(HttpClient client)
        {
            Client = client;
        }

        public void SendEmail(string email, string code)
        {
            Task.Run(() => Client.GetStringAsync($"api/Login/validate?email={email}&code={code}"));
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
