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
        readonly UserAuthAndLogin c = new UserAuthAndLogin(true);
        readonly DBconnect connection = new DBconnect();
        readonly CreateNewUser user = new CreateNewUser(true);

        [Fact]
        public void ValidateFalse()
        {
            Assert.False(c.Validate("username", "mypassword", connection.myConnection));
        }

        //[Fact]
        //public void ValidateTrue()
        //{
        //    Assert.True(c.Validate("srosy", "myballsaresrosy", connection.myConnection));
        //}

        [Fact]
        public void UserNameValidationFalse()
        {
            Assert.False(user.UserNameValidation("Derek", connection.myConnection));
        }

        [Fact]
        public void UserNameValidationTrue()
        {
            Assert.True(user.UserNameValidation("ScoobyDoo", connection.myConnection));
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
