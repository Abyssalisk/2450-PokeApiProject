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

        public string[] PokemonArray;
        public string[] MovesCSVArray;

        string SearchedName;
        string ReturnedName;
        Boolean ValidPokemon;
        int LineupSize;

        public CreateLineUp(Trainer ghostTrainer, MySqlConnection con)
        {
            PokemonArray = new string[6];
            MovesCSVArray = new string[6];
            LineupSize = 0;
            ValidPokemon = false;
            GhostTrainer = ghostTrainer;
            Con = con;
            ReturnedName = "-1";
            while (LineupSize <= 6)
            {
                PokeFinder();
            }
            AddPokemonToDB();
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
                ReturnedName = "-1";
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
                Console.WriteLine("Add " + ReturnedName + " to lineup?(y/n)");
                string choice = Console.ReadLine();
                if (choice.ToLower().Equals("y"))
                {
                    Console.WriteLine(ReturnedName+" added!");
                    LineupSize++;

                    var ChooseMoves = new MoveSelector2000(ReturnedName);

                    AddPokemonToArray(ReturnedName);
                    AddMovesToArray(ChooseMoves.Move1+","+ ChooseMoves.Move2+"," + ChooseMoves.Move3 + "," + ChooseMoves.Move4);
                    Console.WriteLine("Lets add another Pokemon....");
                    return true;

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


        public void AddPokemonToDB()
        {
            var insertPokemonQuery = "INSERT INTO sql3346222.TrainerLineup VALUES('"+GhostTrainer.UserId+"','" +
                GhostTrainer.TrainerName+"','"+PokemonArray[0]+"','"+MovesCSVArray[0]+"','"+PokemonArray[1]
                +"','"+MovesCSVArray[1]+"','"+ PokemonArray[2] + "','" + MovesCSVArray[2] + "','" + PokemonArray[3] +
                "','" + MovesCSVArray[3] + "','" + PokemonArray[4]+ "','" + MovesCSVArray[4] +
                 "','" + PokemonArray[5] + "','" + MovesCSVArray[5] + "');";
            Console.WriteLine(insertPokemonQuery.ToString());

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

        private void AddMovesToArray(string moves)
        {
            if (MovesCSVArray[0] == null)
            {
                MovesCSVArray[0] = moves;
                return;
            }
            else if (MovesCSVArray[1] == null)
            {
                MovesCSVArray[1] = moves;
                return;
            }
            else if (MovesCSVArray[2] == null)
            {
                MovesCSVArray[2] = moves;
                return;
            }
            else if (MovesCSVArray[3] == null)
            {
                MovesCSVArray[3] = moves;
                return;
            }
            else if (MovesCSVArray[4] == null)
            {
                MovesCSVArray[4] = moves;
                return;
            }
            else if (MovesCSVArray[5] == null)
            {
                MovesCSVArray[5] = moves;
                return;
            }
        }

        private void AddPokemonToArray(string pokemon)
        {
            if (PokemonArray[0] == null)
            {
                MovesCSVArray[0] = pokemon;
                return;
            }
            else if (PokemonArray[1] == null)
            {
                PokemonArray[1] = pokemon;
                return; 
            }
            else if (PokemonArray[2] == null)
            {
                PokemonArray[2] = pokemon;
                return;
            }
            else if (PokemonArray[3] == null)
            {
                PokemonArray[3] = pokemon;
                return;
            }
            else if (PokemonArray[4] == null)
            {
                PokemonArray[4] = pokemon;
                return;
            }
            else if (PokemonArray[5] == null)
            {
                PokemonArray[5] = pokemon;
                return;
            }
        }
    }
}
