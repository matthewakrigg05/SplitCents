namespace SplitCents.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using SplitCents.API.DTOs;
using SplitCents.API.Services;
using SplitCents.Core.Interfaces.Services;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtTokenService _jwtTokenService;

    public UsersController(IUserService userService, IJwtTokenService jwtTokenService)
    {
        _userService = userService;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = await _userService.RegisterAsync(
            request.Email,
            request.Password,
            request.DisplayName,
            request.FirstName,
            request.LastName);

        var token = _jwtTokenService.GenerateToken(user);

        return Created(
            $"/api/users/{user.id}",
            new RegisterResponse { User = user, Token = token });
    }
}
