using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace PokemonSimulator
{
    class LoadOpponent
    {
        public string OpponentName;

        public List<Pokemon> OpponentLineUp;
        public LoadOpponent(int rank, MySqlConnection con)
        {
            OpponentLineUp = new List<Pokemon>();
            OpponentName = FindOpponent(rank, con);
            LoadPokemonFromDB loading = new LoadPokemonFromDB(OpponentName, con);
            OpponentLineUp = loading.LoadedLineUp;
        }

        public string FindOpponent(int rank, MySqlConnection con)
        {
            string query = "SELECT TrainerName FROM sql3346222.EliteFour WHERE(Rank = " + rank + ");";
            string returnedQuery = "";

            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnedQuery = reader[0].ToString();
                }
            }
            con.Close();
            return returnedQuery;
        }
    }
}
