using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1;

namespace PokemonSimulator
{
    public class CreateLineUpIO
    {
        public Trainer GhostTrainer { get; set; }
        MySqlConnection Con { get; set; }
        public CreateLineUp MakeLineUp;

        public string[] PokemonArray;
        public string[] MovesCSVArray;

        Boolean HasLineup;
        int LineupSize;
        public CreateLineUpIO(Trainer ghostTrainer, MySqlConnection con, Boolean haslineup)
        {
            MakeLineUp = new CreateLineUp();
            PokemonArray = new string[6];
            MovesCSVArray = new string[6];
            LineupSize = 0;
            HasLineup = haslineup;
            GhostTrainer = ghostTrainer;
            Con = con;
            AddPokemonToLineup();
        }

        public void AddPokemonToLineup()
        {
            while (LineupSize < 6)
            {
                Console.WriteLine("Enter the name of desired pokemon: ");
                string pokemonName = Console.ReadLine();
                bool pokemonfound = MakeLineUp.SearchPokemonAsync(pokemonName);
                if (pokemonfound == true)
                {
                    AddDescion(pokemonName.ToLower());
                }
                else if (pokemonfound == false)
                {
                    Console.WriteLine("Pokemon not found, try again!");
                }
            }
            MakeLineUp.AddPokemonToDB(PokemonArray, MovesCSVArray, GhostTrainer, Con, HasLineup);
        }

        public void AddDescion(string name)
        {
            while (true)
            {
                Console.WriteLine("Add " + name + " to lineup?(y/n)");
                string choice = Console.ReadLine();

                if (choice.ToLower() == "n")
                {
                    break;
                }
                else if (choice.ToLower() == "y")
                {
                    PokemonArray[LineupSize] = name;
                    MovesCSVArray[LineupSize] = MakeLineUp.AddToLineup(name);
                    LineupSize++;
                    break;
                }
                else
                {
                    Console.WriteLine("invalid selection");
                }
            }
        }
    }
}
