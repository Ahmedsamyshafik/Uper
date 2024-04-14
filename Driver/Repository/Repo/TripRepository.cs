using Driver.Models;
using Driver.Models.Data;
using Driver.Repository.GenericRepo;
using Driver.Repository.IRepo;
using Microsoft.EntityFrameworkCore;

namespace Driver.Repository.Repo
{
    public class TripRepository :GenericRepositoryAsync<Trip>,ITripRepository
    {
        private readonly DbSet<Trip> trips;

        public TripRepository(AppDbContext db) : base(db)
        {
            trips = db.Set<Trip>();
        }
    }
}
