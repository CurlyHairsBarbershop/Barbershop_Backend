using System.ComponentModel.DataAnnotations;
using API.Models.Auth;

namespace API.Models.Admin;

public class AdminRegisterModel : RegisterModel
{
    [MinLength(3)] [Required] public string Alias { get; init; }
    
    public AdminRegisterModel(string email, string name, string lastName, string password, string confirmPassword, string alias) 
        : base(email, name, lastName, password, confirmPassword)
    {
        Alias = alias;
    }
}