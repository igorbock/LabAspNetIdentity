﻿namespace ShieldJWT.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class UserController : ShieldControllerAbstract
{
    private readonly IShieldUser _userService;

    public UserController(IShieldUser userService)
    {
        _userService = userService;
    }

    [HttpPost("create")]
    public IActionResult Create(CreateUser user)
        => Handler(_userService.Create, user);

    [HttpPost("login")]
    public IActionResult Login(TokenRequest request)
        => Handler(_userService.Login, request.User, request.Password);
}
