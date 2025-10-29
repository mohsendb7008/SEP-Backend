using Microsoft.EntityFrameworkCore;

namespace SEP_Backend.Budget;

public class BudgetRepository(AppDbContext dbContext) : IBudgetRepository
{
    public async Task<List<Budget>> GetAllForEventAsync(Guid eventId)
    {
        var budgets = await dbContext.Budgets
            .Where(b => b.EventId == eventId)
            .Include(b => b.Event)
            .ToListAsync();
        return budgets;
    }

    public async Task<Budget?> GetByIdAsync(Guid budgetId) => await dbContext.Budgets.FindAsync(budgetId);

    public async Task CreateAsync(Budget budget)
    {
        await dbContext.Budgets.AddAsync(budget);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(Budget budget)
    {
        var existingBudget = await GetByIdAsync(budget.Id);
        if (existingBudget == null)
            return false;
        existingBudget.Update(budget);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid budgetId)
    {
        var budget = await GetByIdAsync(budgetId);
        if (budget == null)
            return false;
        dbContext.Budgets.Remove(budget);
        await dbContext.SaveChangesAsync();
        return true;
    }
}