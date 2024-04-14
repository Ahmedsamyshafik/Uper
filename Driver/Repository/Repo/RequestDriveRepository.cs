using Driver.Models;
using Driver.Models.Data;
using Driver.Repository.GenericRepo;
using Driver.Repository.IRepo;
using Microsoft.EntityFrameworkCore;

namespace Driver.Repository.Repo
{
    public class RequestDriveRepository : GenericRepositoryAsync<RequestDrive>, IRequestDriveRepository
    {
        private readonly DbSet<RequestDrive> RequestDrive;

        public RequestDriveRepository(AppDbContext db) : base(db)
        {
            RequestDrive = db.Set<RequestDrive>();
        }
    }
}
