using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Achievement
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } 
    
    public string Description { get; set; } 

    public ICollection<UserAchievement>? UserAchievements { get; set; }

}