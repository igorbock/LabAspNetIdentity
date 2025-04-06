namespace ShieldJWT.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class UserController : ShieldControllerAbstract
{
    private readonly IShieldUser _userService;

    public UserController(IShieldUser userService, IShieldCompany companyService) : base(companyService)
    {
        _userService = userService;
    }

    [HttpPost("create")]
    public IActionResult Create(CreateUser user)
        => Handler((CreateUser request, Guid id) => _userService.Create(request, id), user, HttpContext.Request.Headers["X-Company-Header"]);

    [HttpPost("login")]
    public IActionResult Login(TokenRequest request)
        => Handler((string username, string password) => _userService.Login(username, password), request.User, request.Password);

    [HttpPost("confirm")]
    public IActionResult ConfirmCode(ConfirmCodeRequest request)
        => Handler((string email, string code) => _userService.ConfirmPassword(email, code), request.Email, request.ConfirmCode);

    [HttpPost("change")]
    public IActionResult ChangePassword(ChangePasswordRequest request)
        => Handler((string user, string newPassword) => _userService.ChangePassword(user, newPassword), request.EmailOrUsername, request.NewPassword);
}
