using System.ComponentModel.DataAnnotations;

namespace API.Models.Barbers;

public class BarberCommentary
{
    [Required]
    public required string BarberEmail { get; set; }
    
    [Required]
    public required string Content { get; set; }
    
    [Required]
    public required int Rating { get; set; }

    [Required]
    public required string Title { get; set; }
}