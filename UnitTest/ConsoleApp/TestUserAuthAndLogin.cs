using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions.Common;
using PokemonSimulator;
using System.Runtime.Intrinsics.X86;

namespace UnitTest.ConsoleApp
{
    public class TestUserAuthAndLogin
    {
        [Fact]
        public void ValidateFalse()
        {
            PokemonSimulator.ConsoleOutputInput  pc = new PokemonSimulator.ConsoleOutputInput("blah");
            var connection = new DBconnect();

            Assert.False(pc.Validate("username", "mypassword", connection.myConnection));
        }

        [Fact]
        public void ValidateTrue()
        {
            PokemonSimulator.ConsoleOutputInput pc = new PokemonSimulator.ConsoleOutputInput("blah");
            var connection = new DBconnect();

            Assert.True(pc.Validate("username", "mypassword", connection.myConnection));
        }
    }
}
