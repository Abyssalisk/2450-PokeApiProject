using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions.Common;
using PokemonSimulator;
using System.Runtime.Intrinsics.X86;

namespace UnitTest.ConsoleApp
{
    public class TestHashingAlg
    {
        public readonly TestSetup _Environment = new TestSetup();

        [Fact]
        public void TestBasicSHA256()
        {
            Assert.NotNull(_Environment.TestHashing.getHash());
        }
    }
}
