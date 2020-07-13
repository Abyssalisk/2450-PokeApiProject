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
    public class TestMoveSelector2000
    {
        [Fact]
        public void TestDisplayMovesTrue()
        {
            var _testMoves = new MoveSelector2000("pikachu");
            Assert.True( _testMoves.DisplayMoves());
            Assert.NotNull(_testMoves.AvailbleMoves);
        }
        [Fact]
        public void TestDisplayMovesFalse()
        {
            var _testMoves = new MoveSelector2000("me");
            Assert.False(_testMoves.DisplayMoves());
            Assert.Empty(_testMoves.AvailbleMoves);
        }
    }
}
