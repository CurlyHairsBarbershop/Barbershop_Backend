namespace Core;

public class Review : Reply
{
    public string Title { get; set; }
    
    public int Rating { get; set; }
    
    public Barber Barber { get; set; }
}