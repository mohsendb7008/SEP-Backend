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

    public async Task<bool> DeleteAsync(Guid reviewId)
    {
        var review = await dbContext.Reviews.FindAsync(reviewId);
        if (review == null)
            return false;
        dbContext.Reviews.Remove(review);
        await dbContext.SaveChangesAsync();
        return true;
    }
}