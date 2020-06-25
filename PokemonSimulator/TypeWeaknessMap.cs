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
            TypeMapNotVery.Add("normal", new List<string> { "ghost", "rock", "steel" });

            //Fire
            TypeMapSuper.Add("fire", new List<string> { "grass", "ice", "bug", "steel" });
            TypeMapNotVery.Add("fire", new List<string> { "water", "rock", "dragon" });


            //Water
            TypeMapSuper.Add("water", new List<string> { "fire", "ground", "rock" });
            TypeMapNotVery.Add("water", new List<string> { "grass", "dargon" });

            //Electric
            TypeMapSuper.Add("electric", new List<string> { "water", "flying" });
            TypeMapNotVery.Add("electric", new List<string> { "ground" });


            //Grass
            TypeMapSuper.Add("grass", new List<string> { "water", "ground", "rock" });
            TypeMapNotVery.Add("grass", new List<string> { "fire", "poison", "bug", "dragon", "steel", "flying" });

            //Ice
            TypeMapSuper.Add("ice", new List<string> { "grass", "ground", "flying", "dragon" });
            TypeMapNotVery.Add("ice", new List<string> { "fire", "water", "steel" });


            //Fighting
            TypeMapSuper.Add("fighting", new List<string> { "ice", "rock", "steel", "dark", "normal" });
            TypeMapNotVery.Add("fighting", new List<string> { "ghost", "poison", "flying", "bug", "psychic" });


            //Poison
            TypeMapSuper.Add("poison", new List<string> { "grass" });
            TypeMapNotVery.Add("poison", new List<string> { "ground", "rock", "ghost", "steel" });

            //Ground
            TypeMapSuper.Add("ground", new List<string> { "fire", "electric", "poison", "rock", "steel" });
            TypeMapNotVery.Add("ground", new List<string> { "grass", "flying", "bug" });

            //Flying
            TypeMapSuper.Add("flying", new List<string> { "grass", "fighting", "bug" });
            TypeMapNotVery.Add("flying", new List<string> { "steel", "rock", "electric" });

            //Psychic
            TypeMapSuper.Add("psychic", new List<string> { "poison", "fighting" });
            TypeMapNotVery.Add("psychic", new List<string> { "dark", "steel" });

            //Bug
            TypeMapSuper.Add("bug", new List<string> { "grass", "psychic", "dark" });
            TypeMapNotVery.Add("bug", new List<string> { "ghost", "flying", "poison", "fighting", "fire", "steel" });


            //Rock
            TypeMapSuper.Add("rock", new List<string> { "ice", "fire", "flying", "bug" });
            TypeMapNotVery.Add("rock", new List<string> { "fighting", "ground", "steel" });

            //Ghost
            TypeMapSuper.Add("ghost", new List<string> { "psychic", "ghost" });
            TypeMapNotVery.Add("ghost", new List<string> { "dark", "steel" });


            //Dragon
            TypeMapSuper.Add("dragon", new List<string> { "dragon" });
            TypeMapNotVery.Add("dragon", new List<string> { "steel" });


            //Dark
            TypeMapSuper.Add("dark", new List<string> { "psychic", "ghost" });
            TypeMapNotVery.Add("dark", new List<string> { "steel", "dark", "fighting" });

            //Steel
            TypeMapSuper.Add("steel", new List<string> { "ice", "rock" });
            TypeMapNotVery.Add("steel", new List<string> { "fire", "water", "electric" });

        }

    }
}
