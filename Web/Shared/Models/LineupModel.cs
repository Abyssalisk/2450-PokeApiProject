using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Shared.Models
{
    public class LineupModel
    {
        public List<PokemonModel> Lineup { get; set; } = new List<PokemonModel>();
        public bool Checked { get; set; }
        public string Text { get; set; }
    }
}
