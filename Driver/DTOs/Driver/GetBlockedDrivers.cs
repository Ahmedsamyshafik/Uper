namespace Driver.DTOs.Driver
{
    public class GetBlockedDrivers
    {
        public string id {  get; set; }
        public string name { get; set; }

        public string ImageUrl { get; set; }
        public decimal rate { get; set; }

       
        public int TripsCount { get; set; }
    }
}
