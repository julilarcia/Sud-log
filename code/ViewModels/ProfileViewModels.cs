namespace GameTracker.ViewModels
{
    public class ProfileViewModel
    {
        public string Login { get; set; }
        public string Role { get; set; }
        public int TotalPoints { get; set; }
        public int GamesPlayed { get; set; }
        public IEnumerable<AchievementRowViewModel> Achievements { get; set; } = new List<AchievementRowViewModel>();
        public IEnumerable<ScoreHistoryRowViewModel> ScoreHistory { get; set; } = new List<ScoreHistoryRowViewModel>();
    }

    public class AchievementRowViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime UnlockedAt { get; set; }
    }

    public class ScoreHistoryRowViewModel
    {
        public string LevelName { get; set; }
        public int Points { get; set; }
        public DateTime DateAchieved { get; set; }
    }
}
