using Microsoft.EntityFrameworkCore;

namespace SEP_Backend.Event;

public class EventRepository(AppDbContext dbContext) : IEventRepository
{
    public async Task<List<Event>> GetAllAsync()
    {
        var events = await dbContext.Events.ToListAsync();
        return events;
    }

    public async Task<Event?> GetByIdAsync(Guid eventId) =>
        await dbContext.Events.FindAsync(eventId);

    public async Task CreateAsync(Event @event)
    {
        await dbContext.Events.AddAsync(@event);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(Event @event)
    {
        var existingEvent = await dbContext.Events.FindAsync(@event.Id);
        if (existingEvent == null)
            return false;
        existingEvent.Update(@event);
        await dbContext.SaveChangesAsync();
        return true;
    }
}