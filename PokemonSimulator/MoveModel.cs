using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Shared.Models
{
    public class MoveModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Damage { get; set; }
        public string ResourceUri { get; set; }
        public string Category { get; set; }
        public bool Seleted { get; set; }
    }
}
