using Driver.DTOs.Driver;

namespace Driver.Service.IServices
{
    public interface IUserService
    {
        Task<List<GetAvailableDriversResponse>> GetAllAvailableDrivers(bool smoking, string CarType);
        Task<GetDriverDetails> GetDriverDetails(string driverId);
        Task<string> AddRate(string DriverID, decimal Rate);
        Task<List<GetAvailableDriversResponse>> GetLowRateDrivers();
        Task<List<GetBlockedDrivers>> GetBlockedDrivers();
        Task<string> BlockDriver(string driverId);
        Task<string> UnBlockDriver(string driverId);
    }
}
