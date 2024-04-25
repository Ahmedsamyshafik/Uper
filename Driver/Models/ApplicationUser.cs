using Microsoft.AspNetCore.Identity;

namespace Driver.Models
{
    public class ApplicationUser : IdentityUser
    {
       
        public string? imageUrl { get; set; }
        public string Region { get; set; }
        public string? Address { get; set; }
        public bool IsSmoking { get; set; }
        public string? CarType { get; set; }
        public bool IsBusy { get; set; }
        public bool IsBlocked { get; set; } 
        public decimal Rate { get; set; }
        public int UsersRating { get; set; }
        //عدد مرات الرحلات سواء لليوزر او الدرايفر
        public int Counter { get; set; }

        public decimal TotalPrice { get; set; }

        public bool IsActive { get; set; }

        public string Gender { get; set; }

        //Navigations
        public ICollection<RequestDrive> Requests { get; set; }
        public ICollection<Trip> Trips { get; set; }
      



    }
}
