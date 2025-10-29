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
    [Authorize(Policy = "TaskManagement")]
    public async Task CreateBatchTaskAsync([FromBody] ETask[] tasks) => await service.CreateBatchAsync(tasks);

    [HttpPut("tasks/{taskId:guid}")]
    [Authorize(Policy = "TaskManagement")]
    public async Task<bool> UpdateTaskAsync([FromRoute] Guid taskId, [FromBody] ETask task)
    {
        task.Id = taskId;
        return await service.UpdateAsync(task);
    }

    [HttpDelete("tasks/batch")]
    [Authorize(Policy = "TaskManagement")]
    public async Task DeleteBatchTaskAsync([FromBody] Guid[] taskIds) => await repository.DeleteBatchAsync(taskIds);
}