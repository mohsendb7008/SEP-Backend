using Microsoft.AspNetCore.Mvc;

namespace SEP_Backend.User;

[ApiController]
public class UserController(IUserRepository repository) : Controller
{
    [HttpGet("users")]
    public async Task<List<User>> GetAllAsync() => await repository.GetAllAsync();

    [HttpPost("users/create")]
    public async Task<bool> CreateAsync([FromBody] User user) => await repository.CreateAsync(user);

    [HttpPut("users/update")]
    public async Task<bool> UpdateAsync([FromBody] User user) => await repository.UpdateAsync(user);

    [HttpDelete("users/delete")]
    public async Task<bool> DeleteAsync([FromQuery] Guid userId) => await repository.DeleteAsync(userId);
}