using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonSimulator
{
    public class Trainer
    {
        public int UserId { get; set; }
        public string TrainerName { get; set; }
        public List<Pokemon> Pokemon { get; set; }
        public Trainer()
        {

        }
        public Trainer(Trainer deep)
        {
            this.UserId = deep.UserId;
            this.TrainerName = string.Copy(deep.TrainerName);
            if (deep.Pokemon != null)
            {
                this.Pokemon = new List<Pokemon>();
                foreach (Pokemon p in deep.Pokemon)
                {
                    this.Pokemon.Add(new Pokemon(p));
                }
            }
        }
    }
}
