namespace ShieldJWT.Controllers;

[ApiController]
[Route("[controller]")]
public class TokenController : ShieldController
{
    private readonly ShieldJWTAbstract _shieldJWT;

    public TokenController(ShieldJWTAbstract shieldJWT)
    {
        _shieldJWT = shieldJWT;
    }

    [HttpPost]
    public IActionResult GenerateToken(TokenRequest request)
    {
        var token = _shieldJWT.GenerateToken(request.Audience);

        return Ok(token);
    }
}
