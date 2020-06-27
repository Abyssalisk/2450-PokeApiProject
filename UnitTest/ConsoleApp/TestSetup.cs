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
    class TestSetup
    {
        public Trainer TestTrainer;
        public MySqlConnection TestConnection;
        public TestSetup()
        {
            MockRepository mocks = new MockRepository();
            TestTrainer = mocks.StrictMock<Trainer>();
            TestConnection = mocks.StrictMock<MySqlConnection>();
        }
    }
}
