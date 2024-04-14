using Driver.DTOs.UserDriver;
using Driver.Models;
using Driver.Repository.IRepo;
using Driver.Service.IServices;
using Microsoft.EntityFrameworkCore;

namespace Driver.Service.Services
{
    public class RequestDriveService : IRequestDriveService
    {
        private readonly IRequestDriveRepository _requestDriveRepository;
        private readonly ITripService _tripService;
        public RequestDriveService(IRequestDriveRepository requestDriveRepository, ITripService tripService)
        {
            _requestDriveRepository = requestDriveRepository;
            _tripService = tripService;
        }

        public async Task<RequestDriverResponseDTO> AddRequest(RequestDriverDTO requestDriverDTO)
        {
            //Check if Avilable
            bool isBusy = _tripService.IsDriverBusy(requestDriverDTO.DriverID);
            if (isBusy) { return new RequestDriverResponseDTO { busy = true }; }
            //mapping (RequestDriverDTO=> RequestDrive)
            var saving = new RequestDrive()
            {
                DriverID = requestDriverDTO.DriverID,
                DateTime = DateTime.UtcNow,
                PassingerID = requestDriverDTO.PassingerID,
                price = requestDriverDTO.price,
                Source = requestDriverDTO.Source,
                Target = requestDriverDTO.Target,
                
            };
            await _requestDriveRepository.AddAsync(saving);
            return new RequestDriverResponseDTO { busy=false };

        }

        public async Task<List<RequestDrive>> GetDriverRequests(string driverId)
        {
            return await _requestDriveRepository.GetTableNoTracking().Where(x=>x.DriverID==driverId).ToListAsync();
        }
    }
}
