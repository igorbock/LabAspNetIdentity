namespace ShieldJWT.Controllers;

[ApiController]
[Route("[controller]")]
public class TesteController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public TesteController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return Ok(_configuration["Environment"]);
    }
}
