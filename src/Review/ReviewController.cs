using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SEP_Backend.Event;

namespace SEP_Backend.Review;

[ApiController]
public class ReviewController(IReviewRepository repository, ReviewService service) : Controller
{
    [HttpGet("reviews")]
    [Authorize]
    public async Task<List<Review>> GetAllAsync() => await repository.GetAllAsync();

    [HttpPost("reviews/approve/{eventId:guid}")]
    [Authorize(Policy = "ReviewManagement")]
    public async Task ApproveAsync([FromRoute] Guid eventId, [FromBody] string comments) =>
        await service.CreateAsync(eventId, EventStatus.InProgress, comments);

    [HttpPost("reviews/reject/{eventId:guid}")]
    [Authorize(Policy = "ReviewManagement")]
    public async Task RejectAsync([FromRoute] Guid eventId, [FromBody] string comments) =>
        await service.CreateAsync(eventId, EventStatus.Rejected, comments);

    [HttpDelete("reviews/{reviewId:guid}")]
    [Authorize(Policy = "ReviewManagement")]
    public async Task<bool> DeleteAsync([FromRoute] Guid reviewId) => await repository.DeleteAsync(reviewId);
}