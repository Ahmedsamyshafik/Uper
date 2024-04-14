using Driver.Models;

namespace Driver.DTOs.UserDriver
{
    public class RequestDriverDTO
    {

        public string PassingerID { get; set; }

        public string DriverID { get; set; }


        public string Source { get; set; }

        public string Target { get; set; }

        public decimal price { get; set; }

    }
}
