using API.Models.Auth;
using API.Models.Options;
using API.Services.JwtProvider;
using Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Services.AuthService;

public class AuthService<TUser> : IAuthService<TUser> where TUser : ApplicationUser, new()
{
    private readonly UserManager<TUser> _userManager;
    private readonly IOptions<JwtProviderOptions> _options;
    private readonly ILogger<AuthService<TUser>> _logger;
    
    public AuthService(
        UserManager<TUser> userManager, 
        IOptions<JwtProviderOptions> options, 
        ILogger<AuthService<TUser>> logger)
    {
        _userManager = userManager;
        _options = options;
        _logger = logger;
    }
    
    public async Task<(SignInResult Result, string? Token, string? Error)> Register(TUser createUser, string password)
    {
        var userExists = _userManager.Users.Any(b => createUser.Email == b.Email);

        if (userExists)
        {
            _logger.LogWarning(
                "Could not register {Role} {Email}: already exists", 
                typeof(TUser).Name, createUser.Email);
            return (SignInResult.NotAllowed, null, "member already exists");
        }

        createUser.UserName ??= createUser.Email;
        
        var result = await _userManager.CreateAsync(createUser, password);

        if (!result.Succeeded) return (SignInResult.Failed, null, result.Errors.FirstOrDefault()?.Description);
        
        var user = await _userManager.FindByEmailAsync(createUser.Email);
        var role = typeof(TUser).Name;
            
        await _userManager.AddToRoleAsync(user, role);
            
        var token = new JwtProvider<TUser>(_options.Value)
            .GenerateJwtTokenString(user, role);
            
        _logger.LogInformation("{Role} {Email} has registered successfully", role, createUser.Email);
            
        return (SignInResult.Success, token, null);

    }

    public async Task<(SignInResult Result, string? Token, string? Error)> Register(RegisterModel registerModel)
    {
        var userExists = _userManager.Users.Any(b => registerModel.Email == b.Email);

        if (userExists)
        {
            _logger.LogWarning(
                "Could not register {Role} {Email}: already exists", 
                typeof(TUser).Name, registerModel.Email);
            return (SignInResult.NotAllowed, null, "member already exists");
        }

        var newUser = new TUser
        {
            Email = registerModel.Email,
            UserName = registerModel.Email,
            FirstName = registerModel.Name,
            LastName = registerModel.LastName,
        };
        
        var result = await _userManager.CreateAsync(newUser, registerModel.Password);

        if (!result.Succeeded) return (SignInResult.Failed, null, result.Errors.FirstOrDefault()?.Description);
        
        var user = await _userManager.FindByEmailAsync(newUser.Email);
        var role = typeof(TUser).Name;
            
        await _userManager.AddToRoleAsync(user, role);
            
        var token = new JwtProvider<TUser>(_options.Value)
            .GenerateJwtTokenString(user, role);
            
        _logger.LogInformation("{Role} {Email} has registered successfully", role, registerModel.Email);
            
        return (SignInResult.Success, token, null);

    }
    
    public async Task<(SignInResult Result, string? Token, string? Error)> Login(LoginModel loginModel)
    {
        var user = await _userManager.FindByEmailAsync(loginModel.Email);
        
        var role = typeof(TUser).Name;
        
        if (user is null)
        {
            _logger.LogWarning("{Role} {Email} invalid login email", role, loginModel.Email);
            return (SignInResult.Failed, null, "invalid email");
        }

        var passwordValid = await _userManager.CheckPasswordAsync(user, loginModel.Password);

        if (!passwordValid)
        {
            _logger.LogWarning("{Role} {Email} invalid login password", role, loginModel.Email);
            return (SignInResult.NotAllowed, null, "invalid password");
        }
        
        var token = new JwtProvider<TUser>(_options.Value).GenerateJwtTokenString(user, role);

        _logger.LogInformation("{Role} {Email} has logged in successfully", role, loginModel.Email);
        
        return (SignInResult.Success, token, null);
    }
}