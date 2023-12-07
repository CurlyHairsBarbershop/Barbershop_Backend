namespace API.Models.Favors;

public class UpdateFavorRequest
{
    public string? Name { get; init; }
    
    public string? Description { get; init; }
    
    public double? Cost { get; init; }
}