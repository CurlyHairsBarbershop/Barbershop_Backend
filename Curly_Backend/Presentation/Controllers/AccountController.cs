using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Models.Auth;
using Presentation.Services.AuthService;

namespace Presentation.Controllers;

[Route("account")]
[ApiController]
[Authorize(Roles = nameof(Client))]
public class AccountController : ControllerBase
{
    private readonly IAuthService<Client> _clientAuthService;
    private readonly UserManager<Client> _userManager;
    private readonly ILogger<AccountController> _logger;
    
    public AccountController(
        IAuthService<Client> clientAuthService, 
        UserManager<Client> userManager, 
        ILogger<AccountController> logger)
    {
        _clientAuthService = clientAuthService;
        _userManager = userManager;
        _logger = logger;
    }
    
    [AllowAnonymous]
    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        if (registerModel.Password != registerModel.ConfirmPassword)
        {
            return BadRequest("password and confirm password do not match");
        }
        
        var authResponse = await _clientAuthService.Register(registerModel);

        if (authResponse.Result.Succeeded == false)
        {
            return BadRequest(authResponse.Error);
        }
        
        return Created("/unsupported", new
        {
            token = authResponse.Token
        });
    }

    [AllowAnonymous]
    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        var loginResponse = await _clientAuthService.Login(loginModel);

        if (loginResponse.Result.Succeeded == false)
        {
            return BadRequest(loginResponse.Error);
        }
        
        return Accepted(new
        {
            token = loginResponse.Token
        });
    }

    [Route("")]
    [HttpGet]
    public async Task<IActionResult> My()
    {
        var userEmailClaim = Request.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email");
        var userInfo = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == userEmailClaim.Value);
        
        _logger.LogInformation("{Role} {Email} retrieved personal information successfully", nameof(Client), userInfo?.Email ?? "error");
        
        return Ok(new
        {
            name = userInfo.FirstName,
            lastName = userInfo.LastName,
            userInfo.Email,
            userInfo.PhoneNumber
        });
    }
}