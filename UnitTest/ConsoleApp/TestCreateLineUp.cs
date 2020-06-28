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

    }
}