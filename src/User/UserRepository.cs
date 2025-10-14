using Microsoft.EntityFrameworkCore;

namespace SEP_Backend.User;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<List<User>> GetAllAsync()
    {
        var users = await dbContext.Users.ToListAsync();
        return users;
    }

    public async Task<User?> GetByIdAsync(Guid userId) =>
        await dbContext.Users.FindAsync(userId);

    public async Task<bool> CreateAsync(User user)
    {
        var exists = await dbContext.Users.FindAsync(user.Id) != null; 
        if (exists)
            return false;
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(User user)
    {
        var existingUser = await dbContext.Users.FindAsync(user.Id);
        if (existingUser == null)
            return false;
        existingUser.Update(user);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid userId)
    {
        var user = await dbContext.Users.FindAsync(userId);
        if (user == null)
            return false;
        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();
        return true;
    }
}