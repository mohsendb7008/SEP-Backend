namespace SEP_Backend.ETask;

public interface ITaskRepository
{
    Task<List<ETask>> GetAllForUserAsync(Guid userId);
    Task<List<ETask>> GetAllForEventAsync(Guid eventId);
    Task<ETask?> GetByIdAsync(Guid taskId);
    Task CreateBatchAsync(params ETask[] tasks);
    Task<bool> UpdateAsync(ETask task);
    Task DeleteBatchAsync(params Guid[] taskIds);
}