namespace Core;

public class Barber : ApplicationUser
{
    public decimal Earnings => Convert.ToDecimal(Appointments?.SelectMany(a => a.Favors).Average(f => f.Cost) ?? 0);
    
    public string? Image { get; set; }
    
    public ICollection<Appointment>? Appointments { get; set; }
    
    public ICollection<Review>? Reviews { get; set; }
    
    public decimal Rating => Convert.ToDecimal(Reviews?.Select(r => r.Rating).DefaultIfEmpty(0).Average());
}