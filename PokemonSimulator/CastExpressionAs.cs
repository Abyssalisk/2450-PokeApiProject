using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonSimulator
{
    partial class Grand
    {
        public static Result CastExpressionAs<True, False, Result>(this bool condition, True trueVal, False falseVal) where True : Result where False : Result => (condition ? (Result)trueVal : (Result)falseVal);
    }
}