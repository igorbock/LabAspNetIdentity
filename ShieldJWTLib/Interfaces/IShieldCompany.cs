using ShieldJWTLib.Models;
using System;

namespace ShieldJWTLib.Interfaces
{
    public interface IShieldCompany
    {
        void ValidateCompany(Guid idCompany);
        void CreateLog(Log log);
    }
}
