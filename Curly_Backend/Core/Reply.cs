using Newtonsoft.Json;

namespace Core;

public class Reply
{
    public int Id { get; set; }
    
    public string Content { get; set; }
    
    public ApplicationUser Publisher { get; set; }
    
    [JsonIgnore]
    public int PublisherId { get; set; }
    
    public int ReplyTo { get; set; }
}