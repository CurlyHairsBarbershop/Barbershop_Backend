namespace Core;

public class Appointment
{
    public int Id { get; set; }
    
    //TODO: to local time set local in db also
    public DateTime At
    {
        get => _at.ToLocalTime(); 
        set => _at = value.ToUniversalTime();
    }
    
    public bool IsCancelled { get; set; }
    
    public double TotalCost => Favors?.Select(f => f.Cost).DefaultIfEmpty(0).Sum() ?? 0.0;
    
    
    public DateTime PlacedAt { get; set; }
    
    public Barber Barber { get; set; }
    
    public Client Client { get; set; }
    
    public ICollection<Favor>? Favors { get; set; }
    
    
    private DateTime _at;
}