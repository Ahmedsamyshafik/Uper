namespace Driver.DTOs.UserDTos.Admination
{
    public class ListUsers
    {
        public string name { get; set; }
        public string id { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string region { get; set; }
        public string address { get; set; }
        public string Role { get; set; }

        public string? ImageUrl { get; set; }
        public string? CarType { get; set; }
        public bool IsSmoking { get; set; }
        //IsSmoking - CarType

    }
}
