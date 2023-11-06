using Core;
using Infrustructure.DTOs.Barbers;

namespace Infrustructure.Extensions.Barbers;

public static partial class BarberExtensions
{
    public static BarberDTO ToBarberDto(this Barber b)
    {
        return new BarberDTO
        {
            Email = b.Email ?? string.Empty,
            Name = b.FirstName,
            LastName = b.LastName,
            PhoneNumber = b.PhoneNumber ?? string.Empty,
            Earnings = b.Earnings,
            Rating = b.Rating,
            ImageUrl = b.Image ?? string.Empty
        };
    }
}