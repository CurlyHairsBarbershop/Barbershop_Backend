using API.Global;
using API.Models.Admin;
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
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    { 
        var loginResponse = await _authService.Login(loginModel);

        if (loginResponse.Result.Succeeded) return Ok(new { token = loginResponse.Token });
        
        _logger.LogInformation("{Role} login failed for {Email}", Roles.Admin, loginModel.Email);
        
        return BadRequest(loginResponse.Error);
    }
    
    [AllowAnonymous]
    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] AdminRegisterModel registerModel)
    {
        if (registerModel.Password != registerModel.ConfirmPassword)
        {
            return BadRequest("password and confirm password do not match");
        }

        var newAdmin = new Admin
        {
            FirstName = registerModel.Name,
            LastName = registerModel.LastName,
            Email = registerModel.Email,
            AdminAlias = registerModel.Alias,
        };
        
        var authResponse = await _authService.Register(newAdmin, registerModel.Password);

        if (authResponse.Result.Succeeded == false)
        {
            return BadRequest(authResponse.Error);
        }
        
        return Created("/unsupported", new
        {
            token = authResponse.Token
        });
    }
}