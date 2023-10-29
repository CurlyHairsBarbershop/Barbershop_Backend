namespace Core;

public class Barber : ApplicationUser
{
    public decimal Earnings => Convert.ToDecimal(Appointments.SelectMany(a => a.Favors).Average(f => f.Cost));
    
    public string? Image { get; set; }
    
    public ICollection<Appointment> Appointments { get; set; }
    
    public ICollection<Review> Reviews { get; set; }
    
    public decimal Rating => Convert.ToDecimal(Reviews.Average(r => r.Rating));
}