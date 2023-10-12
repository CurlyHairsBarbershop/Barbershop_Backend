namespace Core;

public class Appointment
{
    public int Id { get; set; }
    
    public DateTime At { get; set; }
    
    public Barber Barber { get; set; }
    
    public Client Client { get; set; }
    
    public ICollection<Favor> Favors { get; set; }
}