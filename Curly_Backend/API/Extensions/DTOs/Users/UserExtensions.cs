using Core;
using Infrustructure.DTOs;

namespace API.Extensions.DTOs.Users;

public static class ToDtoExtensions
{
    public static PublisherDTO ToPublisherDto(this ApplicationUser source)
    {
        return new PublisherDTO
        {
            Email = source.Email,
            Name = source.FirstName,
            LastName = source.LastName
        };
    }
}