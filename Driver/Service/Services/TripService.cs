using Driver.DTOs.Driver;
using Driver.DTOs.Trips;
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
        public TripService(ITripRepository TripRepository, UserManager<ApplicationUser> userManager)
        {
            _TripRepository = TripRepository;
            _userManager = userManager;
        }

        public bool IsDriverBusy(string driverId)
        {
            return _TripRepository.GetTableNoTracking().Where(x => x.DriverID == driverId && x.isComplete == false).Any();
        }

        public async Task<string> AddTrip(Trip trip)
        {
            await _TripRepository.AddAsync(trip);
            return "";
        }

        public async Task<string> CompleteTrip(int TripID)
        {
            var trip = await _TripRepository.GetTableNoTracking().Where(x => x.id == TripID).FirstOrDefaultAsync();
            trip.isComplete = true;
            //Counter
            var passinger = await _userManager.FindByIdAsync(trip.PassengerID);
            passinger.Counter++;
            var Driver = await _userManager.FindByIdAsync(trip.DriverID);
            Driver.Counter++;
            await _userManager.UpdateAsync(Driver);
            await _userManager.UpdateAsync(passinger);
            await _TripRepository.UpdateAsync(trip);
            return "";
        }

        public async Task<List<GetAllTripResponse>> GetAllTrips()
        {
            var trips=await _TripRepository.GetTableNoTracking().Include(x=>x.Driver).Include(u=>u.Passenger)
                .ToListAsync();
            var response=new List<GetAllTripResponse>();
            foreach (var trip in trips)
            {
                var temp=new GetAllTripResponse();  
                temp.id = trip.id;
                temp.Source = trip.Source;
                temp.Target= trip.Target;
                temp.Stuts = trip.isComplete;
                temp.DriverName = trip.Driver.UserName;
                temp.PassengerName = trip.Passenger.UserName;
                temp.Price = trip.Price;
                
                response.Add(temp);
            }
            return response;
        }

        public async Task<List<Trip>> GetDriverTrips(string DriverID)
        {
            return await _TripRepository.GetTableNoTracking().Include(d=>d.Driver).Include(p=>p.Passenger).Where(x=>x.DriverID==DriverID).ToListAsync();
        }
    }
}
