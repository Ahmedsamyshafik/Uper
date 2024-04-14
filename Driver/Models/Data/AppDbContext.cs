using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Driver.Models.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {   
        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<RequestDrive> RequestDrive { get; set; }
        public DbSet<Trip> trips { get; set; }

    }
}
