using System.Collections.Generic;
using System.Security.Claims;

namespace ShieldJWTLib.Abstract
{
    public abstract class TokenServiceAbstract
    {
        protected readonly string _issuer;
        protected readonly string _key;
        protected readonly string _audience;
        public string Audience { get; set; }

        public TokenServiceAbstract(string issuer, string key)
        {
            _issuer = issuer;
            _key = key;
        }

        public abstract string GenerateToken(string username, string email, string audience, IEnumerable<Claim> claims = null);
    }
}
