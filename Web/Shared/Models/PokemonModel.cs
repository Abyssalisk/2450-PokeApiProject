using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Shared.Models
{
    public class PokemonModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BaseHP { get; set; }
        public int Speed { get; set; }
        public string Type { get; set; }
        public List<string> TypeWeaknesses { get; set; }
        public List<MoveModel> Moves { get; set; }
        // @Sam any other object properties you need
    }
}
