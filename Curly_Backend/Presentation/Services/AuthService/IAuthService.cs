using Core;
using Microsoft.AspNetCore.Identity;
using Presentation.Models.Auth.Barber;

namespace Presentation.Services.AuthService;

public interface IAuthService<TUser> where TUser : ApplicationUser
{
    Task<(SignInResult Result, string? Token)> Register(RegisterModel registerModel);
}