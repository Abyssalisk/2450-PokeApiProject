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

        public CreateLineUp()
        {
        }

        public bool SearchPokemonAsync(string searchName)
        {
            try
            {
                Task<PokemonSpecies> p = DataFetcher.GetNamedApiObject<PokemonSpecies>(searchName.ToLower());
                searchName = p.Result.Name.ToString();
                return true;
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        public bool PokeFinder(string returnedName)
        {
            while (true)
            {

                if (SearchPokemonAsync(returnedName) == false)
                {
                    return false;
                }
                else
                {
                    Console.WriteLine(returnedName + " has been located!");
                    return true;
                }
            }
        }

        public string AddToLineup(string name)
        {
            var ChooseMoves = new MoveSelector2000(name);
            ChooseMoves.DisplayMoves();
            string move1 = ChooseMoves.ChoseMove() + ",";
            string move2 = ChooseMoves.ChoseMove() + ",";
            string move3 = ChooseMoves.ChoseMove() + ",";
            string move4 = ChooseMoves.ChoseMove();

            string moves = move1 + move2 + move3 + move4;
            return moves;
        }



        public void AddPokemonToDB(string[] PokemonArray, string[] MovesCSVArray, string name, int UserId, MySqlConnection Con, bool HasLineup)
        {
            var pokemonQuery = "";
            if (HasLineup == false)
            {
                pokemonQuery = "INSERT INTO sql3346222.TrainerLineup VALUES(" + UserId + ",'" +
                name + "','" + PokemonArray[0] + "','" + MovesCSVArray[0] + "','" + PokemonArray[1]
                + "','" + MovesCSVArray[1] + "','" + PokemonArray[2] + "','" + MovesCSVArray[2] + "','" + PokemonArray[3] +
                "','" + MovesCSVArray[3] + "','" + PokemonArray[4] + "','" + MovesCSVArray[4] +
                 "','" + PokemonArray[5] + "','" + MovesCSVArray[5] + "');";
            }
            else
            {
                pokemonQuery = "UPDATE sql3346222.TrainerLineup SET Pokemon1='" + PokemonArray[0] +
                    "',MovesCSV1='" + MovesCSVArray[0] + "',Pokemon2='" + PokemonArray[1] +
                    "',MovesCSV2='" + MovesCSVArray[1] + "',Pokemon3='" + PokemonArray[2] +
                    "',MovesCSV3='" + MovesCSVArray[2] + "',Pokemon4='" + PokemonArray[3] +
                    "',MovesCSV4='" + MovesCSVArray[3] + "',Pokemon5='" + PokemonArray[4] +
                    "',MovesCSV5='" + MovesCSVArray[4] + "',Pokemon6='" + PokemonArray[5] +
                    "',MovesCSV6='" + MovesCSVArray[5] + "' WHERE( UserID = " + UserId + ");";
            }
            Con.Open();
            MySqlCommand cmd = new MySqlCommand(pokemonQuery, Con);
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
    