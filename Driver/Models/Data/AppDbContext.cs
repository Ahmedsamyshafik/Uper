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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure RequestDrive-ApplicationUser (Passenger) relationship
            modelBuilder.Entity<RequestDrive>()
                .HasOne(rd => rd.Passenger)
                .WithMany(u => u.Requests)
                .HasForeignKey(rd => rd.PassingerID)
                .OnDelete(DeleteBehavior.Restrict); // Disable cascading delete

            // Configure RequestDrive-ApplicationUser (Driver) relationship
            modelBuilder.Entity<RequestDrive>()
                .HasOne(rd => rd.Driver)
                .WithMany() // Assuming you don't need navigation property for driver's requests
                .HasForeignKey(rd => rd.DriverID)
                .OnDelete(DeleteBehavior.Restrict); // Disable cascading delete

            // Configure Trip-ApplicationUser (Passenger) relationship
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Passenger)
                .WithMany(u => u.Trips)
                .HasForeignKey(t => t.PassengerID)
                .OnDelete(DeleteBehavior.Restrict); // Disable cascading delete

            // Configure Trip-ApplicationUser (Driver) relationship
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Driver)
                .WithMany() // Assuming you don't need navigation property for driver's trips
                .HasForeignKey(t => t.DriverID)
                .OnDelete(DeleteBehavior.Restrict); // Disable cascading delete


        }
    }
}
