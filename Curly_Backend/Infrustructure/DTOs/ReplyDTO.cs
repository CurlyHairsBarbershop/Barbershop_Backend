namespace Infrustructure.DTOs.Barbers;

public class ReplyDTO
{
    public required string Content { get; set; }
    
    public required PublisherDTO Publisher { get; set; }
    
    public required IEnumerable<ReplyDTO> Replies { get; set; }
}