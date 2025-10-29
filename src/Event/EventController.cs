using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SEP_Backend.Event;

[ApiController]
public class EventController(IEventRepository repository, EventService service) : Controller
{
    [HttpGet("events")]
    [Authorize]
    public async Task<List<Event>> GetAllAsync() => await repository.GetAllAsync();

    [HttpGet("events/{eventId:guid}")]
    [Authorize]
    public async Task<Event?> GetByIdAsync([FromRoute] Guid eventId) => await repository.GetByIdAsync(eventId);

    [HttpPost("events/create")]
    [Authorize(Policy = "EventManagement")]
    public async Task CreateAsync([FromBody] EventRequest request) => await service.CreateAsync(request);

    [HttpPut("events/{eventId:guid}")]
    [Authorize(Policy = "EventManagement")]
    public async Task<bool> UpdateAsync([FromRoute] Guid eventId, [FromBody] Event @event)
    {
        @event.Id = eventId;
        return await repository.UpdateAsync(@event);
    }

    [HttpDelete("events/{eventId:guid}")]
    [Authorize(Policy = "EventManagement")]
    public async Task<bool> DeleteAsync([FromRoute] Guid eventId) => await repository.DeleteAsync(eventId);
}