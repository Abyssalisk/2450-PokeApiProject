using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Shared.Models
{
    public class TrainerModel
    {
        public int Id { get; set; }
        public string Handle { get; set; }
        public int Score { get; set; }
        public List<PokemonModel> Team { get; set; }
        // @Sam any other object properties you need
    }
}
