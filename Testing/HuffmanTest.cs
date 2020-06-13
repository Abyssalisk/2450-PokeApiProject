using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokemonSimulator;

namespace Testing
{
    [TestClass]
    public class HuffmanTest
    {
        [TestMethod]
        public void SampleHuffOne()
        {
            const string testString = "abcabcabcaaaaabbbbb";
            Assert.AreEqual(Grand.HuffmanDeserialize(Grand.HuffmanSerialize(testString)), testString);
        }
        [TestMethod]
        public void SampleHuffTwo()
        {
            const string testString = "[\']banana[\']aaannb[\']";
            Assert.AreEqual(Grand.HuffmanDeserialize(Grand.HuffmanSerialize(testString)), testString);
        }
        [TestMethod]
        public void SampleHuffThree()
        {
            const string testString = "Lorem ipsum dolor sit amet means roughly \'Lorem very pain let it be carrots\'. " +
                "Interesting, Huh? []I[]Like[]Brackets[]Very[]Much[] and you can use them as the letter O, but boxy. " +
                "H[]wdy there, my []ctopus like fr[][]t l[][]ps.";
            Assert.AreEqual(Grand.HuffmanDeserialize(Grand.HuffmanSerialize(testString)), testString);
        }
        [TestMethod]
        public void SampleHuffFour()
        {
            const string testString = "aaaaaaaaaaaaaaaa0000000000000000";
            Assert.AreEqual(Grand.HuffmanDeserialize(Grand.HuffmanSerialize(testString)), testString);
        }
        [TestMethod]
        public void BigHuffTest()
        {
            string api = string.Empty;
            string result = string.Empty;
            api = APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()).ToString();
            result = Grand.HuffmanDeserialize(Grand.HuffmanSerialize(api));
            Assert.AreEqual(result, api);
            api = APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()).ToString();
            result = Grand.HuffmanDeserialize(Grand.HuffmanSerialize(api));
            Assert.AreEqual(result, api);
            api = APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()).ToString();
            result = Grand.HuffmanDeserialize(Grand.HuffmanSerialize(api));
            Assert.AreEqual(result, api);
            api = APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()).ToString();
            result = Grand.HuffmanDeserialize(Grand.HuffmanSerialize(api));
            Assert.AreEqual(result, api);
            api = APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()).ToString();
            result = Grand.HuffmanDeserialize(Grand.HuffmanSerialize(api));
            Assert.AreEqual(result, api);
            api = APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()).ToString();
            result = Grand.HuffmanDeserialize(Grand.HuffmanSerialize(api));
            Assert.AreEqual(result, api);
            api = APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()).ToString();
            result = Grand.HuffmanDeserialize(Grand.HuffmanSerialize(api));
            Assert.AreEqual(result, api);
            api = APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()).ToString();
            result = Grand.HuffmanDeserialize(Grand.HuffmanSerialize(api));
            Assert.AreEqual(result, api);
            api = APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()).ToString();
            result = Grand.HuffmanDeserialize(Grand.HuffmanSerialize(api));
            Assert.AreEqual(result, api);
            api = APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString()).ToString();
            result = Grand.HuffmanDeserialize(Grand.HuffmanSerialize(api));
            Assert.AreEqual(result, api);
        }
    }
}