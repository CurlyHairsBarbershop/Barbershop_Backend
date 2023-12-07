using System.ComponentModel.DataAnnotations;

namespace API.Models.Favors;

public class CreateFavorRequest
{
    [MinLength(4)][Required] public string Name { get; init; }
    
    public string? Description { get; init; }
    
    [Required] public double Cost { get; init; }
}