using System.ComponentModel.DataAnnotations;

namespace API.Models.Barbers;

public class EditBarberRequest
{
    public string? Name { get; set; }
    public string? LastName { get; set; }
    
    [EmailAddress(ErrorMessage = "Barber email must be of valid format")]
    public string? Email { get; set; }
    
    /// <summary>
    /// Base64 string if new image or name if media exists on the server
    /// </summary>
    public string? Image { get; set; }
}