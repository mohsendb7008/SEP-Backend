using Microsoft.EntityFrameworkCore;

namespace SEP_Backend.Review;

public class ReviewRepository(AppDbContext dbContext) : IReviewRepository
{
    public async Task<List<Review>> GetAllAsync()
    {
        var reviews = await dbContext.Reviews.Include(r => r.Event).ToListAsync();
        return reviews;
    }

    public async Task CreateAsync(Review review)
    {
        await dbContext.Reviews.AddAsync(review);
        await dbContext.SaveChangesAsync();
    }
}