namespace ShieldJWT.Controllers;

[ApiController]
[Route("[controller]")]
public class TokenController : ControllerBase
{
    private readonly ShieldJWTAbstract _shieldJWT;

    public TokenController(ShieldJWTAbstract shieldJWT)
    {
        _shieldJWT = shieldJWT;
    }

    public IActionResult GenerateToken(TokenRequest request)
    {
        
    }
}
