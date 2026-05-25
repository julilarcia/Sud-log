using System.ComponentModel.DataAnnotations;


namespace GameTracker.Models
{
public class GameLevel
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
    public double DifficultyMultiplier { get; set; } = 1.0;

    // RELACJE (Połączenie z pozostałymi tabelami):
    public ICollection<Score>? Scores { get; set; }
}
}