using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.PageHandling;

public class QueryPageModel
{
    [Required]
    public required int PageNumber { get; set; }
    
    [Required]
    public required int PageCount { get; set; }
}