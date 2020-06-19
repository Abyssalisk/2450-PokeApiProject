using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using PokeAPI;

namespace PokemonSimulator
{
    class LoadPokemonFromDB
    {
        public int TrainerId { get; set; }
        public MySqlConnection Con { get; set; }

        public List<Pokemon> LoadedLineUp { get; set; }
        public LoadPokemonFromDB(int trainerId, MySqlConnection con)
        {
            TrainerId = trainerId;
            Con = con;
            LoadPokemonTeam();
        }

        public void LoadPokemonTeam()
        {
            var tempLineUp = new List<Pokemon>();
            var getPokemonQuery = "SELECT `Pokemon1`,`MovesCSV1`,`Pokemon2`,`MovesCSV2`,`Pokemon3`,`MovesCSV3`" +
                ",`Pokemon4`,`MovesCSV4`,`Pokemon5`,`MovesCSV5`,`Pokemon6`,`MovesCSV6`" +
                " FROM sql3346222.TrainerLineup WHERE(UserID = " + TrainerId + ");";

            //Get pokemon from DB
            Con.Open();
            MySqlCommand cmd = new MySqlCommand(getPokemonQuery, Con);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    
                    for (int i = 0; i < 12; i++)
                    {
                        Console.WriteLine("Loading........");
                        Pokemon temp = new Pokemon();

                        temp.Species = reader[i].ToString();
                        Console.Write(reader[i].ToString());

                        temp.Moves = AddMoves(reader[i+1].ToString());
                        //Task<PokeAPI.> p = DataFetcher.GetNamedApiObject<PokeAPI.Stat>(temp.Species);
                        //temp.BaseHP = p.Result.
                        tempLineUp.Add(temp);
                        i++;
                    }
                }
            }
            Con.Close();
        }

        public List<Move> AddMoves(string movescsv)
        {
            Console.WriteLine(movescsv);
            List<Move> tempMovesList = new List<Move>();

            for (int i = 0; i < 4; i++)
            {
                Move tempMove = new Move();
                if (movescsv.Contains(","))
                {
                    tempMove.Name = movescsv.Substring(movescsv.LastIndexOf(","), movescsv.Length - movescsv.LastIndexOf(","));
                    tempMove.Name = tempMove.Name.Remove(0, 1);
                    movescsv = movescsv.Remove(movescsv.LastIndexOf(","), movescsv.Length - movescsv.LastIndexOf(","));
                }
                else
                {
                    tempMove.Name = movescsv;
                }

                Console.WriteLine(tempMove.Name);
                Task<PokeAPI.Move> p = DataFetcher.GetNamedApiObject<PokeAPI.Move>(tempMove.Name.ToString());
                tempMove.Type = p.Result.Type.ToString();
                tempMove.Damage = (int)p.Result.Power;
                tempMovesList.Add(tempMove);
            }

                return tempMovesList;
        }
    }
}
