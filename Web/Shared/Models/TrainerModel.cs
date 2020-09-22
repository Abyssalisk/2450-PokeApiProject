using System.Collections.Generic;

namespace Web.Shared.Models
{
    public class TrainerModel
    {
        public int Id { get; set; }
        public string Handle { get; set; }
        public int CurrentScore { get; set; }
        public int HighScore { get; set; }
        public int CurrentTeamId { get; set; }
        public LineupModel Team { get; set; } = new LineupModel();
        public List<LineupModel> Lineups { get; set; } = new List<LineupModel>();
        public string AvatarUrl { get; set; }
    }
}
