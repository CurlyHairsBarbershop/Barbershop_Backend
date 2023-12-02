using API.Models.Account;
using API.Models.Auth;
using API.Services.AuthService;
using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

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

    [Route("")]
    [HttpPost]
    public async Task<IActionResult> Edit([FromBody] EditPersonalInfoRequest request)
    {
        var userEmailClaim = Request.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email");
        var userInfo = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == userEmailClaim.Value);

        if (!string.IsNullOrWhiteSpace(request.LastName)) userInfo.LastName = request.LastName;
        if (!string.IsNullOrWhiteSpace(request.Name)) userInfo.FirstName = request.Name;

        var result = await _userManager.UpdateAsync(userInfo);

        return result.Succeeded
            ? Ok()
            : StatusCode(StatusCodes.Status500InternalServerError, "could not update information");
    }

    [Route("password")]
    [HttpPost]
    public async Task<IActionResult> ChangePassword([FromBody] EditPasswordRequest request)
    {
        var userEmailClaim = Request.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email");
        var userInfo = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == userEmailClaim.Value);
        
        if (request.CurrentPassword == request.NewPassword)
        {
            return BadRequest("same values");
        }

        if (!(await _userManager.CheckPasswordAsync(userInfo, request.CurrentPassword)))
        {
            return BadRequest("invalid current password");
        }

        if (await _userManager.CheckPasswordAsync(userInfo, request.NewPassword))
        {
            return BadRequest("cannot change to current password");
        }

        var changeResult = await _userManager.ChangePasswordAsync(userInfo, request.CurrentPassword, request.NewPassword);

        return changeResult.Succeeded
            ? Ok()
            : StatusCode(StatusCodes.Status500InternalServerError, "could not update password");
    }
}