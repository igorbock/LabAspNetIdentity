namespace ShieldJWT.Controllers;

[ApiController]
[Route("[controller]")]
public class TokenController : ShieldControllerAbstract
{
    private readonly TokenServiceAbstract _shieldJWT;

    public TokenController(TokenServiceAbstract shieldJWT)
    {
        _shieldJWT = shieldJWT;
    }

    //[HttpPost]
    //public IActionResult GenerateToken(TokenRequest request)
    //    => Handler<string, IEnumerable<Claim>, string>(_shieldJWT.GenerateToken, request.Audience, null);
}
