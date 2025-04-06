using ShieldJWTLib.Models.DTO;
using System;

namespace ShieldJWTLib.Interfaces
{
    public interface IShieldUser
    {
        ShieldReturnType Create(CreateUser newUser, Guid idCompany);
        ShieldReturnType ChangePassword(string email, string newPassword);
        ShieldReturnType ConfirmPassword(string email, string confirmationCode);
        ShieldReturnType Login(string username, string password);
    }
}
