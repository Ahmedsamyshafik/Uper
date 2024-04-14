using Microsoft.AspNetCore.Identity;

namespace Driver.Models
{
    public class ApplicationUser : IdentityUser
    {
        //Name Email Password Role
        public string? imageUrl { get; set; }
        public string Region { get; set; }
        public string? Address { get; set; }
        public bool IsSmoking { get; set; }
        public string? CarType { get; set; }
        public bool IsBusy { get; set; }
        public bool IsBlocked { get; set; } 
        public decimal Rate { get; set; }
        //عدد مرات الرحلات سواء لليوزر او الدرايفر
        public int Counter { get; set; }

        public bool IsActive { get; set; }

        public string Gender { get; set; }





    }
}
