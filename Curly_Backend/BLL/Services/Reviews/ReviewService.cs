using Core;
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BLL.Services.Reviews;

public class ReviewService
{
    private readonly ILogger<ReviewService> _logger;
    private readonly ApplicationContext _context;

    public ReviewService(ILogger<ReviewService> logger, ApplicationContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Review> Read(int reviewId)
    {
        var review = await _context.Reviews
            .Include(r => r.Publisher)
            .FirstOrDefaultAsync(r => r.Id == reviewId);

        if (review is null)
        {
            throw new InvalidDataException($"review id {reviewId} was invalid");
        }

        return review;
    }
}