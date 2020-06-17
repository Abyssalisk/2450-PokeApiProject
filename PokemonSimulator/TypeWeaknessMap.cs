using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonSimulator
{
    class TypeWeaknessMap
    {
        public Dictionary<string, List<string>> TypeMapSuper = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> TypeMapNotVery = new Dictionary<string, List<string>>();

        List<string> SuperEffective;
        List<string> NotVeryEffective;
        public TypeWeaknessMap()
        {
            //Only using one type for this not dual types. Dual types that the given move is super effective against
            //have one of the two types rooted in single type effectiveness. This single type effectiveness can be
            //used as a lookup for dual type effectiveness. Exceptions to this methodology, yes but very limited cases
           
            //Normal
            NotVeryEffective.Add("Ghost");
            NotVeryEffective.Add("Rock");
            NotVeryEffective.Add("Steel");

            TypeMapNotVery.Add("Normal", NotVeryEffective);

            NotVeryEffective.Clear();

            //Fire
            SuperEffective.Add("Grass");
            SuperEffective.Add("Ice");
            SuperEffective.Add("Bug");
            SuperEffective.Add("Steel");

            NotVeryEffective.Add("Water");
            NotVeryEffective.Add("Rock");
            NotVeryEffective.Add("Dragon");

            TypeMapSuper.Add("Fire", SuperEffective);
            TypeMapNotVery.Add("Fire", NotVeryEffective);

            SuperEffective.Clear();
            NotVeryEffective.Clear();

            //Water
            SuperEffective.Add("Fire");
            SuperEffective.Add("Ground");
            SuperEffective.Add("Rock");

            NotVeryEffective.Add("Grass");
            NotVeryEffective.Add("Dragon");

            TypeMapSuper.Add("Water", SuperEffective);
            TypeMapNotVery.Add("Water", NotVeryEffective);

            SuperEffective.Clear();
            NotVeryEffective.Clear();

            //Electric
            SuperEffective.Add("Water");
            SuperEffective.Add("Flying");

            NotVeryEffective.Add("Ground");

            TypeMapSuper.Add("Electric", SuperEffective);
            TypeMapNotVery.Add("Electric", NotVeryEffective);

            SuperEffective.Clear();
            NotVeryEffective.Clear();

            //Grass
            SuperEffective.Add("Water");
            SuperEffective.Add("Ground");
            SuperEffective.Add("Rock");

            NotVeryEffective.Add("Fire");
            NotVeryEffective.Add("Poison");
            NotVeryEffective.Add("Bug");
            NotVeryEffective.Add("Dragon");
            NotVeryEffective.Add("Steel");
            NotVeryEffective.Add("Flying");

            TypeMapSuper.Add("Grass", SuperEffective);
            TypeMapNotVery.Add("Grass", NotVeryEffective);

            SuperEffective.Clear();
            NotVeryEffective.Clear();

            //Ice
            SuperEffective.Add("Grass");
            SuperEffective.Add("Ground");
            SuperEffective.Add("Flying");
            SuperEffective.Add("Dragon");

            NotVeryEffective.Add("Fire");
            NotVeryEffective.Add("Water");
            NotVeryEffective.Add("Steel");

            TypeMapSuper.Add("Ice", SuperEffective);
            TypeMapNotVery.Add("Ice", NotVeryEffective);

            SuperEffective.Clear();
            NotVeryEffective.Clear();

            //Fight
            SuperEffective.Add("");
            SuperEffective.Add("");
            SuperEffective.Add("");

            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");

            TypeMapSuper.Add("Fight", SuperEffective);
            TypeMapNotVery.Add("Fight", NotVeryEffective);

            SuperEffective.Clear();
            NotVeryEffective.Clear();

            //Poison
            SuperEffective.Add("");
            SuperEffective.Add("");
            SuperEffective.Add("");

            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");

            TypeMapSuper.Add("Poison", SuperEffective);
            TypeMapNotVery.Add("Poison", NotVeryEffective);

            SuperEffective.Clear();
            NotVeryEffective.Clear();

            //Ground
            SuperEffective.Add("");
            SuperEffective.Add("");
            SuperEffective.Add("");

            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");

            TypeMapSuper.Add("Ground", SuperEffective);
            TypeMapNotVery.Add("Ground", NotVeryEffective);

            SuperEffective.Clear();
            NotVeryEffective.Clear();

            //Flying
            SuperEffective.Add("");
            SuperEffective.Add("");
            SuperEffective.Add("");

            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");

            TypeMapSuper.Add("Flying", SuperEffective);
            TypeMapNotVery.Add("Flying", NotVeryEffective);

            SuperEffective.Clear();
            NotVeryEffective.Clear();

            //Psychic
            SuperEffective.Add("");
            SuperEffective.Add("");
            SuperEffective.Add("");

            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");

            TypeMapSuper.Add("Psychic", SuperEffective);
            TypeMapNotVery.Add("Psychic", NotVeryEffective);

            SuperEffective.Clear();
            NotVeryEffective.Clear();

            //Bug
            SuperEffective.Add("");
            SuperEffective.Add("");
            SuperEffective.Add("");

            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");

            TypeMapSuper.Add("Bug", SuperEffective);
            TypeMapNotVery.Add("Bug", NotVeryEffective);

            SuperEffective.Clear();
            NotVeryEffective.Clear();

            //Rock
            SuperEffective.Add("");
            SuperEffective.Add("");
            SuperEffective.Add("");

            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");

            TypeMapSuper.Add("Rock", SuperEffective);
            TypeMapNotVery.Add("Rock", NotVeryEffective);

            SuperEffective.Clear();
            NotVeryEffective.Clear();

            //Ghost
            SuperEffective.Add("");
            SuperEffective.Add("");
            SuperEffective.Add("");

            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");

            TypeMapSuper.Add("Ghost", SuperEffective);
            TypeMapNotVery.Add("Ghost", NotVeryEffective);

            SuperEffective.Clear();
            NotVeryEffective.Clear();

            //Dragon
            SuperEffective.Add("");
            SuperEffective.Add("");
            SuperEffective.Add("");

            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");

            TypeMapSuper.Add("Dragon", SuperEffective);
            TypeMapNotVery.Add("Dragon", NotVeryEffective);

            SuperEffective.Clear();
            NotVeryEffective.Clear();

            //Dark
            SuperEffective.Add("");
            SuperEffective.Add("");
            SuperEffective.Add("");

            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");

            TypeMapSuper.Add("Dark", SuperEffective);
            TypeMapNotVery.Add("Dark", NotVeryEffective);

            SuperEffective.Clear();
            NotVeryEffective.Clear();

            //Steel
            SuperEffective.Add("");
            SuperEffective.Add("");
            SuperEffective.Add("");

            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");
            NotVeryEffective.Add("");

            TypeMapSuper.Add("Steel", SuperEffective);
            TypeMapNotVery.Add("Steel", NotVeryEffective);

            SuperEffective.Clear();
            NotVeryEffective.Clear();
        }


    }
}
