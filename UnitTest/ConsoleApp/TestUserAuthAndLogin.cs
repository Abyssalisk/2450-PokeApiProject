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
        ConsoleOutputInput c = new ConsoleOutputInput("blah");
        DBconnect connection = new DBconnect();
        CreateNewUser user = new CreateNewUser();

        [Fact]
        public void ValidateFalse()
        {
            Assert.False(c.Validate("username", "mypassword", connection.myConnection));
        }

        [Fact]
        public void ValidateTrue()
        {
            Assert.True(c.Validate("srosy", "myballsaresrosy", connection.myConnection));
        }

        [Fact]
        public void UserNameValidationFalse()
        {
            Assert.False(user.userNameValidation("Derek", connection.myConnection));
        }

        [Fact]
        public void UserNameValidationTrue()
        {
            Assert.True(user.userNameValidation("ScoobyDoo", connection.myConnection));
        }

        [Fact]
        public void UserPasswordHashFalse()
        {
            Assert.False(user.UserPasswordHash("password").Equals("password"));
        }

        [Fact]
        public void UserPasswordHashTrue()
        {
            Assert.True(user.UserPasswordHash("password").Equals("e201065d0554652615c320c00a1d5bc8edca469d72c2790e24152d0c1e2b6189-f57b2598c0d81870249bf8ca6190a41d3d8223d672484941d3a2c1302456ee80"));
        }
    }
}
