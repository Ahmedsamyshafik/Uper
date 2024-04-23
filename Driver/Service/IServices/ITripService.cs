using Driver.DTOs.Trips;
using Driver.Models;

namespace Driver.Service.IServices
{
    public interface ITripService
    {
        bool IsDriverBusy(string driverId);
        Task<string> AddTrip(Trip trip);
        Task<string> CompleteTrip(int TripID);
        Task<List<GetAllTripResponse>> GetAllTrips();
        Task<List<Trip>> GetDriverTrips(string DriverID);
    }
}
