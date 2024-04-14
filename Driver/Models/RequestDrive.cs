﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Driver.Models
{
    public class RequestDrive
    {
        public int id { get; set; }
        public string PassingerID { get; set; }

        public string DriverID { get; set; }

        public DateTime DateTime { get; set; }

        public string Source { get; set; }

        public string Target { get; set; }

        public decimal price { get; set; }

        public virtual ApplicationUser? applicationUser { get; set; }
    }
}
