using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Infrustructure.Extensions;

public static class RequestHelperExtensions
{
    /// <summary>
    /// Gets member email from the request
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Member email or empty string if none</returns>
    public static string GetMemberEmail(this HttpRequest request)
    {
        var memberEmail = request.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == "Email")?.Value ?? string.Empty;

        return memberEmail;
    }

    /// <summary>
    /// Gets member role from the request
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Member role or empty string if none</returns>
    public static string GetMemberRole(this HttpRequest request)
    {
        var memberRole = request.HttpContext.User.Claims
                             .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;

        return memberRole;
    }
}