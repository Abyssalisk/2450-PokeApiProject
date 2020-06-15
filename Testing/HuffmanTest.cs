using Google.Protobuf.WellKnownTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokemonSimulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Testing
{
    [TestClass]
    public class HuffmanTest
    {
        public class Ref<T>
        {
            public T Value { get; set; }
        }
        [TestMethod]
        public void SampleHuffOne()
        {
            const string testString = @"abcabcabcaaaaabbbbb";
            Assert.AreEqual(testString, Grand.HuffmanDecompress(Grand.HuffmanCompress(testString)));
        }
        [TestMethod]
        public void SampleHuffTwo()
        {
            const string testString = @"[']banana[']aaannb[']";
            Assert.AreEqual(testString, Grand.HuffmanDecompress(Grand.HuffmanCompress(testString)));
        }
        [TestMethod]
        public void SampleHuffThree()
        {
            const string testString = @"Lorem ipsum dolor sit amet means roughly 'Lorem very pain let it be carrots'. " +
                "Interesting, Huh? []I[]Like[]Brackets[]Very[]Much[] and you can use them as the letter O, but boxy. " +
                "H[]wdy there, my []ctopus like fr[][]t l[][]ps.";
            Assert.AreEqual(testString, Grand.HuffmanDecompress(Grand.HuffmanCompress(testString)));
        }
        /// <summary>
        /// This one is to check an edge case bug I suspect might exist.
        /// </summary>
        [TestMethod]
        public void SampleHuffFour()
        {
            const string testString = "aaaaaaaaaaaaaaaa0000000000000000aaaaaaaaaaaaaaaa";
            Assert.AreEqual(testString, Grand.HuffmanDecompress(Grand.HuffmanCompress(testString)));
        }
        /// <summary>
        /// Mass testing on large json objects.
        /// </summary>
        [TestMethod]
        public void BigHuffTest()
        {
            //string api = string.Empty;
            //string result = string.Empty;
            //Func<object?, string> getPoke = async (object? o) => await ((object? o) => APIPokemonBlueprint.GetPokemonBlueprint(o.ToString()).ToString());
            //Task<string> task = new Task<string>(getPoke, Grand.rand.Next(1, 785).ToString());
            //IEnumerable GetPokemon()
            //{
            //    yield return await task;
            //}
            //Parallel.ForEach(Enumerable.Repeat()
            Grand.HuffmanCoder<Ref<APIPokemonBlueprint>> coder1 = new Grand.HuffmanCoder<Ref<APIPokemonBlueprint>>();
            coder1.DecompressedObject = new Ref<APIPokemonBlueprint> { Value = APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()) };
            Grand.HuffmanCoder<Ref<APIPokemonBlueprint>> coder2 = new Grand.HuffmanCoder<Ref<APIPokemonBlueprint>>();
            coder2.CompressedObject = coder1.CompressedObject;
            Assert.IsTrue(coder1.DecompressedObject.Value.Equals(coder2.DecompressedObject.Value));
            Assert.AreEqual(coder1.CompressedObject, coder2.CompressedObject);
            //ulong charCount = 0;
            //api = @APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()).ToString();
            //charCount = charCount + (ulong)api.Length;
            //result = Grand.HuffmanDecompress(Grand.HuffmanCompress(api));
            //Assert.AreEqual(api, result);
            //api = @APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()).ToString();
            //charCount = charCount + (ulong)api.Length;
            //result = Grand.HuffmanDecompress(Grand.HuffmanCompress(api));
            //Assert.AreEqual(api, result);
            //api = @APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()).ToString();
            //charCount = charCount + (ulong)api.Length;
            //result = Grand.HuffmanDecompress(Grand.HuffmanCompress(api));
            //Assert.AreEqual(api, result);
            //api = @APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()).ToString();
            //charCount = charCount + (ulong)api.Length;
            //result = Grand.HuffmanDecompress(Grand.HuffmanCompress(api));
            //Assert.AreEqual(api, result);
            //api = @APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()).ToString();
            //charCount = charCount + (ulong)api.Length;
            //result = Grand.HuffmanDecompress(Grand.HuffmanCompress(api));
            //Assert.AreEqual(api, result);
            //api = @APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()).ToString();
            //charCount = charCount + (ulong)api.Length;
            //result = Grand.HuffmanDecompress(Grand.HuffmanCompress(api));
            //Assert.AreEqual(api, result);
            //api = @APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()).ToString();
            //charCount = charCount + (ulong)api.Length;
            //result = Grand.HuffmanDecompress(Grand.HuffmanCompress(api));
            //Assert.AreEqual(api, result);
            //api = @APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()).ToString();
            //charCount = charCount + (ulong)api.Length;
            //result = Grand.HuffmanDecompress(Grand.HuffmanCompress(api));
            //Assert.AreEqual(api, result);
            //api = @APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()).ToString();
            //charCount = charCount + (ulong)api.Length;
            //result = Grand.HuffmanDecompress(Grand.HuffmanCompress(api));
            //Assert.AreEqual(api, result);
            //api = @APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()).ToString();
            //charCount = charCount + (ulong)api.Length;
            //result = Grand.HuffmanDecompress(Grand.HuffmanCompress(api));
            //Assert.AreEqual(api, result);
            //ulong mbs = charCount / 524288;
            //;
        }
    }
}