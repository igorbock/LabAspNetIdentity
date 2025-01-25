using System.Collections.Generic;
using System.Security.Claims;

namespace ShieldJWTLib.Interfaces
{
    public interface IShieldJWT
    {
        string GenerateJWT(IEnumerable<Claim> claims = null);
    }
}
