using Microsoft.AspNetCore.Identity;

namespace Core;

public class Barber : ApplicationUser
{
    public ICollection<Appointment> Appointments { get; set; }
}