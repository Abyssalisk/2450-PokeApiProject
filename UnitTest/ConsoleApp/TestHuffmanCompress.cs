using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static PokemonSimulator.Grand;

namespace UnitTest.ConsoleApp
{
    public class TestHuffmanCompress
    {
        [Fact]
        public void TenRandomPokemon()
        {
            HuffmanCoder huff = new HuffmanCoder();
            HuffmanCoder huffTwo = new HuffmanCoder();
            for (int i = 0; i < 10; i++)
            {
                string temp = huff.DecompressedObject = PokeAPI.DataFetcher.GetJsonOfAny(new Uri($"https://pokeapi.co/api/v2/pokemon/{rand.Next(808)}")).Result.ToJson();
                huffTwo.CompressedObject = huff.CompressedObject;
                Assert.True(huffTwo.DecompressedObject == temp);
            }
        }
    }
}