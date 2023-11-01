using Core;
using Microsoft.AspNetCore.Identity;
using Presentation.Models.Auth;

namespace Presentation.Services.AuthService;

public interface IAuthService<TUser> where TUser : ApplicationUser
{
    Task<(SignInResult Result, string? Token, string? Error)> Register(RegisterModel registerModel);
    Task<(SignInResult Result, string? Token, string? Error)> Login(LoginModel loginModel);
}