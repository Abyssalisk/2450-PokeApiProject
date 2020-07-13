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

        public bool SearchPokemonAsync(string name)
        {
            try
            {
                Task<PokemonSpecies> p = DataFetcher.GetNamedApiObject<PokemonSpecies>(name.Trim().ToLower());
                if (p.Result.Name.ToString() == name)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        public string AddToLineup(string name)
        {
            MoveSelector2000 ChooseMoves = new MoveSelector2000(name);
            ChooseMoves.DisplayMoves();
            string moves = "";
            for(int i = 0; i < 4; i++)
                moves += ChooseMoves.ChoseMove()+",";

            moves = moves.Remove((moves.Length)-1);
            Console.WriteLine(moves);
            Console.WriteLine("Lets add another Pokemon...");

            return moves;

        }


        public void AddPokemonToDB(string[] PokemonArray, string[] MovesCSVArray,Trainer GhostTrainer,MySqlConnection Con,bool HasLineup)
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
                cmd.Parameters[@"@Poke2"].Value = PokemonArray[1];
                cmd.Parameters.Add(@"@CSV2", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV2"].Value = MovesCSVArray[1];

                cmd.Parameters.Add(@"@Poke3", MySqlDbType.MediumText);
                cmd.Parameters[@"@Poke3"].Value = PokemonArray[2];
                cmd.Parameters.Add(@"@CSV3", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV3"].Value = MovesCSVArray[2];

                cmd.Parameters.Add(@"@Poke4", MySqlDbType.MediumText);
                cmd.Parameters[@"@Poke4"].Value = PokemonArray[3];
                cmd.Parameters.Add(@"@CSV4", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV4"].Value = MovesCSVArray[3];

                cmd.Parameters.Add(@"@Poke5", MySqlDbType.MediumText);
                cmd.Parameters[@"@Poke5"].Value = PokemonArray[4];
                cmd.Parameters.Add(@"@CSV5", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV5"].Value = MovesCSVArray[4];

                cmd.Parameters.Add(@"@Poke6", MySqlDbType.MediumText);
                cmd.Parameters[@"@Poke6"].Value = PokemonArray[5];
                cmd.Parameters.Add(@"@CSV6", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV6"].Value = MovesCSVArray[5];
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
                cmd.Parameters[@"@Poke2"].Value = PokemonArray[1];
                cmd.Parameters.Add(@"@CSV2", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV2"].Value = MovesCSVArray[1];

                cmd.Parameters.Add(@"@Poke3", MySqlDbType.MediumText);
                cmd.Parameters[@"@Poke3"].Value = PokemonArray[2];
                cmd.Parameters.Add(@"@CSV3", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV3"].Value = MovesCSVArray[2];

                cmd.Parameters.Add(@"@Poke4", MySqlDbType.MediumText);
                cmd.Parameters[@"@Poke4"].Value = PokemonArray[3];
                cmd.Parameters.Add(@"@CSV4", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV4"].Value = MovesCSVArray[3];

                cmd.Parameters.Add(@"@Poke5", MySqlDbType.MediumText);
                cmd.Parameters[@"@Poke5"].Value = PokemonArray[4];
                cmd.Parameters.Add(@"@CSV5", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV5"].Value = MovesCSVArray[4];

                cmd.Parameters.Add(@"@Poke6", MySqlDbType.MediumText);
                cmd.Parameters[@"@Poke6"].Value = PokemonArray[5];
                cmd.Parameters.Add(@"@CSV6", MySqlDbType.VarString);
                cmd.Parameters[@"@CSV6"].Value = MovesCSVArray[5];
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
