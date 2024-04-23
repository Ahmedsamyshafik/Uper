namespace Driver.DTOs.Driver
{
    public class GetDriverDetails
    {
        public string id {  get; set; }
        public string name { get; set; }
        public string? imageUrl { get; set; }
        public string Region { get; set; }
        public string? Address { get; set; }
        public bool IsSmoking { get; set; }
        public string? CarType { get; set; }
      
        public bool IsBlocked { get; set; }
        public decimal Rate { get; set; }
        //عدد مرات الرحلات سواء لليوزر او الدرايفر
        public int Counter { get; set; }

        public string? ImageUrl { get; set; }
        public string Gender { get; set; }

        public ICollection<DriverTripProberty>? Trips { get; set; }
    }
    public class DriverTripProberty
    {
        public int id { get; set; }

        public string PassengerName { get; set; }

        public string Source { get; set; }
        public string Target { get; set; }
        public decimal Price { get; set; }
    }
}
