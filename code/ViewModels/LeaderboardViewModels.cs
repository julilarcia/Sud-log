namespace GameTracker.ViewModels
{
    public class LeaderboardRowViewModel
    {
        public string Login { get; set; }
        public int TotalPoints { get; set; }
        public int GamesPlayed { get; set; }
        public DateTime LastPlayed { get; set; }
    }
}
