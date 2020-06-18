using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Shared.Models
{
    public class PokemonType
    {
        public string Name { get; set; }
        public List<string> Weaknesses { get; set; } = new List<string>();
        public string ResourceUri { get; set; }

    }
}
