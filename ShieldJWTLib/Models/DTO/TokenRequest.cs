using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace ShieldJWTLib.Models.DTO
{
    public class TokenRequest
    {
        public string User { get; set; }
        public string Password { get; set; }

        [JsonIgnore]
        public string Audience { get; set; }

        [JsonIgnore]
        public IEnumerable<Claim> Claims { get; set; }
    }
}
