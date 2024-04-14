using Driver.DTOs.UserDriver;
using Driver.Models;

namespace Driver.Service.IServices
{
    public interface IRequestDriveService
    {
        Task<RequestDriverResponseDTO> AddRequest(RequestDriverDTO requestDriverDTO);
        Task<List<RequestDrive>> GetDriverRequests(string driverId);
    }
}
