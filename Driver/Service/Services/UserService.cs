using Driver.DTOs.Driver;
using Driver.Helpers;
using Driver.Models;
using Driver.Service.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Driver.Service.Services
{
    public class UserService : IUserService
    {
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITripService _tripService;
        #endregion

        #region CTOR
        public UserService(UserManager<ApplicationUser> userManager, ITripService tripService)
        {
            _userManager = userManager;
            _tripService = tripService;

        }
        #endregion

        #region Handle Functions
        public async Task<List<GetAvailableDriversResponse>> GetAllAvailableDrivers(bool smoking, string CarType)
        {
            var dbUsers = await _userManager.Users.Where(x => x.IsBusy == false && x.IsBlocked == false && x.IsActive == true)
                .ToListAsync();
            //All users now? => Role! 
            //Filteration
            if (smoking == false)
            {
                dbUsers = dbUsers.Where(x => x.IsSmoking == false).ToList();
            }
            if (CarType != null)
            {
                dbUsers = dbUsers.Where(x => x.CarType == CarType).ToList();
            }
            //return
            var response = new List<GetAvailableDriversResponse>();
            foreach (var user in dbUsers)
            {
                if (await _userManager.IsInRoleAsync(user, Constants.DriverRole) && !await _userManager.IsInRoleAsync(user, Constants.AdminRole))
                {
                    var temp = new GetAvailableDriversResponse();
                    temp.id = user.Id;
                    temp.Rate = await GetDriverRate(user.Id);
                    temp.Gender = user.Gender;
                    temp.Region = user.Region;
                    temp.CarType = user.CarType;
                    temp.Counter = user.Counter;
                    temp.imageUrl = user.imageUrl;
                    temp.IsSmoking = user.IsSmoking;

                    response.Add(temp);
                }

            }

            return response;
        }

        public async Task<GetDriverDetails> GetDriverDetails(string driverId)
        {
            var dbUser = await _userManager.FindByIdAsync(driverId);
            var response = new GetDriverDetails()
            {
                Address = dbUser.Address,
                CarType = dbUser.CarType,
                Counter = dbUser.Counter,
                Gender = dbUser.Gender,
                id = dbUser.Id,
                imageUrl = dbUser.imageUrl,
                IsBlocked = dbUser.IsBlocked,
                IsSmoking = dbUser.IsSmoking,
                name = dbUser.UserName,
                Rate = await GetDriverRate(dbUser.Id),
                Region = dbUser.Region,
                ImageUrl = dbUser.imageUrl
               
            };
            var DriverTrips = await _tripService.GetDriverTrips(driverId);
            var Trips = new List<DriverTripProberty>();
            foreach (var trip in DriverTrips)
            {
                var temp = new DriverTripProberty()
                {
                    id = trip.id,
                    PassengerName = trip.Passenger.UserName,
                    Price = trip.Price,
                    Source = trip.Source,
                    Target = trip.Target

                };
                Trips.Add(temp);
            }
            response.Trips = Trips;
            return response;
        }

        public async Task<string> AddRate(string DriverID, decimal Rate)
        {
            var Driver = await _userManager.FindByIdAsync(DriverID);
            Driver.UsersRating++;
            Driver.Rate += Rate;
            await _userManager.UpdateAsync(Driver);
            return "";
        }

        public async Task<List<GetAvailableDriversResponse>> GetLowRateDrivers()
        {
            var response = new List<GetAvailableDriversResponse>();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                if (await GetDriverRate(user.Id) <= 2 && await _userManager.IsInRoleAsync(user, Constants.DriverRole) &&
                    !await _userManager.IsInRoleAsync(user, Constants.AdminRole))
                {
                    var temp = new GetAvailableDriversResponse();
                    temp.id = user.Id;
                    temp.Rate = await GetDriverRate(user.Id);
                    temp.CarType = user.CarType;
                    temp.Counter = user.Counter;
                    temp.Gender = user.Gender;
                    temp.Region = user.Region;
                    temp.imageUrl = user.imageUrl;
                    temp.IsSmoking = user.IsSmoking;
                    temp.imageUrl = user.imageUrl;

                    response.Add(temp);
                }

            }
            return response;
        }


        public async Task<List<GetBlockedDrivers>> GetBlockedDrivers()
        {
            var response = new List<GetBlockedDrivers>();
            var BlockedUsers = await _userManager.Users.Where(x => x.IsBlocked == true).ToListAsync();
            foreach (var user in BlockedUsers)
            {
                var temp = new GetBlockedDrivers();
                temp.id = user.Id;
                temp.rate = await GetDriverRate(user.Id);
                temp.TripsCount = user.Counter;
                temp.ImageUrl = user.imageUrl;
                temp.name = user.UserName;

                response.Add(temp);
            }
            return response;
        }

        public async Task<string> BlockDriver(string driverId)
        {
            var user = await _userManager.FindByIdAsync(driverId);
            user.IsBlocked = true;
            await _userManager.UpdateAsync(user);
            return "";
        }

        public async Task<string> UnBlockDriver(string driverId)
        {
            var user = await _userManager.FindByIdAsync(driverId);
            user.IsBlocked = false;
            await _userManager.UpdateAsync(user);
            return "";
        }
        #endregion

        #region Private Funcs

        private async Task<decimal> GetDriverRate(string DriverID)
        {
            var user = await _userManager.FindByIdAsync(DriverID);
            //Calc Rate?

            decimal rate = 0;
            if (user.UsersRating > 0)
            {
                rate = user.Rate / user.UsersRating;
            }
            else
            {
                rate = 0;
            }
            return rate;
        }
        #endregion
    }
}
