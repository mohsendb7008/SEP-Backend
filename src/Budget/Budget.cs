namespace SEP_Backend.Budget;

public class Budget
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Event.Event Event { get; set; }
    public decimal ProposedAmount { get; set; }
    public decimal NegotiatedAmount { get; set; }

    public void Update(Budget budget)
    {
        NegotiatedAmount = budget.NegotiatedAmount;
    }
}