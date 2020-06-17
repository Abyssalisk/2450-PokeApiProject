using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonSimulator
{
    class TypeWeaknessMap
    {
        public Dictionary<string, List<string>> TypeMapSuper = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> TypeMapNotVery = new Dictionary<string, List<string>>();

        public TypeWeaknessMap()
        {
            //Only using one type for this not dual types. Dual types that the given move is super effective against
            //have one of the two types rooted in single type effectiveness. This single type effectiveness can be
            //used as a lookup for dual type effectiveness. Exceptions to this methodology, yes but very limited cases

            //Normal
            TypeMapNotVery.Add("Normal", new List<string> { "Ghost", "Rock","Steel" });

            //Fire
            TypeMapSuper.Add("Fire", new List<string> {"Grass","Ice" ,"Bug","Steel"});
            TypeMapNotVery.Add("Fire", new List<string> { "Water", "Rock" ,"Dragon"});


            //Water
            TypeMapSuper.Add("Water", new List<string> { "Fire", "Ground","Rock" });
            TypeMapNotVery.Add("Water", new List<string> { "Grass", "Dargon" });

            //Electric
            TypeMapSuper.Add("Electric", new List<string> { "Water", "Flying" });
            TypeMapNotVery.Add("Electric", new List<string> { "Ground" });


            //Grass
            TypeMapSuper.Add("Grass", new List<string> { "Water","Ground", "Rock" });
            TypeMapNotVery.Add("Grass", new List<string> { "Fire", "Poison","Bug","Dragon","Steel","Flying" });

            //Ice
            TypeMapSuper.Add("Ice", new List<string> { "Grass", "Ground","Flying","Dragon" });
            TypeMapNotVery.Add("Ice", new List<string> { "Fire", "Water","Steel" });


            //Fighting
            TypeMapSuper.Add("Fighting", new List<string> { "Ice", "Rock","Steel","Dark","Normal" });
            TypeMapNotVery.Add("Fighting", new List<string> { "Ghost", "Poison","Flying","Bug","Psychic" });


            //Poison
            TypeMapSuper.Add("Poison", new List<string> { "Grass" });
            TypeMapNotVery.Add("Poison", new List<string> { "Ground", "Rock","Ghost","Steel" });

            //Ground
            TypeMapSuper.Add("Ground", new List<string> { "Fire", "Electric","Poison","Rock","Steel" });
            TypeMapNotVery.Add("Ground", new List<string> { "Grass", "Flying","Bug" });

            //Flying
            TypeMapSuper.Add("Flying", new List<string> { "Grass", "Fighting","Bug" });
            TypeMapNotVery.Add("Flying", new List<string> { "Steel", "Rock","Electric" });

            //Psychic
            TypeMapSuper.Add("Psychic", new List<string> { "Poison", "Fighting" });
            TypeMapNotVery.Add("Psychic", new List<string> { "Dark", "Steel" });

            //Bug
            TypeMapSuper.Add("Bug", new List<string> { "Grass", "Psychic","Dark" });
            TypeMapNotVery.Add("Bug", new List<string> { "Ghost", "Flying","Poison","Fighting","Fire","Steel" });


            //Rock
            TypeMapSuper.Add("Rock", new List<string> { "Ice", "Fire","Flying","Bug" });
            TypeMapNotVery.Add("Rock", new List<string> { "Fighting", "Ground","Steel" });

            //Ghost
            TypeMapSuper.Add("Ghost", new List<string> { "Psychic", "Ghost" });
            TypeMapNotVery.Add("Ghost", new List<string> { "Dark", "Steel" });


            //Dragon
            TypeMapSuper.Add("Dragon", new List<string> { "Dragon" });
            TypeMapNotVery.Add("Dragon", new List<string> { "Steel" });


            //Dark
            TypeMapSuper.Add("Dark", new List<string> { "Psychic","Ghost"});
            TypeMapNotVery.Add("Dark", new List<string> { "Steel","Dark","Fighting" });

            //Steel
            TypeMapSuper.Add("Steel", new List<string> {"Ice","Rock" });
            TypeMapNotVery.Add("Steel", new List<string> { "Fire", "Water","Electric" });

        }

    }
}
