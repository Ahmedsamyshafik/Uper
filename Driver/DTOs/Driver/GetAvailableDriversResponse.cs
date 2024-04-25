namespace Driver.DTOs.Driver
{
    public class GetAvailableDriversResponse
    {
        public string id { get; set; }
        public string? imageUrl { get; set; }//src="//"
        public string Region { get; set; }
        public bool IsSmoking { get; set; }
        public string? CarType { get; set; }
        public decimal Rate { get; set; }
        //عدد مرات الرحلات سواء لليوزر او الدرايفر
        public int Counter { get; set; }

        public string Gender { get; set; }
    }









}
