using System.ComponentModel.DataAnnotations;

namespace Driver.DTOs.UserDTos.Login
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]

        [MinLength(6)]
        [MaxLength(255)]
        public string Password { get; set; }
    }
}
