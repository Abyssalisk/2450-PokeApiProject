using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Linq;

namespace PokemonSimulator
{
    public static class Grand
    {
        public readonly static HashAlgorithm sha = new SHA1CryptoServiceProvider();
        public static bool VerifyPokemonLegitimacy(APIPokemonBlueprint mine, APIPokemonBlueprint theirs)
        {        
            byte[] mh = sha.ComputeHash(Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(mine, typeof(APIPokemonBlueprint), Formatting.None, null)));
            byte[] th = sha.ComputeHash(Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(theirs, typeof(APIPokemonBlueprint), Formatting.None, null)));
            for (int i = 0; i < mh.Length; i++)
            {
                if (mh[i] != th[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}