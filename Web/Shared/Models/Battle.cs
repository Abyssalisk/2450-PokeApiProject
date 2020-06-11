using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Shared.Models
{
    class Battle
    {
        public int Score { get; set; }
        public Trainer Player { get; set; }
        public Trainer Computer { get; set; }
        // @Sam any other object properties you need
    }
}
