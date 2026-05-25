using System.ComponentModel.DataAnnotations;

namespace GameTracker.ViewModels
{
    public class UserListItemViewModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }
        public string ApiKey { get; set; }
    }

    public class CreateUserViewModel
    {
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Rola")]
        public string Role { get; set; } = "Player";

        [Display(Name = "Klucz API")]
        public string ApiKey { get; set; }
    }
}
