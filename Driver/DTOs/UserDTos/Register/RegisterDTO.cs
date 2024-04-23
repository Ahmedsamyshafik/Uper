using System.ComponentModel.DataAnnotations;

namespace Driver.DTOs.UserDTos.Register
{
    public class RegisterDTO
    {
        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string UserType { get; set; }

        public string? Address { get; set; }

        public string Region { get; set; }

        public string Gender { get; set; }
   

      
        //If Driver
        public IFormFile? Image { get; set; }
        public bool IsSmoking { get; set; }
        public string? CarType { get; set; }


    }
}
