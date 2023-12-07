using System.ComponentModel.DataAnnotations;

namespace API.Models.Account;

public class ResetPasswordRequest
{
    [Required] public string Email { get; init; }
    
    [Required] public string NewPassword { get; init; }
    
    [Required] public string Token { get; init; }
}