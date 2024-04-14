namespace Driver.DTOs.UserDTos.Login
{
    public class LoginResponseDTO
    {

        public string UserName { get; set; }

        public string Email { get; set; }

        public string userId { get; set; }

        public string Gender { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }

        public string Region { get; set; }

        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
         

        public string? CarType { get; set; }

        public string? Address { get; set; }

        public DateTime Expier { get; set; }

    }
}
