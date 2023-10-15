using Microsoft.AspNetCore.Identity;

namespace Core;

public class Client : ApplicationUser
{
    public ICollection<Appointment> Appointments { get; set; }
}