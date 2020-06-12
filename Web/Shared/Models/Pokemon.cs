using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Shared.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BaseHP { get; set; }
        public string Type { get; set; }
        public List<string> TypeWeaknesses { get; set; }
        public List<Move> Moves { get; set; }
        // @Sam any other object properties you need
    }
}
