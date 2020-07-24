using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using System.ComponentModel;

namespace Web.Server.Classes
{
    public class Encryption
    {
        private static SHA384 hasher = SHA384.Create();
        private string hashedPassword = null;
        public string HashedPassword
        {
            get
            {
                return hashedPassword;
            }
        }
        private string password;
        public string Password
        {
            set
            {
                if (password != value)
                {
                    password = value;
                    hashedPassword = Hash(value, salt);
                }
            }
        }
        private string salt = string.Empty;
        public string Salt
        {
            set
            {
                if (salt != value)
                {
                    salt = value;
                    hashedPassword = Hash(password, salt);
                }
            }
        }
        private static string Hash(string inp, string salt)
        {
            byte[] br = hasher.ComputeHash((inp + salt).Select(x => Convert.ToByte(x)).ToArray());
            for (int i = 0; i < br.Length - 1; i++)
            {
                if (br[i] == br[i + 1] && br[i] == 0)
                {
                    throw new CryptographicException("Error: The hashing algorithim generated a null termination character. This is not allowed, choose a different password.");
                }
            }
            StringBuilder res = new StringBuilder(Convert.ToBase64String(br));
            res.Length = 300;
            res.Replace('\0', ' ');
            return res.ToString();
        }
        //        public string EncryptedPassword { get; set; }

        //        public Encryption()
        //        {

        //        }
        //        public Encryption(string password)
        //        {
        //            EncryptedPassword = Encrypt(password);
        //        }

        //#warning without any hashing this is totally reversable?? is a call to a hashing thing hidden in here somewhere?
        //        public string Encrypt(string password)
        //        {
        //            var bytes = Encoding.UTF8.GetBytes(password);
        //#warning It's technechally possible for bytes to encode to 8 consecutive null termination characters, when encoded to base 64 (and then used) I *think* absolutely breaks everything.
        //            return Convert.ToBase64String(bytes);
        //        }
        //        public string Decrypt(string encryptedPassword)
        //        {
        //            try
        //            {
        //                var base64EncodedBytes = Convert.FromBase64String(encryptedPassword);
        //                return Encoding.UTF8.GetString(base64EncodedBytes);
        //            }
        //            catch { Console.WriteLine("Failed to decode password in LoginController.cs"); }
        //            return string.Empty;
        //        }
    }
}
