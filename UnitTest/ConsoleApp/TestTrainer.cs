using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions.Common;
using PokemonSimulator;
using System.Runtime.Intrinsics.X86;

namespace UnitTest.ConsoleApp
{
    public class TestTrainer
    {
        public readonly Trainer _Trainer;
        public readonly TestSetup _Environment = new TestSetup();

        [Fact]
        public void TestGettersAndSetters()
        {
            Assert.Equal("Fake_name",_Environment.TestTrainer.TrainerName);
            Assert.Equal(1, _Environment.TestTrainer.UserId);

            _Environment.TestTrainer.TrainerName = "replace";
            _Environment.TestTrainer.UserId = 2;

            Assert.Equal("replace", _Environment.TestTrainer.TrainerName);
            Assert.Equal(2, _Environment.TestTrainer.UserId);
        }
        [Fact]
        public void TestPokemonList()
        {
            Assert.Single(_Environment.TestTrainer.Pokemon);
        }
    }
}
