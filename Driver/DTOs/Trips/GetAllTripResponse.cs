namespace Driver.DTOs.Trips
{
    public class GetAllTripResponse
    {
        public int id {  get; set; }

        public string PassengerName { get; set; }

        public string DriverName { get; set; }

        public string Source { get; set; }

        public string Target { get; set; }

        public bool Stuts { get; set; }

        public decimal Price { get; set; }

    }
}
