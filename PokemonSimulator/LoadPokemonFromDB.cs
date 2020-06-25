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
        public string TrainerName { get; set; }
        public MySqlConnection Con { get; set; }

        public List<Pokemon> LoadedLineUp { get; set; }
        public LoadPokemonFromDB(int trainerId, MySqlConnection con)
        {
            LoadedLineUp = new List<Pokemon>();
            TrainerId = trainerId;
            Con = con;
            LoadPokemonTeam();
        }

        public LoadPokemonFromDB(string trainername, MySqlConnection con)
        {
            LoadedLineUp = new List<Pokemon>();
            TrainerName = "";
            TrainerName = trainername;
            Con = con;
            LoadPokemonTeam(true);
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
                        Console.WriteLine(reader[i].ToString());

                        temp.ConsoleMoves = AddMoves(reader[i+1].ToString());
                        Task < PokeAPI.Pokemon> p = DataFetcher.GetNamedApiObject<PokeAPI.Pokemon>(temp.Species);
                       
                        PokeAPI.PokemonStats[] stats = new PokemonStats[10];
                        PokeAPI.PokemonTypeMap[] type = new PokemonTypeMap[10];

                        stats = p.Result.Stats;
                        type = p.Result.Types;
                        
                        for(int j = 0;j<type.Length;j++)
                            temp.ConsoleTypes.Add(type[j].Type.Name);

                        temp.BaseHP = stats[0].BaseValue;
                        temp.ActingHP = temp.BaseHP;
                        temp.Attack = stats[1].BaseValue;
                        temp.Defense = stats[2].BaseValue;
                        temp.SpecialAttack = stats[3].BaseValue;
                        temp.SpecialDefense = stats[4].BaseValue;
                        temp.Speed = stats[5].BaseValue;
                        
                        tempLineUp.Add(temp);
                        i++;
                    }
                }
            }
            Con.Close();
            LoadedLineUp = tempLineUp;
        }

        public void LoadPokemonTeam(Boolean x)
        {
            var tempLineUp = new List<Pokemon>();
            var getPokemonQuery = "SELECT `Pokemon1`,`MovesCSV1`,`Pokemon2`,`MovesCSV2`,`Pokemon3`,`MovesCSV3`" +
                ",`Pokemon4`,`MovesCSV4`,`Pokemon5`,`MovesCSV5`,`Pokemon6`,`MovesCSV6`" +
                " FROM sql3346222.EliteFour WHERE(TrainerName = '" + TrainerName + "');";

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
                        Console.WriteLine(reader[i].ToString());

                        temp.ConsoleMoves = AddMoves(reader[i + 1].ToString());
                        Task<PokeAPI.Pokemon> p = DataFetcher.GetNamedApiObject<PokeAPI.Pokemon>(temp.Species);

                        PokeAPI.PokemonStats[] stats = new PokemonStats[10];
                        PokeAPI.PokemonTypeMap[] type = new PokemonTypeMap[10];

                        stats = p.Result.Stats;
                        type = p.Result.Types;

                        for (int j = 0; j < type.Length; j++)
                            temp.ConsoleTypes.Add(type[j].Type.Name);

                        temp.BaseHP = stats[0].BaseValue;
                        temp.ActingHP = temp.BaseHP;
                        temp.Attack = stats[1].BaseValue;
                        temp.Defense = stats[2].BaseValue;
                        temp.SpecialAttack = stats[3].BaseValue;
                        temp.SpecialDefense = stats[4].BaseValue;
                        temp.Speed = stats[5].BaseValue;

                        tempLineUp.Add(temp);
                        i++;
                    }
                }
            }
            Con.Close();
            LoadedLineUp = tempLineUp;
        }

        public List<Move> AddMoves(string movescsv)
        {
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

                Task<PokeAPI.Move> p = DataFetcher.GetNamedApiObject<PokeAPI.Move>(tempMove.Name.ToString());
                tempMove.Type = p.Result.Type.ToString();
                tempMove.Damage = (int)p.Result.Power;
                
                string damagetype = p.Result.DamageClass.Name;
                
                if (damagetype == "physical")
                    tempMove.IsPhysical = true;
                else
                    tempMove.IsPhysical = false;

                tempMovesList.Add(tempMove);
            }

            return tempMovesList;
        }
    }
}
