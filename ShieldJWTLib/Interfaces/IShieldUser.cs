using ShieldJWTLib.Models;

namespace ShieldJWTLib.Interfaces
{
    public interface IShieldUser
    {
        ShieldReturnType Create(string email, string username, string newPassword);
        ShieldReturnType ChangePassword(string email, string newPassword);
        ShieldReturnType ConfirmPassword(string email, string confirmationCode);
    }
}
