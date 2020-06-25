using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonSimulator
{
    class ScoreBoard
    {
        int Wins;
        Trainer GhostTrainer;
        MySqlConnection Con;
        string[] Pokemon = new string[6];
        string[] MovesCSV = new string[6];

        public ScoreBoard(int wins, Trainer ghosttrainer, MySqlConnection con)
        {
            Wins = wins;
            GhostTrainer = ghosttrainer;
            Con = con;
            PokeExtractor();
            AddToEliteFour();
        }

        public void AddToEliteFour()
        {
            int rank = 6 - Wins;
            if(Wins>0)
            {
                Con.Open();
                string query = "UPDATE sql3346222.EliteFour SET UserId="+GhostTrainer.UserId+", " +
                    "Rank="+rank+",TrainerName='"+GhostTrainer.TrainerName+"',Pokemon1='"+Pokemon[0]+
                    "',MovesCSV1='"+MovesCSV[0]+ "',Pokemon2='" + Pokemon[1] +
                    "',MovesCSV2='" + MovesCSV[1] + "',Pokemon3='" + Pokemon[2] +
                    "',MovesCSV3='" + MovesCSV[2] + "',Pokemon4='" + Pokemon[3] +
                    "',MovesCSV4='" + MovesCSV[3] + "',Pokemon5='" + Pokemon[4] +
                    "',MovesCSV5='" + MovesCSV[4] + "',Pokemon6='" + Pokemon[5] +
                    "',MovesCSV6='" + MovesCSV[5] + "' WHERE( Rank = " +rank+");";

                MySqlCommand cmd = new MySqlCommand(query, Con);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    string returnedmessage = (rdr[0]).ToString();
                }
                rdr.Close();
                Con.Close();
            }
            else
            {
                return;
            }
        }

        public void PokeExtractor()
        {
            for (int i = 0; i < GhostTrainer.Pokemon.Count; i++)
            {
                Pokemon[i] = GhostTrainer.Pokemon[i].Species;
                MovesCSV[i] = string.Join(',', GhostTrainer.Pokemon[i].ConsoleMoves.Select(x => x.Name));/*MakeCSV(pokemon.ConsoleMoves);*/
            }
        }

        //public string MakeCSV(List<Move> moves)
        //{
        //    string csv = null;
        //    foreach (Move element in moves)
        //        csv += element.Name + ",";
        //    csv.Remove(csv.Length-2);
        //    return csv;
        //}
    }
}
