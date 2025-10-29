namespace SEP_Backend.Event;

public interface IEventRepository
{
    Task<List<Event>> GetAllAsync();
    Task<Event?> GetByIdAsync(Guid eventId);
    Task CreateAsync(Event @event);
    Task<bool> UpdateAsync(Event @event);
    Task<bool> DeleteAsync(Guid eventId);
}