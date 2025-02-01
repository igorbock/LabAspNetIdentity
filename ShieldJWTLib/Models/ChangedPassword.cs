using System;

namespace ShieldJWTLib.Models
{
    public class ChangedPassword
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string NewHash { get; set; }
        public string ConfimationCode { get; set; }
        public bool Confirmed { get; set; }
        public DateTime Date { get; set; }
    }
}
