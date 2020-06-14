using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Shared.Models
{
    class BattleModel
    {
        public int Score { get; set; }
        public TrainerModel Player { get; set; }
        public TrainerModel Computer { get; set; }
        // @Sam any other object properties you need
    }
}
