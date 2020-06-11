using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonSimulator
{
    class CreateLineUp
    {
        PokemonTrainer ghostTrainer;
        MySqlConnection con;

        public CreateLineUp(PokemonTrainer ghostTrainer, MySqlConnection con)
        {
            this.ghostTrainer = ghostTrainer;
            this.con = con;
        }
    }
}
