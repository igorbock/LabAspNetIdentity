namespace ShieldJWT.Controllers;

[ApiController]
[Route("[controller]")]
public class TesteController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok();
    }
}
