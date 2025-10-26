namespace SEP_Backend.ETask;

public interface ITaskRepository
{
    Task CreateBatchAsync(params ETask[] tasks);
    Task<List<ETask>> GetAllForUserAsync(Guid userId);
    Task<List<ETask>> GetAllForEventAsync(Guid eventId);
}