using Core;
using Infrustructure.DTOs;

namespace Infrustructure.Extensions.Users;

public static class UserExtensions
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