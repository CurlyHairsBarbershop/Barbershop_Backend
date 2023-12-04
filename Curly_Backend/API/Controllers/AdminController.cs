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
    private readonly IAuthService<Admin> _clientAuthService;
    private readonly UserManager<Admin> _adminManager;
    private readonly UserManager<Barber> _barberManager;
    private readonly ILogger<AccountController> _logger;
    private readonly AppointmentService _appointmentService;

    public AdminController(
        IAuthService<Admin> clientAuthService, 
        UserManager<Admin> adminManager, 
        ILogger<AccountController> logger)
    {
        _clientAuthService = clientAuthService;
        _adminManager = adminManager;
        _logger = logger;
    }
    
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
}