using Microsoft.EntityFrameworkCore;

namespace GameTracker.Models
{
    public class GameTrackerContext : DbContext
    {
        public GameTrackerContext(DbContextOptions<GameTrackerContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<GameLevel> GameLevels { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
    }
}