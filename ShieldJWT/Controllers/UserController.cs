namespace ShieldJWT.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ShieldControllerAbstract
{
    private readonly IShieldUser _userService;

    public UserController(IShieldUser userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public IActionResult Create(CreateUser user)
        => Handler(_userService.Create, user);
}
