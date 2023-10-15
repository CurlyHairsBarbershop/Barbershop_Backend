using Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Presentation.Models.Auth.Barber;
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
        var barberExits = _userManager.Users.Any(b => registerModel.Email == b.Email);

        if (barberExits)
        {
            return (SignInResult.Failed, null);
        }

        var newBarber = new TUser
        {
            Email = registerModel.Email,
            UserName = registerModel.Email,
            FirstName = registerModel.Name,
            LastName = registerModel.LastName
        };
        
        var result = await _userManager.CreateAsync(newBarber);

        if (result.Succeeded)
        {
            var barber = await _userManager.FindByEmailAsync(newBarber.Email);
            var role = typeof(TUser).Name;
            
            await _userManager.AddToRoleAsync(barber, role);
            
            var token = new JwtProvider<TUser>(_options.Value)
                .GenerateJwtTokenString(barber, role);
            
            return (SignInResult.Success, token);
        }

        return (SignInResult.Failed, null);
    }
}