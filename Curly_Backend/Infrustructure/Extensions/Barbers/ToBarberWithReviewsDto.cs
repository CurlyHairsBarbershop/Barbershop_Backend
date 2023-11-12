using Core;
using Infrustructure.DTOs;
using Infrustructure.DTOs.Barbers;
using Infrustructure.Extensions.Users;

namespace Infrustructure.Extensions.Barbers;

public static partial class BarberExtensions
{
    public static BarberWithReviewsDTO ToBarberWithReviewsDto(this Barber b)
    {
        return new BarberWithReviewsDTO
        {
            Id = b.Id,
            Email = b.Email ?? string.Empty,
            Name = b.FirstName,
            LastName = b.LastName,
            PhoneNumber = b.PhoneNumber ?? string.Empty,
            Earnings = b.Earnings,
            Rating = b.Rating,
            ImageUrl = b.Image ?? string.Empty,
            Reviews = b.Reviews?
                .Where(r => r.ReplyTo is null)
                .Select(r => new ReviewDTO
                {
                    Id = r.Id,
                    Title = r.Title,
                    Content = r.Content,
                    Rating = r.Rating,
                    Publisher = r.Publisher.ToPublisherDto(),
                    Replies = GetReplies(r, b.Reviews?.ToList() ?? new())
                }).ToList()
        };
    }

    private static List<ReplyDTO> GetReplies(Review review, List<Review> allReviews)
    {
        return allReviews
            .Where(r => r.ReplyTo == review.Id)
            .Select(reply => new ReplyDTO
            {
                Id = reply.Id,
                Content = reply.Content,
                Publisher = reply.Publisher.ToPublisherDto(),
                Replies = GetReplies(reply, allReviews)
            }).ToList();
    }
}