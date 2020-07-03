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

        public string SearchedName = null;
        public string ReturnedName = null;
        public Boolean ValidPokemon;
        Boolean HasLineup;
        int LineupSize;

        public CreateLineUp(Trainer ghostTrainer, MySqlConnection con, Boolean haslineup)
        {
            PokemonArray = new string[6];
            MovesCSVArray = new string[6];
            LineupSize = 0;
            ValidPokemon = false;
            HasLineup = haslineup;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ReturnedName = "-1";
            }

        }

        public void PokeFinder()
        {
            while (ValidPokemon == false)
            {
                ReturnedName = "-1";
                ReadName();
                SearchPokemonAsync();
                if (ReturnedName == "-1")
                {
                    Console.WriteLine("Pokemon not found, try again!");
                }
                else
                {
                    Console.WriteLine(ReturnedName + " has been located!");

                    if (AddToLineup() == true)
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
                    Console.WriteLine(ReturnedName + " added!");

                    var ChooseMoves = new MoveSelector2000(ReturnedName);

                    PokemonArray[LineupSize] = ReturnedName;
                    MovesCSVArray[LineupSize] = ChooseMoves.Move1 + "," + ChooseMoves.Move2 + "," + ChooseMoves.Move3 + "," + ChooseMoves.Move4;

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
            string pokemonQuery = "";
            if (!HasLineup)
            {
                pokemonQuery = "INSERT INTO sql3346222.TrainerLineup VALUES(@ID,@Username,@Poke1,@CSV1,@Poke2,@CSV2,@Poke3,@CSV3,@Poke4,@CSV4,@Poke5,@CSV5,@Poke6,@CSV6);";
            }
            else
            {
                pokemonQuery = "UPDATE sql3346222.TrainerLineup SET Pokemon1=@Poke1,MovesCSV1=@CSV1,Pokemon2=@Poke2,MovesCSV2=@CSV2,Pokemon3=@Poke3,MovesCSV3=@CSV3,Pokemon4=@Poke4,MovesCSV4=@CSV4,Pokemon5=@Poke5,MovesCSV5=@CSV5,Pokemon6=@Poke6,MovesCSV6=@CSV6 WHERE(UserID = @ID);";
            }
            Con.Open();
            MySqlCommand cmd = new MySqlCommand(pokemonQuery, Con);
            if (!HasLineup)
            {
                cmd.Parameters.Add(@"@ID", MySqlDbType.Int32);
                cmd.Parameters[@"@ID"].Value = GhostTrainer.UserId;
                cmd.Parameters.Add(@"@Username", MySqlDbType.VarChar);
                cmd.Parameters[@"@Username"].Value = GhostTrainer.TrainerName;

                cmd.Parameters.Add(@"@Poke1", MySqlDbType.MediumText);
                cmd.Parameters[@"@Poke1"].Value = PokemonArray[0];
                cmd.Parameters.Add(@"@CSV1", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV1"].Value = MovesCSVArray[0];

                cmd.Parameters.Add(@"@Poke2", MySqlDbType.MediumText);
                cmd.Parameters[@"@Poke2"].Value = PokemonArray[0];
                cmd.Parameters.Add(@"@CSV2", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV2"].Value = MovesCSVArray[0];

                cmd.Parameters.Add(@"@Poke3", MySqlDbType.MediumText);
                cmd.Parameters[@"@Poke3"].Value = PokemonArray[0];
                cmd.Parameters.Add(@"@CSV3", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV3"].Value = MovesCSVArray[0];

                cmd.Parameters.Add(@"@Poke4", MySqlDbType.MediumText);
                cmd.Parameters[@"@Poke4"].Value = PokemonArray[0];
                cmd.Parameters.Add(@"@CSV4", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV4"].Value = MovesCSVArray[0];

                cmd.Parameters.Add(@"@Poke5", MySqlDbType.MediumText);
                cmd.Parameters[@"@Poke5"].Value = PokemonArray[0];
                cmd.Parameters.Add(@"@CSV5", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV5"].Value = MovesCSVArray[0];

                cmd.Parameters.Add(@"@Poke6", MySqlDbType.MediumText);
                cmd.Parameters[@"@Poke6"].Value = PokemonArray[0];
                cmd.Parameters.Add(@"@CSV6", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV6"].Value = MovesCSVArray[0];
            }
            else
            {
                cmd.Parameters.Add(@"@ID", MySqlDbType.Int32);
                cmd.Parameters[@"@ID"].Value = GhostTrainer.UserId;

                cmd.Parameters.Add(@"@Poke1", MySqlDbType.MediumText);
                cmd.Parameters[@"@Poke1"].Value = PokemonArray[0];
                cmd.Parameters.Add(@"@CSV1", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV1"].Value = MovesCSVArray[0];

                cmd.Parameters.Add(@"@Poke2", MySqlDbType.MediumText);
                cmd.Parameters[@"@Poke2"].Value = PokemonArray[0];
                cmd.Parameters.Add(@"@CSV2", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV2"].Value = MovesCSVArray[0];

                cmd.Parameters.Add(@"@Poke3", MySqlDbType.MediumText);
                cmd.Parameters[@"@Poke3"].Value = PokemonArray[0];
                cmd.Parameters.Add(@"@CSV3", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV3"].Value = MovesCSVArray[0];

                cmd.Parameters.Add(@"@Poke4", MySqlDbType.MediumText);
                cmd.Parameters[@"@Poke4"].Value = PokemonArray[0];
                cmd.Parameters.Add(@"@CSV4", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV4"].Value = MovesCSVArray[0];

                cmd.Parameters.Add(@"@Poke5", MySqlDbType.MediumText);
                cmd.Parameters[@"@Poke5"].Value = PokemonArray[0];
                cmd.Parameters.Add(@"@CSV5", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV5"].Value = MovesCSVArray[0];

                cmd.Parameters.Add(@"@Poke6", MySqlDbType.MediumText);
                cmd.Parameters[@"@Poke6"].Value = PokemonArray[0];
                cmd.Parameters.Add(@"@CSV6", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV6"].Value = MovesCSVArray[0];
            }
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
