using Microsoft.AspNetCore.Identity;

namespace Core;

public class Barber : IdentityUser
{
    //public int Id { get; set; }
    public string SomeData { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
}