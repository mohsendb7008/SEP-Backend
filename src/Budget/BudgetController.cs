using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SEP_Backend.Budget;

[ApiController]
public class BudgetController(IBudgetRepository repository, BudgetService service) : Controller
{
    [HttpGet("budgets/{eventId:guid}")]
    [Authorize]
    public async Task<List<Budget>> GetAllForEventAsync([FromRoute] Guid eventId) =>
        await repository.GetAllForEventAsync(eventId);

    [HttpPost("budgets/{eventId:guid}")]
    [Authorize(Policy = "BudgetManagement")]
    public async Task ProposeAsync([FromRoute] Guid eventId, [FromQuery] decimal amount) =>
        await service.ProposeBudget(eventId, amount);

    [HttpPut("budgets/{budgetId:guid}")]
    [Authorize(Policy = "BudgetManagement")]
    public async Task UpdateAsync([FromRoute] Guid budgetId, [FromQuery] decimal amount) =>
        await service.UpdateBudget(budgetId, amount);

    [HttpDelete("budgets/{budgetId:guid}")]
    [Authorize(Policy = "BudgetManagement")]
    public async Task<bool> DeleteAsync([FromRoute] Guid budgetId) => await repository.DeleteAsync(budgetId);
}