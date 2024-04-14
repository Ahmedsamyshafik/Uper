namespace Driver.Models
{
    public class Trip
    {
        public string id { get; set; }

        public string PassengerID { get; set; }
        public string DriverID { get; set; }

        public decimal Price { get; set; }

        public string Source { get; set; }
        public string Target { get; set; }
        public bool isComplete { get; set; }

        public ApplicationUser User { get; set; }   

    }
}
