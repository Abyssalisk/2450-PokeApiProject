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
        public List<PokemonType> Types { get; set; } = new List<PokemonType>();
        public List<string> TypeWeaknesses { get; set; } = new List<string>();
        public List<string> HardlyEffectiveTypes { get; set; } = new List<string>();
        public List<string> NotEffectiveTypes { get; set; }
        public List<MoveModel> Moves { get; set; } = new List<MoveModel>();
        public string BackImageUri { get; set; }
        public string FrontImageUri { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpecialAttack { get; set; }
        public int SpecialDefense { get; set; }
        public int ActingHP { get; set; }
        public bool IsAlive { get => ActingHP > 0; }
        public List<MoveModel> MoveSelections { get; set; } = new List<MoveModel>() { new MoveModel(), new MoveModel(), new MoveModel(), new MoveModel() }; // stores moves
    }
}
