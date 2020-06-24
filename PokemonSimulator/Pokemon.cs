using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System;
using System.Collections.Generic;
using PokeAPI;
using System;
using System.Collections.Generic;

using PokeAPI;
using Web.Shared.Models;


namespace PokemonSimulator
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Species { get; set; }
        public int BaseHP { get; set; }
        public int Speed { get; set; }
        public List<string> ConsoleTypes { get; set; } = new List<string>();
        public List<PokemonType> Types { get; set; } = new List<PokemonType>();
        public List<string> TypeWeaknesses { get; set; } = new List<string>();
        public List<MoveModel> Moves { get; set; } = new List<MoveModel>();
        public List<Move> ConsoleMoves { get; set; } = new List<Move>();
        public string BackImageUri { get; set; }
        public string FrontImageUri { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpecialAttack { get; set; }
        public int SpecialDefense { get; set; }

        //public MoveModel[] MoveSelections { get; set; } = new MoveModel[5]; // stores moves
        //public List<MoveModel> MoveSelections { get; set; } = new List<MoveModel>(); // stores moves
        public int ActingHP { get; set; }

        public bool IsAlive { get => ActingHP > 0; }




        #region Ctors
        public Pokemon()
        {

        }
        /// <summary>
        /// Deep copy constructor for Pokemon object.
        /// </summary>
        /// <param name="deep">The object to deep copy.</param>
        public Pokemon(Pokemon deep)
        {
            this.Id = deep.Id;
            this.Species = string.Copy(deep.Species ?? string.Empty);
            this.BaseHP = deep.BaseHP;
            this.Speed = deep.Speed;
            if (deep.ConsoleTypes != null)
            {
                foreach (string s in deep.ConsoleTypes)
                {
                    this.ConsoleTypes.Add(string.Copy(s ?? string.Empty));
                }
            }
            if (deep.Types != null)
            {
                foreach (PokemonType t in deep.Types)
                {
                    //Make this deep copy later.
                    this.Types.Add(t);
                }
            }
            if (deep.TypeWeaknesses != null)
            {
                foreach (string s in deep.TypeWeaknesses)
                {
                    this.TypeWeaknesses.Add(string.Copy(s ?? string.Empty));
                }
            }
            if (deep.Moves != null)
            {
                foreach (MoveModel m in deep.Moves)
                {
                    this.Moves.Add(new MoveModel()
                    {
                        Category = string.Copy(m.Category ?? string.Empty),
                        Damage = m.Damage,
                        Id = m.Id,
                        Name = string.Copy(m.Name ?? string.Empty),
                        ResourceUri = string.Copy(m.ResourceUri ?? string.Empty),
                        Seleted = m.Seleted,
                        Type = string.Copy(m.Type ?? string.Empty)
                    });
                }
            }
            if (deep.ConsoleMoves != null)
            {
                foreach (Move m in deep.ConsoleMoves)
                {
                    this.ConsoleMoves.Add(new Move() { Damage = m.Damage, IsPhysical = m.IsPhysical, Name = string.Copy(m.Name ?? string.Empty), Type = string.Copy(m.Type ?? string.Empty) });
                }
            }
            this.BackImageUri = string.Copy(deep.BackImageUri ?? string.Empty);
            this.FrontImageUri = string.Copy(deep.FrontImageUri ?? string.Empty);
            this.Attack = deep.Attack;
            this.Defense = deep.Defense;
            this.SpecialAttack = deep.SpecialAttack;
            this.SpecialDefense = deep.SpecialDefense;
            this.ActingHP = deep.ActingHP;
        }
        //public Pokemon(API.PokemonBlueprint model)
        //{
        //    string[] enumStrings = Enum.GetNames(typeof(PokemonType));
        //    for (int i = 0; i < model.types.Length; i++)
        //    {
        //        for (int j = 0; j < enumStrings.Length; j++)
        //        {
        //            Type |= ((enumStrings[j].ToLower() == model.types[i].type.name) ? ((PokemonType)Math.Pow(2, j)) : (PokemonType)0);
        //        }
        //    }
        //    Species = model.name;
        //    //Nickname = Nickname ?? Species;
        //    ActingHP = BaseHP = Array.Find(model.stats, x => x.stat.name == "hp").base_stat;
        //    //ActingSpeed = BaseSpeed = Array.Find(model.stats, x => x.stat.name == "speed").base_stat;
        //    //ActingAttack = BaseAttack = Array.Find(model.stats, x => x.stat.name == "attack").base_stat;
        //    //ActingDefense = BaseDefense = Array.Find(model.stats, x => x.stat.name == "defense").base_stat;
        //    //ActingSpecialAttack = BaseSpecialAttack = Array.Find(model.stats, x => x.stat.name == "special-attack").base_stat;
        //    //ActingSpecialDefense = BaseSpecialDefense = Array.Find(model.stats, x => x.stat.name == "special-defense").base_stat;
        //    //Temporary.
        //    if (model.moves.Length > 3)
        //    {
        //        FirstMove = model.moves[0].move.name;
        //        SecondMove = model.moves[1].move.name;
        //        ThirdMove = model.moves[2].move.name;
        //        FourthMove = model.moves[3].move.name;
        //    }
        //    else if (model.moves.Length > 2)
        //    {
        //        FirstMove = model.moves[0].move.name;
        //        SecondMove = model.moves[1].move.name;
        //        ThirdMove = model.moves[2].move.name;
        //    }
        //    else if (model.moves.Length > 1)
        //    {
        //        FirstMove = model.moves[0].move.name;
        //        SecondMove = model.moves[1].move.name;
        //    }
        //    else if (model.moves.Length > 0)
        //    {
        //        FirstMove = model.moves[0].move.name;
        //    }
        //}
        #endregion

        //public void RequestFieldChange(Pokemon proposedModel /*handle to conversation*/)
        //{
        //    //notify server and opponent that your pokemon differs from the template.
        //    throw new NotImplementedException();
        //}
        /// <summary>
        /// Modifies the health by adding the value provided to the pokemon's current health value.
        /// </summary>
        /// <param name="value">The value to add or subtract from the health value</param>
        [Obsolete("For testing only.")]
        public void ModifyHealth(int value)
        {
            ActingHP += value;
            if (ActingHP < 0)
            {
                ActingHP = 0;
            }
        }

        #region Redefinitions/Overrides
        public new string ToString()
        {
            return
                $" species\' is {Species}, types are {string.Join(", ", Types.Select(x => x.Name))}, stats are BaseHP:{BaseHP}" +
                $" BaseSpeed:{Speed} BaseAttack:{Attack} BaseDefense:{Defense}" +
                $" BaseSpecialAttack:{SpecialAttack} BaseSpecialDefense:{SpecialDefense},";
        }
        #endregion

        #region Class Methods
        //public static VunerabilityLevel GetVunerability(PokemonType attack, PokemonType defense)
        //{
        //    switch (attack)
        //    {

        //    }
        //    throw new NotImplementedException();
        //}
        #endregion
    }
}