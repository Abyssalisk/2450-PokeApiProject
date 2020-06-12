using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonSimulator
{
    public class CreateLineUp
    {
        public PokemonTrainer GhostTrainer { get; set; }
        MySqlConnection Con { get; set; }

        public CreateLineUp(PokemonTrainer ghostTrainer, MySqlConnection con)
        {
            GhostTrainer = ghostTrainer;
            Con = con;
        }
    }
}
