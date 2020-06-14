using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonSimulator
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BaseHP { get; set; }
        public string Type { get; set; }
        public int Speed { get; set; }
        public List<string> TypeWeaknesses { get; set; }
        public List<Move> Moves { get; set; }
        // @Sam any other object properties you need


        public Pokemon()
        {

        }
    }
}
