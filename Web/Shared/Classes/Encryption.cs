using System;
using System.Text;

namespace Web.Shared.Classes
{
    public class Encryption
    {
        public string EncryptedPassword { get; set; }
        private readonly string _secret;

        public Encryption()
        {
            _secret = "whabulabadubdub";
        }
        public Encryption(string password)
        {
            _secret = "whabulabadubdub";
            EncryptedPassword = Encrypt(password);
        }

        public string Encrypt(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password + _secret);
            return Convert.ToBase64String(bytes);
        }
        public string Decrypt(string encryptedPassword)
        {
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(encryptedPassword);
                return Encoding.UTF8.GetString(base64EncodedBytes).Replace(_secret, string.Empty);
            }
            catch { Console.WriteLine("Failed to decode password in LoginController.cs"); }
            return string.Empty;
        }
    }
}
