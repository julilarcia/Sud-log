using System.ComponentModel.DataAnnotations;

namespace GameTracker.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

    [Required]
    public string Login { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

        // Dodane pod wymagania projektu:
        public string Role { get; set; } = "Player"; // Domyślnie zwykły gracz, "Admin" dla administratora

    public string? ApiKey { get; set; } // Token dla REST API

        // RELACJE (Połączenie z pozostałymi tabelami):
        public ICollection<Score>? Scores { get; set; }
        public ICollection<UserAchievement>? UserAchievements { get; set; }
    }
}