using Core;

namespace API.Services.JwtProvider;

public interface IJwtProvider<in TUser> where TUser : ApplicationUser
{
    string GenerateJwtTokenString(TUser user, params string[] roles);
}