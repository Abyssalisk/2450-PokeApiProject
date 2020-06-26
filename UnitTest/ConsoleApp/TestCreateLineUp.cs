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
        private CreateLineUp _LineUpHasLineUp;
        private CreateLineUp _LineUpNoLineUp;
        private readonly TestSetup _Environment;
        public TestCreateLineUp()
        {
            _Environment = new TestSetup();
        }

        [Fact]
        public void TestCreateLineUpInstansation()
        {
            _LineUpHasLineUp = new CreateLineUp(_Environment.TestTrainer, _Environment.TestConnection,true);
            _LineUpNoLineUp = new CreateLineUp(_Environment.TestTrainer, _Environment.TestConnection, false);
        }
        
        [Fact]
        public void TestReadName()
        {
            Assert.NotNull(_LineUpHasLineUp.SearchedName);
            Assert.NotNull(_LineUpNoLineUp.SearchedName);
        }

        [Fact]
        public void TestSearchPokemonAsync()
        {
            Assert.NotNull(_LineUpHasLineUp.ReturnedName);
            Assert.NotNull(_LineUpNoLineUp.ReturnedName);

            Assert.DoesNotContain("-1", _LineUpHasLineUp.ReturnedName);
            Assert.DoesNotContain("-1", _LineUpNoLineUp.ReturnedName);
        }

        [Fact]
        public void TestPokeFinder()
        {
            Assert.True(_LineUpHasLineUp.ValidPokemon);
            Assert.True(_LineUpNoLineUp.ValidPokemon);
        }

    }
}
