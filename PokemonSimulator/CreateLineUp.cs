using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using PokeAPI;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PokemonSimulator
{
    public class CreateLineUp
    {
        public Trainer GhostTrainer { get; set; }
        MySqlConnection Con { get; set; }

        string SearchedName;
        string ReturnedName;
        Boolean ValidPokemon;
        int LineupSize;

        public CreateLineUp(Trainer ghostTrainer, MySqlConnection con)
        {
            LineupSize = 0;
            ValidPokemon = false;
            GhostTrainer = ghostTrainer;
            Con = con;
            ReturnedName = "-1";
            while (LineupSize <= 6)
            {
                PokeFinder();
            }
        }

        public void ReadName()
        {
            Console.WriteLine("Enter the name of desired pokemon: ");
            SearchedName = Console.ReadLine();
        }

        public void SearchPokemonAsync()
        {
            Task<PokemonSpecies> p = DataFetcher.GetNamedApiObject<PokemonSpecies>(SearchedName);
            ReturnedName = p.Result.Name.ToString();
           
        }

        public void PokeFinder()
        {
            while(ValidPokemon==false)
            {
                ReadName();
                SearchPokemonAsync();
                if(ReturnedName=="-1")
                {
                    Console.WriteLine("Pokemon not found, try again!");
                }
                else
                {
                    Console.WriteLine(ReturnedName + " has been located!");
                     
                    if(AddToLineup()==true)
                    {
                        ValidPokemon = true;
                    }
                }
            }
        }

        public Boolean AddToLineup()
        {
            while (true)
            {
                Console.WriteLine("Add " + ReturnedName + " to lineup?");
                string choice = Console.ReadLine();
                if (choice.ToLower().Equals("y"))
                {
                    Console.WriteLine(ReturnedName+" added!");
                    LineupSize++;

                    var ChooseMoves = new MoveSelector2000();

                    AddPokemonToDB(ChooseMoves.Move1+","+ ChooseMoves.Move2+",");
                    Console.WriteLine("Lets add another Pokemon....");
                }
                else if (choice.ToLower().Equals("n"))
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Invalid choice!");
                }
            }
        }

        public void AddPokemonToDB(string moves)
        {
            var insertPokemonQuery = "INSERT INTO sql3346222.TrainerLineup WHERE();";

            Con.Open();
            MySqlCommand cmd = new MySqlCommand(insertPokemonQuery, Con);
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    Console.WriteLine(rdr[0] + " -- " + rdr[1]);
                }
            }
            Con.Close();

        }
    }
}
