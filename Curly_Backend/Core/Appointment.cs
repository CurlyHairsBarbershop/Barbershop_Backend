namespace Core;

public class Appointment
{
    private DateTime _at;
    public int Id { get; set; }

    public DateTime At
    {
        get => _at.ToLocalTime(); 
        set => _at = value.ToUniversalTime();
    }
    
    public DateTime PlacedAt { get; set; }
    
    public Barber Barber { get; set; }
    
    public Client Client { get; set; }
    
    public ICollection<Favor> Favors { get; set; }
}