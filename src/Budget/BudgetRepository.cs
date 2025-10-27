using Microsoft.EntityFrameworkCore;

namespace SEP_Backend.Budget;

public class BudgetRepository(AppDbContext dbContext) : IBudgetRepository
{
    public async Task<List<Budget>> GetAllForEventAsync(Guid eventId)
    {
        var budgets = await dbContext.Budgets.Where(b => b.EventId == eventId).ToListAsync();
        return budgets;
    }

    public async Task<Budget?> GetByIdAsync(Guid budgetId) => await dbContext.Budgets
        .Where(b => b.Id == budgetId).Include(b => b.Event).FirstAsync();

    public async Task CreateAsync(Budget budget)
    {
        await dbContext.Budgets.AddAsync(budget);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(Budget budget)
    {
        var existingBudget = await dbContext.Budgets.FindAsync(budget.Id);
        if (existingBudget == null)
            return false;
        existingBudget.Update(budget);
        await dbContext.SaveChangesAsync();
        return true;
    }
}