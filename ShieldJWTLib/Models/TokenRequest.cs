using System.Collections.Generic;
using System.Security.Claims;

namespace ShieldJWTLib.Models
{
    public class TokenRequest
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string Audience { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }
}
