namespace SEP_Backend.Budget;

public interface IBudgetRepository
{
    Task<List<Budget>> GetAllForEventAsync(Guid eventId);
    Task CreateAsync(Budget budget);
    Task<bool> UpdateAsync(Budget budget);
}