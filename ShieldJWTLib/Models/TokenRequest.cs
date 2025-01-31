namespace ShieldJWTLib.Models
{
    public class TokenRequest
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string Audience { get; set; }
    }
}
