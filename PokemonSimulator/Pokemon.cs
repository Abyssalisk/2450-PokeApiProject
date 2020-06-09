using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonSimulator
{
    /// <summary>
    /// Author: Samuel Gardner
    /// </summary>
    class Pokemon
    {
        #region Fields
        public PokemonType Type { get; private set; } = (PokemonType)0;
        public string Species { get; private set; }
        public string Nickname { get; private set; } = null;
        public int BaseHP { get; private set; }
        public int BaseSpeed { get; private set; }
        public int BaseAttack { get; private set; }
        public int BaseDefense { get; private set; }
        public int BaseSpecialAttack { get; private set; }
        public int BaseSpecialDefense { get; private set; }
        public string FirstMove { get; private set; } = string.Empty;
        public string SecondMove { get; private set; } = string.Empty;
        public string ThirdMove { get; private set; } = string.Empty;
        public string FourthMove { get; private set; } = string.Empty;
        #endregion

        #region Ctors
        public Pokemon(APIPokemonBlueprint model)
        {
            string[] enumStrings = Enum.GetNames(typeof(PokemonType));
            for (int i = 0; i < model.types.Length; i++)
            {
                for (int j = 0; j < enumStrings.Length; j++)
                {
                    Type |= ((enumStrings[j].ToLower() == model.types[i].type.name) ? ((PokemonType)Math.Pow(2, j)) : (PokemonType)0);
                }
            }
            Species = model.name;
            Nickname = Nickname ?? Species;
            BaseHP = Array.Find(model.stats, x => x.stat.name == "hp").base_stat;
            BaseSpeed = Array.Find(model.stats, x => x.stat.name == "speed").base_stat;
            BaseAttack = Array.Find(model.stats, x => x.stat.name == "attack").base_stat;
            BaseDefense = Array.Find(model.stats, x => x.stat.name == "defense").base_stat;
            BaseSpecialAttack = Array.Find(model.stats, x => x.stat.name == "special-attack").base_stat;
            BaseSpecialDefense = Array.Find(model.stats, x => x.stat.name == "special-defense").base_stat;
            if (model.moves.Length > 3)
            {
                FirstMove = model.moves[0].move.name;
                SecondMove = model.moves[1].move.name;
                ThirdMove = model.moves[2].move.name;
                FourthMove = model.moves[3].move.name;
            }
            else if (model.moves.Length > 2)
            {
                FirstMove = model.moves[0].move.name;
                SecondMove = model.moves[1].move.name;
                ThirdMove = model.moves[2].move.name;
            }
            else if (model.moves.Length > 1)
            {
                FirstMove = model.moves[0].move.name;
                SecondMove = model.moves[1].move.name;
            }
            else if (model.moves.Length > 0)
            {
                FirstMove = model.moves[0].move.name;
            }
        }
        #endregion

        #region Methods
        public void RequestFieldChange(Pokemon proposedModel /*handle to conversation*/)
        {
            //notify server and opponent that your pokemon differs from the template.
        }
        #endregion

        #region Redefinitions/Overrides
        public new string ToString()
        {
            return $"{Nickname.ToPascalCase(System.Globalization.CultureInfo.CurrentCulture)}" +
                $" species\' is {Species}, types are {Type.ToString()}, stats are BaseHP:{BaseHP}" +
                $" BaseSpeed:{BaseSpeed} BaseAttack:{BaseAttack} BaseDefense:{BaseDefense}" +
                $" BaseSpecialAttack:{BaseSpecialAttack} BaseSpecialDefense:{BaseSpecialDefense}," +
                $" moves are {((FirstMove == string.Empty) ? "No move in this slot" : FirstMove)};" +
                $" {((SecondMove == string.Empty) ? "No move in this slot" : SecondMove)};" +
                $" {((ThirdMove == string.Empty) ? "No move in this slot" : ThirdMove)};" +
                $" {((FourthMove == string.Empty) ? "No move in this slot" : FourthMove)}.";
        }
        #endregion
    }
}