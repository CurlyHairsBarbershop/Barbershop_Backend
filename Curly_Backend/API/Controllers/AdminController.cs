using API.Models.Auth;
using API.Services.AuthService;
using BLL.Services.Appointments;
using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("admin")]
[Authorize(Roles = nameof(Admin))]
public class AdminController : ControllerBase
{
    private readonly IAuthService<Admin> _authService;
    private readonly UserManager<Barber> _barberManager;
    private readonly ILogger<AdminController> _logger;
    private readonly AppointmentService _appointmentService;

    public AdminController(
        IAuthService<Admin> authService,
        ILogger<AdminController> logger)
    {
        _authService = authService;
        _logger = logger;
    }
    
    [Route("login")]
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginModel loginModel)
    { 
        var memberEmail = Request.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == "Email")?.Value ?? "anonymous";
        
        var loginResponse = await _authService.Login(loginModel);

        if (loginResponse.Result.Succeeded == false)
        {
            _logger.LogError("{Email} could not login as admin, reason: {Message}", memberEmail, loginResponse.Error);
            return BadRequest(loginResponse.Error);
        }
        
        return Ok(new { token = loginResponse.Token });
    }
}