namespace ShieldJWTLib.Models
{
    public class ChangePasswordRequest
    {
        public string EmailOrUsername { get; set; }
        public string NewPassword { get; set; }
    }
}
