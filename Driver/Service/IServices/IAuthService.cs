using Driver.DTOs.UserDTos.Admination;
using Driver.DTOs.UserDTos.Login;
using Driver.DTOs.UserDTos.Register;
using Driver.Models;

namespace Driver.Service.IServices
{
    public interface IAuthService
    {
        Task<RegisterErrorResponeDTO> RegisterAsync(RegisterDTO model);
        Task<LoginResponseDTO> Login(LoginDTO model);
        Task<List<ListUsers>> ListOfUsers();
        Task<string> UserActivation(UserActivation activation);
        Task<ApplicationUser> GetUserById(string userid);
    }
}
