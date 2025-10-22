namespace SEP_Backend.Event;

public class EventService(IEventRepository repository, EventFactory factory, EventRequestValidator validator)
{
    public async Task CreateAsync(EventRequest request)
    {
        var isValid = validator.IsValid(request, out var error);
        if (!isValid)
            throw new BadHttpRequestException(error ?? "");
        var @event = factory.Create(request);
        await repository.CreateAsync(@event);
    }
}