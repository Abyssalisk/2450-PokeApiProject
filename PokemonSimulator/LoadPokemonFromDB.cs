using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using PokeAPI;

namespace PokemonSimulator
{
    /// <summary>
    /// Author: Not Samuel Gardner; Derek I think.
    /// </summary>
    class LoadPokemonFromDB
    {
        public int TrainerId { get; set; }
        public string TrainerName { get; set; }
        public MySqlConnection Con { get; set; }

        public List<Pokemon> LoadedLineUp { get; set; }
        public LoadPokemonFromDB(int trainerId, MySqlConnection con)
        {
            LoadedLineUp = new List<Pokemon>();
            TrainerName = string.Empty;
            TrainerId = trainerId;
            Con = con;
            LoadData();
        }

        public LoadPokemonFromDB(string trainername, MySqlConnection con)
        {
            LoadedLineUp = new List<Pokemon>();
            TrainerName = trainername;
            Con = con;
            LoadData(/*true*/);
        }

        /// <summary>
        /// Author: Samuel Gardner
        /// </summary>
        public void LoadData()
        {
            var tempLineUp = new List<Pokemon>();
            var getPokemonQuery = (TrainerName == string.Empty ? "SELECT `Pokemon1`,`MovesCSV1`,`Pokemon2`,`MovesCSV2`,`Pokemon3`,`MovesCSV3`" +
                ",`Pokemon4`,`MovesCSV4`,`Pokemon5`,`MovesCSV5`,`Pokemon6`,`MovesCSV6`" +
                " FROM sql3346222.TrainerLineup WHERE(UserID = " + TrainerId + ");" :
                "SELECT `Pokemon1`,`MovesCSV1`,`Pokemon2`,`MovesCSV2`,`Pokemon3`,`MovesCSV3`" +
                ",`Pokemon4`,`MovesCSV4`,`Pokemon5`,`MovesCSV5`,`Pokemon6`,`MovesCSV6`" +
                " FROM sql3346222.EliteFour WHERE(TrainerName = '" + TrainerName + "');");

            //Get pokemon from DB
            Con.Open();
            MySqlCommand cmd = new MySqlCommand(getPokemonQuery, Con);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                Queue<Tuple<Task<PokeAPI.Pokemon>, List<Task<PokeAPI.Move>>>> tasks = new Queue<Tuple<Task<PokeAPI.Pokemon>, List<Task<PokeAPI.Move>>>>();
                Task<PokeAPI.Pokemon> tempGetPoke = null;
                List<Task<PokeAPI.Move>> tempGetMove = null;
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (i % 2 == 0)
                        {
                            //this record is a pokemon.
                            Console.WriteLine($"Starting request for pokemon: {reader.GetString(i)}...");
                            tempGetPoke = DataFetcher.GetNamedApiObject<PokeAPI.Pokemon>(reader.GetString(i));
                            //tempGetPoke.Start();
                        }
                        else
                        {
                            //this record is a moves.
                            tempGetMove = reader.GetString(i).Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x =>
                            {
                                string cleaned = x.Trim().ToLower();
                                Console.WriteLine($"Starting request for move: {cleaned}...");
                                return DataFetcher.GetNamedApiObject<PokeAPI.Move>(cleaned);
                            }).ToList();
                            //tempGetMove = GetMovesAsync(reader.GetString(i).Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray());
                            //tempGetMove.ForEach(x =>
                            //{
                            //    x.Start();
                            //});
                        }
                        if (tempGetMove != null && tempGetPoke != null)
                        {
                            tasks.Enqueue(new Tuple<Task<PokeAPI.Pokemon>, List<Task<PokeAPI.Move>>>(tempGetPoke, tempGetMove));
                            tempGetMove = null;
                            tempGetPoke = null;
                        }
                    }
                }
                //Task.WaitAll(tasks.Select(x => new List<Task>(x.Item2).Add(x.Item1)).Aggregate((x, y) => x.Concat(y).ToArray()));
                foreach (Tuple<Task<PokeAPI.Pokemon>, List<Task<PokeAPI.Move>>> e in tasks)
                {
                    Task.WaitAll((e.Item2.Select(x => (Task)x).Append((Task)e.Item1)).ToArray());
                    int hp = e.Item1.Result.Stats.FirstOrDefault(x => x.Stat.Name == "hp").BaseValue;
                    tempLineUp.Add(new Pokemon()
                    {
                        BaseHP = hp,
                        ActingHP = hp,
                        Attack = e.Item1.Result.Stats.FirstOrDefault(x => x.Stat.Name == "attack").BaseValue,
                        Defense = e.Item1.Result.Stats.FirstOrDefault(x => x.Stat.Name == "defense").BaseValue,
                        SpecialAttack = e.Item1.Result.Stats.FirstOrDefault(x => x.Stat.Name == "special-attack").BaseValue,
                        SpecialDefense = e.Item1.Result.Stats.FirstOrDefault(x => x.Stat.Name == "special-defense").BaseValue,
                        Speed = e.Item1.Result.Stats.FirstOrDefault(x => x.Stat.Name == "speed").BaseValue,
                        BackImageUri = e.Item1.Result.Sprites.BackFemale ?? e.Item1.Result.Sprites.BackMale,
                        FrontImageUri = e.Item1.Result.Sprites.FrontFemale ?? e.Item1.Result.Sprites.FrontMale,
                        Id = e.Item1.Result.ID,
                        Species = e.Item1.Result.Species.Name,
                        ConsoleMoves = e.Item2.Select(x => new Move()
                        {
                            Damage = x.Result.Power ?? 0,
                            IsPhysical = x.Result.DamageClass.Name == "physical",
                            Name = x.Result.Name,
                            Type = x.Result.Type.Name
                        }).ToList(),
                        ConsoleTypes = e.Item1.Result.Types.Select(x => x.Type.Name).ToList(),
                        Moves = e.Item2.Select(x => new Web.Shared.Models.MoveModel()
                        {
                            Category = x.Result.DamageClass.Name,
                            Damage = x.Result.Power ?? 0,
                            Id = x.Result.ID,
                            Name = x.Result.Name,
                            ResourceUri = null, //Not sure what to do about this field.
                            Seleted = default, //Not sure what to do about this either.
                            Type = x.Result.Type.Name
                        }).ToList(),
                        Types = e.Item1.Result.Types.Select(x => new PokemonType()
                        {
                            //GameIndices = default,
                            //Generation = default,
                            //MoveDamageClass = default,
                            //Moves = default,               //I'm guessing this stuff needs to be gotten through GetNamedApiObject
                            //Name = default,
                            //Names = default,
                            //Pokemon = default
                        }).ToList(),
                        TypeWeaknesses = null
                    });
                }

                #region Old
                //while (reader.Read())
                //{

                //    for (int i = 0; i < 12; i++)
                //    {
                //        Console.WriteLine("Loading........");
                //        Pokemon temp = new Pokemon();

                //        temp.Species = reader[i].ToString();
                //        Console.WriteLine(reader[i].ToString());

                //        temp.ConsoleMoves = AddMoves(reader[i+1].ToString());
                //        Task < PokeAPI.Pokemon> p = DataFetcher.GetNamedApiObject<PokeAPI.Pokemon>(temp.Species);

                //        PokeAPI.PokemonStats[] stats = new PokemonStats[10];
                //        PokeAPI.PokemonTypeMap[] type = new PokemonTypeMap[10];

                //        stats = p.Result.Stats;
                //        type = p.Result.Types;

                //        for(int j = 0;j<type.Length;j++)
                //            temp.ConsoleTypes.Add(type[j].Type.Name);

                //        temp.BaseHP = stats[0].BaseValue;
                //        temp.ActingHP = temp.BaseHP;
                //        temp.Attack = stats[1].BaseValue;
                //        temp.Defense = stats[2].BaseValue;
                //        temp.SpecialAttack = stats[3].BaseValue;
                //        temp.SpecialDefense = stats[4].BaseValue;
                //        temp.Speed = stats[5].BaseValue;

                //        tempLineUp.Add(temp);
                //        i++;
                //    }
                //}
                #endregion

            }
            Con.Close();
            LoadedLineUp = tempLineUp;
        }

        #region Duplicate of LoadPokemonTeam (Above method covers the differences this one had).
        //public void LoadPokemonTeam(Boolean x)
        //{
        //    var tempLineUp = new List<Pokemon>();
        //    var getPokemonQuery = "SELECT `Pokemon1`,`MovesCSV1`,`Pokemon2`,`MovesCSV2`,`Pokemon3`,`MovesCSV3`" +
        //        ",`Pokemon4`,`MovesCSV4`,`Pokemon5`,`MovesCSV5`,`Pokemon6`,`MovesCSV6`" +
        //        " FROM sql3346222.EliteFour WHERE(TrainerName = '" + TrainerName + "');";

        //    //Get pokemon from DB
        //    Con.Open();
        //    MySqlCommand cmd = new MySqlCommand(getPokemonQuery, Con);
        //    using (MySqlDataReader reader = cmd.ExecuteReader())
        //    {
        //        while (reader.Read())
        //        {

        //            for (int i = 0; i < 12; i++)
        //            {
        //                Console.WriteLine("Loading........");
        //                Pokemon temp = new Pokemon();

        //                temp.Species = reader[i].ToString();
        //                Console.WriteLine(reader[i].ToString());

        //                temp.ConsoleMoves = AddMoves(reader[i + 1].ToString());
        //                Task<PokeAPI.Pokemon> p = DataFetcher.GetNamedApiObject<PokeAPI.Pokemon>(temp.Species);

        //                PokeAPI.PokemonStats[] stats = new PokemonStats[10];
        //                PokeAPI.PokemonTypeMap[] type = new PokemonTypeMap[10];

        //                stats = p.Result.Stats;
        //                type = p.Result.Types;

        //                for (int j = 0; j < type.Length; j++)
        //                    temp.ConsoleTypes.Add(type[j].Type.Name);

        //                temp.BaseHP = stats[0].BaseValue;
        //                temp.ActingHP = temp.BaseHP;
        //                temp.Attack = stats[1].BaseValue;
        //                temp.Defense = stats[2].BaseValue;
        //                temp.SpecialAttack = stats[3].BaseValue;
        //                temp.SpecialDefense = stats[4].BaseValue;
        //                temp.Speed = stats[5].BaseValue;

        //                tempLineUp.Add(temp);
        //                i++;
        //            }
        //        }
        //    }
        //    Con.Close();
        //    LoadedLineUp = tempLineUp;
        //}
        #endregion

        #region Covered by the LoadData
        //public List<Task<PokeAPI.Move>> GetMovesAsync(params string[] moves)
        //{
        //    List<Move> tempMovesList = new List<Move>();

        //    for (int i = 0; i < 4; i++)
        //    {
        //        Move tempMove = new Move();
        //        if (moves.Contains(","))
        //        {
        //            tempMove.Name = moves.Substring(moves.LastIndexOf(","), moves.Length - moves.LastIndexOf(","));
        //            tempMove.Name = tempMove.Name.Remove(0, 1);
        //            moves = moves.Remove(moves.LastIndexOf(","), moves.Length - moves.LastIndexOf(","));
        //        }
        //        else
        //        {
        //            tempMove.Name = moves;
        //        }

        //        Task<PokeAPI.Move> p = DataFetcher.GetNamedApiObject<PokeAPI.Move>(tempMove.Name.ToString());
        //        tempMove.Type = p.Result.Type.Name.ToString();
        //        tempMove.Damage = (int)p.Result.Power;

        //        string damagetype = p.Result.DamageClass.Name;

        //        if (damagetype == "physical")
        //            tempMove.IsPhysical = true;
        //        else
        //            tempMove.IsPhysical = false;

        //        tempMovesList.Add(tempMove);
        //    }

        //    return tempMovesList;
        //}
        #endregion
    }
}
