using AuthService.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthService.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly Domain.AuthService authService;

    public UserController(Domain.AuthService authService)
    {
        this.authService = authService;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AuthRequest user)
    {
        var token = await authService.Authenticate(user.Login, user.Password);

        if (token == null)
            return BadRequest(new { message = "Username or password incorrect" });

        return Ok(new { Token = token });
    }

    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest user)
    {
        var register = await authService.Register(user.login, user.password, user.avatar);
        if (!register)
            return BadRequest(new { message = "Username already exists" });
        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost("avatar")]
    public async Task<IActionResult> Avatar([FromForm] UploadAvatarRequest request)
    {
        var avatar = await authService.UploadAvatar(request.file, CancellationToken.None);
        return Ok(avatar);
    }

    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?["Bearer ".Length..].Trim();
        return Ok(authService.AgentFromLogin(token));
    }
}