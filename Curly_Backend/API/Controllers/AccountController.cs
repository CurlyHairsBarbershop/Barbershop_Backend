using API.Extensions.DTOs.Barbers;
using API.Global;
using API.Models.Account;
using API.Models.Auth;
using API.Services.AuthService;
using BLL.Services.Users;
using BLL.Services.Users.Barbers;
using Core;
using Infrustructure.Extensions;
using Infrustructure.Services.Emails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;

namespace API.Controllers;

[Route("account")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IAuthService<Client> _clientAuthService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly UserManager<Client> _clientManager;
    private readonly ILogger<AccountController> _logger;
    private readonly BarberService _barberService;
    private readonly EmailSender _emailSender;
    
    public AccountController(
        IAuthService<Client> clientAuthService, 
        UserManager<Client> clientManager, 
        ILogger<AccountController> logger,
        BarberService barberService, 
        EmailSender emailSender, UserManager<ApplicationUser> userManager)
    {
        _clientAuthService = clientAuthService;
        _clientManager = clientManager;
        _logger = logger;
        _barberService = barberService;
        _emailSender = emailSender;
        _userManager = userManager;
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
    [Route("forgot-password")]
    [HttpPost]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var client = await _userManager.FindByEmailAsync(request.Email);

        if (client is null) return BadRequest($"{request.Email} is not a valid application user");
        
        var token = await _userManager.GeneratePasswordResetTokenAsync(client);

        var link = new UriBuilder("http://194.1.220.48:5092/account/password-reset")
        {
            Query = string.Join("&", $"Token={token}", $"Email={request.Email}")
        };

        var body =
            $"<!DOCTYPE html>\n<html lang=\"en\">\n\n<head>\n    <meta charset=\"UTF-8\">\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <link href=\"https://fonts.googleapis.com/css2?family=Nunito:wght@400;700&display=swap\" rel=\"stylesheet\">\n    <title>BarberShop Password Reset</title>\n    <style>\n        body {{\n            font-family: 'Nunito', sans-serif;\n            background-color: #363636;\n            margin: 0;\n            padding: 0;\n        }}\n\n        .closing-message {{\n            color: #007bff;\n            font-size: 14px;\n            margin-top: 20px;\n        }}\n\n        .container {{\n            max-width: 600px;\n            margin: 40px auto;\n            padding: 30px;\n            background-color: #1c1c1c;\n            border-radius: 10px;\n            box-shadow: 0 0 15px rgba(0, 0, 0, 0.3);\n            color: #007bff;\n        }}\n\n        h1 {{\n            color: #007bff;\n            text-align: center;\n        }}\n\n        p {{\n            color: whitesmoke;\n            /* color: #007bff; */\n            text-align: justify;\n            margin-bottom: 20px;\n        }}\n\n        .reset-button {{\n            display: inline-block;\n            margin-top: 30px;\n            margin-bottom: 30px;\n            padding: 15px 25px;\n            font-size: 18px;\n            text-align: center;\n            text-decoration: none;\n            border-radius: 8px;\n            background-color: #007bff;\n            color: #ffffff;\n            cursor: pointer;\n        }}\n\n        .reset-button:hover {{\n            background-color: #0056b3;\n        }}\n    </style>\n</head>\n\n<body>\n    <div class=\"container\">\n        <h1>Password Reset</h1>\n        <p>\n            Dear {client.FirstName} {client.LastName},\n            <br><br>\n            We hope this email finds you well and groomed! It seems like you've requested a password reset for your\n            customer account. No worries, we've got you covered like a perfectly trimmed beard.\n            <br><br>\n            To proceed with resetting your password, simply click on the button below.\n        </p>\n        <div style=\"text-align: center;\">\n            <a class=\"reset-button\" href=\"{link}\">Reset Your Password</a>\n        </div>\n        <p>\n            If you didn't request this password reset, please ignore this email. Your account security is of the utmost\n            importance to us, and we'll keep your data as safe as a barber keeps their shears.\n            <br><br>\n        </p>\n        <p>Best regards,<br>The CurlyHairs support team</p>\n\n        <p class=\"closing-message\">\n            <i>\n                Thank you for choosing CurlyHairs – where style meets relaxation. If you encounter any issues or have\n                questions, feel free to reach out to our support team at curlysupport@gmail.com.\n            </i>\n        </p>\n\n    </div>\n</body>\n\n</html>";

        await _emailSender.SendHtmlEmail(request.Email, body);

        return Ok();
    }

    [HttpPost]
    [Route("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null) return BadRequest($"User {request.Email} was invalid");

        var resetResult = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        
        if (resetResult.Succeeded) return Ok("password reset successful");

        var errors = resetResult.Errors.SelectMany(e => e.Description);

        return BadRequest(errors);
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
        
        return Ok(new
        {
            token = loginResponse.Token
        });
    }

    [Route("")]
    [HttpGet]
    public async Task<IActionResult> My()
    {
        var memberEmail = Request.GetMemberEmail();
        var memberRole = Request.GetMemberRole();
        var userInfo = await _userManager.FindByEmailAsync(memberEmail);
        
        _logger.LogInformation("{Role} {Email} retrieved personal information successfully", memberRole, userInfo?.Email ?? "error");
        
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
            return BadRequest("Same values");
        }

        if (!(await _userManager.CheckPasswordAsync(userInfo, request.CurrentPassword)))
        {
            return BadRequest("Invalid current password");
        }

        if (await _userManager.CheckPasswordAsync(userInfo, request.NewPassword))
        {
            return BadRequest("Cannot change to current password");
        }

        var changeResult = await _userManager.ChangePasswordAsync(userInfo, request.CurrentPassword, request.NewPassword);

        return changeResult.Succeeded
            ? Ok()
            : StatusCode(StatusCodes.Status500InternalServerError, "could not update password");
    }

    [Route("favourite-barbers")]
    [HttpGet]
    [Authorize(Roles = Roles.Client)]
    public async Task<IActionResult> GetFavouriteBarbers()
    {
        var memberEmail = Request.GetMemberEmail();
        var userInfo = await _userManager.FindByEmailAsync(memberEmail);
        var favouriteBarbers = await _barberService.GetFavouriteBarbers(userInfo.Id);

        return Ok(JsonConvert.SerializeObject(favouriteBarbers.Select(f => f.ToBarberDto())));
    }
}