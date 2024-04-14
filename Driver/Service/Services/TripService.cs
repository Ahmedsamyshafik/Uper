using Driver.DTOs.Driver;
using Driver.Models;
using Driver.Repository.IRepo;
using Driver.Service.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Driver.Service.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _TripRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public TripService(ITripRepository TripRepository , UserManager<ApplicationUser> userManager)
        {
            _TripRepository = TripRepository;
            _userManager = userManager;
        }

        public bool IsDriverBusy(string driverId)
        {
            return _TripRepository.GetTableNoTracking().Where(x => x.DriverID == driverId && x.isComplete == false).Any();
        }
       
    }
}
