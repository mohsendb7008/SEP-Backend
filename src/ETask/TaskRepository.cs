using Microsoft.EntityFrameworkCore;

namespace SEP_Backend.ETask;

public class TaskRepository(AppDbContext dbContext) : ITaskRepository
{
    public async Task<List<ETask>> GetAllForUserAsync(Guid userId) => await dbContext.Tasks
        .Where(t => t.UserId == userId).Include(t => t.Event).ToListAsync();

    public async Task<List<ETask>> GetAllForEventAsync(Guid eventId) => await dbContext.Tasks
        .Where(t => t.EventId == eventId).Include(t => t.User).ToListAsync();

    public async Task<ETask?> GetByIdAsync(Guid taskId) => await dbContext.Tasks.FindAsync(taskId);

    public async Task CreateBatchAsync(params ETask[] tasks)
    {
        await dbContext.Tasks.AddRangeAsync(tasks);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(ETask task)
    {
        var existingTask = await GetByIdAsync(task.Id);
        if (existingTask == null)
            return false;
        existingTask.Update(task);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task DeleteBatchAsync(params Guid[] taskIds)
    {
        var tasks = await dbContext.Tasks.Where(t => taskIds.Contains(t.Id)).ToListAsync();
        dbContext.Tasks.RemoveRange(tasks);
        await dbContext.SaveChangesAsync();
    }
}