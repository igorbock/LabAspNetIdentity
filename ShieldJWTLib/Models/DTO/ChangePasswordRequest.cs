namespace ShieldJWTLib.Models.DTO
{
    public class ChangePasswordRequest
    {
        public string EmailOrUsername { get; set; }
        public string NewPassword { get; set; }
    }
}
