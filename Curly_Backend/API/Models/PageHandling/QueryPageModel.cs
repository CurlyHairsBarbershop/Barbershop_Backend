using System.ComponentModel.DataAnnotations;

namespace API.Models.PageHandling;

public class QueryPageModel
{
    [Required]
    public required int PageNumber { get; set; }
    
    [Required]
    public required int PageCount { get; set; }
}