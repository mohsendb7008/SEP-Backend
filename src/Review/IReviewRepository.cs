namespace SEP_Backend.Review;

public interface IReviewRepository
{
    Task<List<Review>> GetAllAsync();
    Task CreateAsync(Review review);
}