using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.Auth.Login;

public class LoginModel
{
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}