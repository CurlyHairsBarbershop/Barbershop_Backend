namespace Infrustructure.DTOs;

public class ReplyDTO
{
    public int Id { get; set; }
    public required string Content { get; set; }
    public required PublisherDTO Publisher { get; set; }
    public required IEnumerable<ReplyDTO> Replies { get; set; }
}