using System.ComponentModel.DataAnnotations;

namespace API.Models.Auth;

public class LoginModel
{
    [Required] public required string Email { get; init; }
    
    [Required] public required string Password { get; init; }
}