using API.Models.Auth;
using Core;
using Microsoft.AspNetCore.Identity;

namespace API.Services.AuthService;

public interface IAuthService<TUser> where TUser : ApplicationUser
{
    Task<(SignInResult Result, string? Token, string? Error)> Register(RegisterModel registerModel);
    Task<(SignInResult Result, string? Token, string? Error)> Login(LoginModel loginModel);
}