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
        var existingEvent = await GetByIdAsync(@event.Id);
        if (existingEvent == null)
            return false;
        existingEvent.Update(@event);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid eventId)
    {
        var @event = await GetByIdAsync(eventId);
        if (@event == null)
            return false;
        dbContext.Events.Remove(@event);
        await dbContext.SaveChangesAsync();
        return true;
    }
}