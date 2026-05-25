using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameTracker.Models
{
    public class UserAchievement
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateUnlocked { get; set; } = DateTime.Now;

        // KLUCZE OBCE
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        public int AchievementId { get; set; }
        [ForeignKey("AchievementId")]
        public Achievement? Achievement { get; set; }

    }
}