using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SEP_Backend.ETask;

[ApiController]
public class TaskController(ITaskRepository repository, TaskService service) : Controller
{
    [HttpGet("tasks/event/{eventId:guid}")]
    [Authorize]
    public async Task<List<ETask>> GetEventTasksAsync([FromRoute] Guid eventId) =>
        await repository.GetAllForEventAsync(eventId);

    [HttpGet("tasks/user/{userId:guid}")]
    [Authorize]
    public async Task<List<ETask>> GetUserTasksAsync([FromRoute] Guid userId) =>
        await repository.GetAllForUserAsync(userId);

    [HttpPost("tasks/batch")]
    [Authorize(Policy = "Task")]
    public async Task CreateBatchTaskAsync([FromBody] ETask[] tasks) => await service.CreateBatchAsync(tasks);
}