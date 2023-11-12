namespace Core;

public class Barber : ApplicationUser
{
    public decimal Earnings => Convert.ToDecimal(Appointments?
        .Where(a => a.Favors != null) // Check if Appointments is null and if Favors is not null
        .SelectMany(a => a.Favors)
        .Where(f => f is not null) // Check if Cost is not null
        .Select(f => f.Cost)
        .DefaultIfEmpty(0)
        .Average() ?? 0);
    
    public string? Image { get; set; }
    
    public ICollection<Appointment>? Appointments { get; set; }
    
    public ICollection<Review>? Reviews { get; set; }
    
    public decimal Rating => Convert.ToDecimal(Reviews?
        .Select(r => r.Rating)
        .DefaultIfEmpty(0)
        .Average());
}