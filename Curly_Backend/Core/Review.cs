namespace Core;

public class Review
{
    public int Id { get; set; }
    
    public int Rating { get; set; }
    
    public string Title { get; set; }
    
    public string Content { get; set; }
    
    public Barber Barber { get; set; }
    
    public Client Publisher { get; set; }
}