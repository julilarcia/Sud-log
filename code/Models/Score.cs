using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Score
{
    [Key]
        public int Id { get; set; }
        
        public int Points { get; set; }
        public DateTime DateAchieved { get; set; } = DateTime.Now;

        // KLUCZE OBCE (Foreign Keys)
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        public int GameLevelId { get; set; }
        [ForeignKey("GameLevelId")]
        public GameLevel? GameLevel { get; set; }

}