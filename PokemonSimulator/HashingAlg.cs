using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace PokemonSimulator
{
    class HashingAlg
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
}
