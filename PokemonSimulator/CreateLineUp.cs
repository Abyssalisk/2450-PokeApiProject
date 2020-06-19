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
            while (LineupSize < 6)
            {
                PokeFinder();
                ValidPokemon = false;
            }
            AddPokemonToDB();
            var Lineup = new TrainerLineUp(GhostTrainer.UserId, GhostTrainer.TrainerName, Con);
        }

        public void ReadName()
        {
            Console.WriteLine("Enter the name of desired pokemon: ");
            SearchedName = Console.ReadLine();
        }

        public void SearchPokemonAsync()
        {
            try
            {
                Task<PokemonSpecies> p = DataFetcher.GetNamedApiObject<PokemonSpecies>(SearchedName.ToLower());
                ReturnedName = p.Result.Name.ToString();
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                ReturnedName = "-1";
            }
           
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

                    var ChooseMoves = new MoveSelector2000(ReturnedName);

                    PokemonArray[LineupSize] = ReturnedName;
                    MovesCSVArray[LineupSize] = ChooseMoves.Move1+","+ ChooseMoves.Move2+"," + ChooseMoves.Move3 + "," + ChooseMoves.Move4;
                    
                    Console.WriteLine("Lets add another Pokemon....");
                    LineupSize++;
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
            var insertPokemonQuery = "INSERT INTO sql3346222.TrainerLineup VALUES("+GhostTrainer.UserId+",'" +
                GhostTrainer.TrainerName+"','"+PokemonArray[0]+"','"+MovesCSVArray[0]+"','"+PokemonArray[1]
                +"','"+MovesCSVArray[1]+"','"+ PokemonArray[2] + "','" + MovesCSVArray[2] + "','" + PokemonArray[3] +
                "','" + MovesCSVArray[3] + "','" + PokemonArray[4]+ "','" + MovesCSVArray[4] +
                 "','" + PokemonArray[5] + "','" + MovesCSVArray[5] + "');";

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
