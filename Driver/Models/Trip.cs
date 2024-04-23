namespace Driver.Models
{
    public class Trip
    {
        public int id { get; set; }

        public string PassengerID { get; set; }
        public string DriverID { get; set; }

        public decimal Price { get; set; }

        public string Source { get; set; }
        public string Target { get; set; }
        public bool isComplete { get; set; }

        public virtual ApplicationUser Passenger { get; set; } // Navigation property to ApplicationUser
        public virtual ApplicationUser Driver { get; set; } // Navigation property to ApplicationUser

    }
}
