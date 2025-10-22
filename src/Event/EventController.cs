using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SEP_Backend.Event;

[ApiController]
public class EventController(IEventRepository repository, EventService service) : Controller
{
    [HttpGet("events")]
    [Authorize]
    public async Task<List<Event>> GetAllAsync() => await repository.GetAllAsync();

    [HttpPost("events/create")]
    [Authorize(Roles = "CustomerServiceOfficer")]
    public async Task CreateAsync([FromBody] EventRequest request) => await service.CreateAsync(request);
}