using Microsoft.AspNetCore.Mvc;

namespace SEP_Backend.User;

[ApiController]
public class LoginController(IUserRepository repository, JwtTokenGenerator jwtTokenGenerator) : Controller
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromQuery] Guid userId)
    {
        var user = await repository.GetByIdAsync(userId);
        if (user == null)
            return Unauthorized();
        var token = jwtTokenGenerator.Generate(user);
        return Ok(new { token });
    } 
}