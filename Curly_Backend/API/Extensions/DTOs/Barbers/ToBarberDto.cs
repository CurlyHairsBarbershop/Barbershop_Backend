using Core;
using Infrustructure.DTOs.Barbers;

namespace API.Extensions.DTOs.Barbers;

public static partial class ToDtoExtensions
{
    public static BarberDTO ToBarberDto(this Barber b)
    {
        return new BarberDTO
        {
            Id = b.Id,
            Email = b.Email,
            Name = b.FirstName,
            LastName = b.LastName,
            PhoneNumber = b.PhoneNumber ?? string.Empty,
            Rating = b.Rating,
            ImageUrl = b.Image ?? string.Empty
        };
    }
}