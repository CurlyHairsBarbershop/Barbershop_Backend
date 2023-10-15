using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Presentation.Models.Auth.Barber;

public class RegisterModel
{
    public RegisterModel(string email, string name, string lastName, string password, string confirmPassword)
    {
        Email = email;
        Name = name;
        LastName = lastName;
        Password = password;
        ConfirmPassword = confirmPassword;
    }

    [EmailAddress]
    public required string Email { get; init; }
    
    public required string Name { get; init; }
    
    public required string LastName { get; init; }
    
    //public required string Gender { get; set; }
    
    //public required int Experience { get; set; }
    
    [JsonIgnore]
    [PasswordPropertyText]
    public required string Password { get; init; }
    
    [JsonIgnore]
    [PasswordPropertyText]
    public required string ConfirmPassword { get; init; }
}