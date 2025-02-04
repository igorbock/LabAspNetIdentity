namespace ShieldJWT.Services;

public class TokenService : TokenServiceAbstract
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration) : base(configuration["JWT:Issuer"], configuration["JWT:Key"])
    {
        _configuration = configuration;
    }

    public override string GenerateToken(string username, string email, string audience, IEnumerable<Claim> claims = null)
    {
        if (string.IsNullOrEmpty(audience))
            throw new ArgumentNullException(nameof(Audience));

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

        if (claims == null)
            claims = new List<Claim>();

        var subject = new Claim(ClaimTypes.NameIdentifier, username);
        var emailClaim = new Claim (ClaimTypes.Email, email);
        claims.Append(subject);
        claims.Append(emailClaim);


        var jwt = new JwtSecurityToken(
            issuer: _issuer,
            audience: audience,
            expires: DateTime.Now.AddMinutes(60),
            claims: claims,
            signingCredentials: credentials);

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(jwt);
            return token;
        }
        catch (SecurityTokenEncryptionFailedException ex)
        {
            return $"A criptografia do token falhou! Confira se a chave tem o mínimo de caracteres e se está correta. Erro: {ex.Message}";
        }
    }
}
