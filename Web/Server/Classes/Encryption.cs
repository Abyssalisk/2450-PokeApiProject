using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Server.Classes
{
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
            var bytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(bytes);
        }
        public string Decrypt(string encryptedPassword)
        {
            var base64EncodedBytes = Convert.FromBase64String(encryptedPassword);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
