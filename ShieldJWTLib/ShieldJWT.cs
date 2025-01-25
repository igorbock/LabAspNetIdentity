using Microsoft.IdentityModel.Tokens;
using ShieldJWTLib.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShieldJWTLib
{
    public class ShieldJWT : IShieldJWT
    {
        private readonly string _audience;
        private readonly string _issuer;
        private readonly string _key;

        public ShieldJWT(string audience)
        {
            _audience = audience;
            _issuer = "ShieldJWT";
            _key = "a890597f580476d70368bd4c40081dc1bd6f6fb76512318f0fe92929f8cb2720";
        }

        public string GenerateJWT(IEnumerable<Claim> claims = null)
        {
            var expiration = DateTime.UtcNow.AddMinutes(60);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var newToken = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                expires: expiration,
                claims: claims,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.WriteToken(newToken);

            return jwt;
        }
    }
}
