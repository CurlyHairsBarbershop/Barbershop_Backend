using Core;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.Auth.Barber;
using Presentation.Models.Auth.Login;
using Presentation.Services.AuthService;

namespace Presentation.Controllers;

[Route("account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAuthService<Client> _clientAuthService;
    
    public AccountController(IAuthService<Client> clientAuthService)
    {
        _clientAuthService = clientAuthService;
    }
    
    [HttpPost("")]
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        var authResponse = await _clientAuthService.Register(registerModel);

        if (authResponse.Result.Succeeded)
        {
            return Created("/unsupported", new
            {
                token = authResponse.Token
            });
        }

        return StatusCode(StatusCodes.Status500InternalServerError, "could not register client");
    }

    [HttpGet("")]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        var loginResponse = await _clientAuthService.Login(loginModel);

        if (loginResponse.Result.Succeeded)
        {
            return Accepted(new
            {
                token = loginResponse.Token
            });
        }
        
        return StatusCode(StatusCodes.Status500InternalServerError, "could not login client");
    }
}