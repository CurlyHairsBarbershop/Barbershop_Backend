using System.Security.Claims;
using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Models.Auth.Barber;
using Presentation.Models.Auth.Login;
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
    [Route("")]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        var authResponse = await _clientAuthService.Register(registerModel);

        if (authResponse.Result.Succeeded)
        {
            _logger.LogInformation("{Role} {Email} has registered successfully", nameof(Client), registerModel.Email);
           
            return Created("/unsupported", new
            {
                token = authResponse.Token
            });
        }

        _logger.LogError("{Role} {Email} could not register", nameof(Client), registerModel.Email);
        
        return StatusCode(StatusCodes.Status500InternalServerError, "could not register client");
    }

    [AllowAnonymous]
    [Route("")]
    [HttpGet]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        var loginResponse = await _clientAuthService.Login(loginModel);

        if (loginResponse.Result.Succeeded)
        {
            _logger.LogInformation("{Role} {Email} has logged in successfully", nameof(Client), loginModel.Email);
            return Accepted(new
            {
                token = loginResponse.Token
            });
        }
        
        _logger.LogInformation("{Role} {Email} could not log in", nameof(Client), loginModel.Email);
        
        return StatusCode(StatusCodes.Status500InternalServerError, "could not login client");
    }

    [Route("my")]
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