using Microsoft.AspNetCore.Identity;

namespace Core;

public class Client : ApplicationUser
{
    public ICollection<Appointment>? Appointments { get; set; }
    
    public ICollection<Review>? Reviews { get; set; }
}