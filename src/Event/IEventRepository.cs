namespace SEP_Backend.Event;

public interface IEventRepository
{
    Task<List<Event>> GetAllAsync();
    Task CreateAsync(Event @event);
}