using Microsoft.EntityFrameworkCore;

namespace SEP_Backend.ETask;

public class TaskRepository(AppDbContext dbContext) : ITaskRepository
{
    public async Task CreateBatchAsync(params ETask[] tasks)
    {
        await dbContext.Tasks.AddRangeAsync(tasks);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<ETask>> GetAllForUserAsync(Guid userId) =>
        await dbContext.Tasks.Where(t => t.UserId == userId).ToListAsync();

    public async Task<List<ETask>> GetAllForEventAsync(Guid eventId) =>
        await dbContext.Tasks.Where(t => t.EventId == eventId).ToListAsync();
}