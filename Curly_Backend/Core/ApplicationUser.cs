using Microsoft.AspNetCore.Identity;

namespace Core;

public class ApplicationUser : IdentityUser<int>
{
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public new required string Email { get => base.Email; set => base.Email = value; }
}