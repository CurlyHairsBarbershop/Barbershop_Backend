using Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Presentation.Models.Auth.Barber;
using Presentation.Models.Auth.Login;
using Presentation.Models.Options;
using Presentation.Services.JwtProvider;

namespace Presentation.Services.AuthService;

public class AuthService<TUser> : IAuthService<TUser> where TUser : ApplicationUser, new()
{
    private readonly UserManager<TUser> _userManager;
    private readonly IOptions<JwtProviderOptions> _options;
    
    public AuthService(
        UserManager<TUser> userManager, 
        IOptions<JwtProviderOptions> options)
    {
        _userManager = userManager;
        _options = options;
    }

    public async Task<(SignInResult Result, string? Token)> Register(RegisterModel registerModel)
    {
        var userExists = _userManager.Users.Any(b => registerModel.Email == b.Email);

        if (userExists)
        {
            return (SignInResult.Failed, null);
        }

        var newUser = new TUser
        {
            Email = registerModel.Email,
            UserName = registerModel.Email,
            FirstName = registerModel.Name,
            LastName = registerModel.LastName
        };
        
        var result = await _userManager.CreateAsync(newUser);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(newUser.Email);
            var role = typeof(TUser).Name;
            
            await _userManager.AddToRoleAsync(user, role);
            
            var token = new JwtProvider<TUser>(_options.Value)
                .GenerateJwtTokenString(user, role);
            
            return (SignInResult.Success, token);
        }

        return (SignInResult.Failed, null);
    }
    
    public async Task<(SignInResult Result, string? Token)> Login(LoginModel loginModel)
    {
        var userExists = await _userManager.Users.AnyAsync(u => loginModel.Email == u.Email);

        if (!userExists)
        {
            return (SignInResult.Failed, null);
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginModel.Email);
        var role = typeof(TUser).Name;
        var token = new JwtProvider<TUser>(_options.Value).GenerateJwtTokenString(user, role);

        return (SignInResult.Success, token);
    }
}