using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonSimulator
{
    public class PokemonTrainer
    {
        public int UserId { get; set; }
        public string TrainerName { get; set; }
        public List<string> Pokemon { get; set; }
    }
}
