using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using MimeKit;
using MailKit.Security;

namespace PokemonSimulator
{
    public class EmailValidation
    {
        public Boolean emailIsInCorrectForm;
        public Boolean emailIsValid;
        private string randomCode;

        public EmailValidation(string email)
        {
            randomCode = randoStringBuilder();
            emailIsInCorrectForm = emailFormCheck(email);
            sendEmail(randomCode, email);
            emailIsValid = emailVerificationCode(email,randomCode);
        }
        private async void sendEmail(string verificationCode, string email)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("Pokemanz Password Reset", "pokemanz2450@gmail.com"));
            mailMessage.To.Add(new MailboxAddress("Trainer", email));
            mailMessage.Subject = "Password verification code";
            mailMessage.Body = new TextPart("plain")
            {
                Text = "Here is your verification code: \n\n" + verificationCode
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("pokemanz2450@gmail.com", "13juliet");
                await client.SendAsync(mailMessage);
                await client.DisconnectAsync(true);
            }
        }

        private Boolean validateEmailCode()
        {
            string codeEntered;
            Console.WriteLine("Enter email validation code: ");
            codeEntered = Console.ReadLine();
            if (codeEntered == randomCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Boolean emailFormCheck(string email)
        {
            Boolean isValid = System.Text.RegularExpressions.Regex.IsMatch(email,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-0-9a-zA-Z]*[0-9a-zA-Z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");
            return isValid;
        }

        private string randoStringBuilder()
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

        private Boolean emailVerificationCode(string email, string randomCode)
        {
            Console.WriteLine("Email with verifiaction code has been sent, please check your email....");
            if (validateEmailCode() == true)
            {
                Console.WriteLine("Email has been verified");
                return true;
            }else
            {
                Console.WriteLine("Incorrect verfication code! Lets try this again");
                return false;
            }
        }
    }
}
