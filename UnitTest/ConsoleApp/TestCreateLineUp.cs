using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions.Common;
using PokemonSimulator;
using System.Runtime.Intrinsics.X86;
using Rhino.Mocks;
using MySql.Data.MySqlClient;

namespace UnitTest.ConsoleApp
{
    public class TestCreateLineUp
    {
        private readonly TestSetup _Environment;
        private readonly CreateLineUp _TestLineUp;


        public TestCreateLineUp()
        {
            _Environment = new TestSetup();
            _TestLineUp = new CreateLineUp();
        }

        [Fact]
        public void TestSeachPokemonAsync()
        {
            Assert.True(_TestLineUp.SearchPokemonAsync("pikachu"));
            Assert.False(_TestLineUp.SearchPokemonAsync("spencer is the worst"));
        }

        [Fact]
        public void TestPokeFinder()
        {
            Assert.True(_TestLineUp.PokeFinder("pikachu"));
            Assert.False(_TestLineUp.PokeFinder("spencer is the worst"));
        }

    }
}