using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace PokemonSimulator
{
    /// <summary>
    /// Author: Samuel Gardner
    /// </summary>
    public static partial class Grand
    {
        #region Fields
        #region Consts
        private const string yesRegex = @"^[y|Ye|Es|S]|[y|Y]";
        /// <summary>
        /// Regex field that matches common permutations of "yes". Regex value is: <see cref="yesRegex"/>.
        /// </summary>
        public static readonly Regex yes = new Regex(yesRegex);
        private const string noRegex = @"^[n|No|O]|[n|N]";
        /// <summary>
        /// Regex field that matches common permutations of "no". Regex value is: <see cref="noRegex"/>.
        /// </summary>
        public static readonly Regex no = new Regex(noRegex);
        #endregion

        #region Class Fields
        public readonly static HashAlgorithm sha;
#if DEBUG
        public readonly static Random rand;
#endif
        #endregion
        #endregion

        #region Constructor
        /// <summary>
        /// Static Ctor for Grand, creates the SHA object, sets and initializes the RAND object, and sets up the static Dtor.
        /// </summary>
        static Grand()
        {
            sha = new SHA1CryptoServiceProvider();
            rand = new Random(((DateTime.Now.Millisecond - 500) * 17) / 5);
            AppDomain.CurrentDomain.ProcessExit += GrandDestructor;
        }
        /// <summary>
        /// Static Dtor for Grand.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void GrandDestructor(object sender, EventArgs e)
        {
            sha.Dispose();
        }
        #endregion

        //public static bool VerifyPokemonLegitimacy(API.PokemonBlueprint mine, API.PokemonBlueprint theirs)
        //{
        //    byte[] mh = sha.ComputeHash(Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(mine, typeof(API.PokemonBlueprint), Formatting.None, null)));
        //    byte[] th = sha.ComputeHash(Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(theirs, typeof(API.PokemonBlueprint), Formatting.None, null)));
        //    for (int i = 0; i < mh.Length; i++)
        //    {
        //        if (mh[i] != th[i])
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
        [Obsolete("Create your own instance of HuffmanCoder instead.")]
        public static string HuffmanCompress(string decompressed)
        {
            HuffmanCoder h = new HuffmanCoder();
            h.DecompressedObject = decompressed;
            return h.CompressedObject;
        }
        [Obsolete("Create your own instance of HuffmanCoder instead.")]
        public static string HuffmanCompress<T>(T decompressed) where T : class
        {
            HuffmanCoder<T> h = new HuffmanCoder<T>();
            h.DecompressedObject = decompressed;
            return h.CompressedObject;
        }
        [Obsolete("Create your own instance of HuffmanCoder instead.")]
        public static string HuffmanDecompress(string huffCompressed)
        {
            HuffmanCoder h = new HuffmanCoder();
            h.CompressedObject = huffCompressed;
            return h.DecompressedObject;
        }
        [Obsolete("Create your own instance of HuffmanCoder instead.")]
        public static T HuffmanDecompress<T>(string huffCompressed) where T : class
        {
            HuffmanCoder<T> h = new HuffmanCoder<T>();
            h.CompressedObject = huffCompressed;
            return h.DecompressedObject;
        }
    }
}