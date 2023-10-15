using Core;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.Auth.Barber;
using Presentation.Services.AuthService;

namespace Presentation.Controllers;

[Route("account/barber")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAuthService<Barber> _barberAuthService;
    
    public AccountController(IAuthService<Barber> barberAuthService)
    {
        _barberAuthService = barberAuthService;
    }
    
    [HttpPost("")]
    public async Task<IActionResult> RegisterBarber([FromBody] RegisterModel registerModel)
    {
        var authResponse = await _barberAuthService.Register(registerModel);

        if (authResponse.Result.Succeeded)
        {
            return Created("/unsupported", new
            {
                token = authResponse.Token
            });
        }

        return StatusCode(StatusCodes.Status500InternalServerError, "could not register barber");
    }
}