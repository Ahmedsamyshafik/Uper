namespace Driver.DTOs.Driver
{
    public class AllDriverRequestedResponseDTO
    {
        public int id { get; set; }

        public string name { get; set; }

        public DateTime dateTime { get; set; }
        public decimal price { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
    }
}
