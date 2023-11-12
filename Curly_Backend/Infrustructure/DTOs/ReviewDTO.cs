using Infrustructure.DTOs.Barbers;

namespace Infrustructure.DTOs;

public class ReviewDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    
    public string Content { get; set; }
    
    public int Rating { get; set; }
    
    public PublisherDTO Publisher { get; set; }
    
    public List<ReplyDTO> Replies { get; set; }
}