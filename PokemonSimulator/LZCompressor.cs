using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PokemonSimulator
{
    /// <summary>
    /// Author: Samuel Gardner
    /// </summary>
    public static partial class Grand
    {
        #region Typedefs
        /// <summary>
        /// Author: Samuel Gardner<br/><br/>
        /// 
        /// This compression algorithem relies upon multiple copies of substrings that occur in the string to be compressed for its efficency. (Works best on things that contain
        /// multiple copies of substrings.)
        /// </summary>
        public class LZCompressor
        {
            /// <summary>
            /// Initializes an instance of <see cref="LZCompressor"/>. This style of compresson works best on strings that have common occurances of substrings, 
            /// (Natural english works well, and JSON works well too). Essentially, If you have a string that is 8 instances of the same phrase over and over again, 
            /// this compressor can compress that string to slightly more than 1/8th of the original size. (This is case sensitive though). This function's efficency is measured
            /// in exponential time, as opposed to polynomial time. (Meaning, if you pass a string n characters long and a string m characters long, the second compression will
            /// take (2^sqrt(m)/2^sqrt(n)) times as long as the first, assuming you left <paramref name="maxSubstringLength"/> and <paramref name="settleForBestMaxTime"/> to default). 
            /// This trade off is made because the implementation this way was easier, and is technechally perfect at finding the best compression table, assuming no time stipulation.
            /// </summary>
            /// <param name="reservedDelimiter">This string must be the smallest (ideally) string possible which isn't contained in any of the string(s) to be compressed by this object. 
            /// If you want to compress a string and the <paramref name="reservedDelimiter"/> that this object was initialized with is contained in the string, you must change the 
            /// <see cref="Delimiter"/> property to a string that does not occur in the string. If this situation occurs, and the <see cref="Delimiter"/> property is not changed, an 
            /// exception will be thrown on the attempt to assign the string to the property <see cref="DecompressedObject"/>.</param>
            /// <param name="split">An int flag representing the boundary to the relative index of the reference and the length. LHS is relative index, RHS is reference length.
            /// This int flag should (ideally) be set to the flag value just larger than the length of the maximum substring that occurs more than once in the provided string, if you're unsure, 
            /// it's better to shoot higher than the length of the maximum length substring that occurs more than once as opposed to lower than it. This value can be changed after initialization; 
            /// <see cref="Split"/>, but doing so will invalidate the cached <see cref="CompressedObject"/> and <see cref="DecompressedObject"/> values. The default value assumes the longest
            /// substring of the string to be compressed that occurs more than once is less than or equal to 255 in length. (Exceeding the length of this value doesn't break the
            /// compression, it just makes it less efficent.)</param>
            /// <param name="maxSubstringLength">The maximum substring length to seek for when searching to replace duplicate instances. Ideally, this value would be no larger than the length of the
            /// longest substring that occurs in the string to be compressed more than once. This value can be changed after creation with the property <see cref="MaxSubstringLength"/>.
            /// If this value is not set upon initialization, it will be determined to be <c>(uint)Math.Sqrt(DecompressedObject.Length)</c> upon attempt to assign to the property
            /// <see cref="CompressedObject"/>. If this value is less than or equal to <c>reservedDelimiter.Length + 2</c>, the compression algorithm will perform no compression. 
            /// (i.e. <see cref="CompressedObject"/> and <see cref="DecompressedObject"/> will always be identical.)
            /// The largest gap possible between the values <c>reservedDelimiter.Length + 2</c> and <c>maxSubstringLength</c> is ideal, after accounting for execution time constraints.</param>
            /// <param name="settleForBestMaxTime">If the object is being compressed and the time it is taking to iterate through all possible permutations of substrings less than or equal to 
            /// <see cref="MaxSubstringLength"/> exceeds this time, the compression algorithm will stop and settle for the best compression table it's found so far. (Measured in ms).</param>
            public LZCompressor(string reservedDelimiter, int split = 0b00000000000000000000000010000000, uint? maxSubstringLength = null, uint? settleForBestMaxTime = null)
            {
                throw new NotImplementedException();
                if (split == 0 || ((int)(Math.Ceiling((Math.Log(split) / Math.Log(2)))) != (int)(Math.Floor(((Math.Log(split) / Math.Log(2)))))))
                {
                    throw new ArgumentException("Error: split must have one and only one flag set (split must be a power of 2).");
                }
                Delimiter = reservedDelimiter;
                MaxSubstringLength = maxSubstringLength;
                Split = split;
            }
            /// <summary>
            /// If compression calculation exceeds this time (in ms) during compression, futher table calculations are cancelled and the algorithem will settle with the best one it's found so far.
            /// </summary>
            public uint? SettleTime { get; set; } = null;
            /// <summary>
            /// The unique delimiter to demarcate a substring reference. This value is not permitted to occur in the decompressed string upon compression calculation.
            /// </summary>
            public string Delimiter { get; set; } = null;
            /// <summary>
            /// The maximum substring length to seek duplicate occurances for. Setting this value larger than <see cref="Split"/> is redundant.
            /// </summary>
            public uint? MaxSubstringLength { get; set; } = null;
            private int split;
            /// <summary>
            /// The flag value to split the bit region to be dedicated (on the left region) to relative index, and (on the right region) to reference string length. Setting this value
            /// uneccessarily larger than <see cref="MaxSubstringLength"/> is not recommended, but setting it lower than the length of the maximum length substring that occurs more than 
            /// once in the decompressed string is not ideal. Changing this value (IN THIS VERSION OF THIS FILE) invalidates the cached <see cref="CompressedObject"/> and 
            /// <see cref="DecompressedObject"/> values.
            /// </summary>
            public int Split
            {
                get => split;
                set
                {
                    //TODO:
                    //saving this value to the front of the compressed string would make it so that it doesn't invalidate the data, but that also means that the currently cached data will
                    //still be using the old split value, and using the new split value would require a recalculation of the compression (which we won't do).
                    compressedObject = decompressedObject = null;
                    split = value;
                }
            }
            private protected string compressedObject;
            /// <summary>
            /// The compressed form of the object. If this value was not set manually, but the <see cref="DecompressedObject"/> property was set, this value will be generated, 
            /// cached, and returned. Otherwise, this will return <c>null</c>.
            /// </summary>
            //public string CompressedObject
            //{
            //    get
            //    {

            //    }
            //    set
            //    {

            //    }
            //}
            private protected string decompressedObject;
            /// <summary>
            /// The decompressed form of the object. If this value was not set manually, but the <see cref="CompressedObject"/> property was set, this value will be generated, 
            /// cached, and returned. Otherwise, this will return <c>null</c>.
            /// </summary>
            //public string DecompressedObject
            //{
            //    get
            //    {

            //    }
            //    set
            //    {
            //        if (value.Contains(Delimiter))
            //        {
            //            throw new InvalidConstraintException("Error: the reserved delimiter is contained at least once in the provided string.");
            //        }
            //        else
            //        {

            //        }
            //    }
            //}
        }
        #endregion
    }
}