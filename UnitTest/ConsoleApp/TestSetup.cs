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
    public class TestSetup
    {
        public Trainer TestTrainer;
        public MySqlConnection TestConnection;
        public HashingAlg TestHashing;
        public TestSetup()
        {
            TestConnection = new MySqlConnection();
            TestTrainer = new Trainer();
            TestTrainer.TrainerName = "Fake_name";
            TestTrainer.UserId = 1;
            var list = new List<Pokemon>();
            var pokemon = new Pokemon();
            pokemon.Species = "pikachu";
            list.Add(pokemon) ;
            TestTrainer.Pokemon = list;
            TestHashing = new HashingAlg("1234");

        }
    }
}
