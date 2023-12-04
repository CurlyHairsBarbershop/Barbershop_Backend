using System.ComponentModel.DataAnnotations;

namespace API.Models.Admin;

public class CreateBarberRequest
{
    [Required] public required string Email { get; set; }
    
    [Required] public required string FirstName { get; set; }
    
    [Required] public required string LastName { get; set; }
    
    public string? Image { get; set; }
}