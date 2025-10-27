using SEP_Backend.Event;

namespace SEP_Backend.Budget;

public class BudgetService(IBudgetRepository repository, IEventRepository eventRepository)
{
    public async Task ProposeBudget(Guid eventId, decimal amount)
    {
        var @event = await eventRepository.GetByIdAsync(eventId);
        if (@event == null)
            throw new BadHttpRequestException("Event not found");
        if (amount < 1)
            throw new BadHttpRequestException("Amount is too small");
        var budget = new Budget
        {
            EventId = eventId,
            ProposedAmount = amount
        };
        await repository.CreateAsync(budget);
    }

    public async Task UpdateBudget(Guid budgetId, decimal amount)
    {
        if (amount < 1)
            throw new BadHttpRequestException("Amount is too small");
        var budget = new Budget
        {
            Id = budgetId,
            NegotiatedAmount = amount
        };
        var success = await repository.UpdateAsync(budget);
        if (!success)
            throw new BadHttpRequestException("Budget not found");
    }
}