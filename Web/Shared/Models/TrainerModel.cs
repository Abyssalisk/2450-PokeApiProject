using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Shared.Models
{
    public class TrainerModel
    {
        public int Id { get; set; }
        public string Handle { get; set; }
        public int CurrentScore { get; set; }
        public int HighScore { get; set; }
        public LineupModel Team { get; set; } // current lineup
        public List<LineupModel> Lineups { get; set; } = new List<LineupModel>(); // all lineups from previous teams
    }
}
