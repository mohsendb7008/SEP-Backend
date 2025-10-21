using Microsoft.EntityFrameworkCore;

namespace SEP_Backend.Event;

public class EventRepository(AppDbContext dbContext) : IEventRepository
{
    public async Task<List<Event>> GetAllAsync()
    {
        var events = await dbContext.Events.ToListAsync();
        return events;
    }

    public async Task CreateAsync(Event @event)
    {
        await dbContext.Events.AddAsync(@event);
        await dbContext.SaveChangesAsync();
    }
}