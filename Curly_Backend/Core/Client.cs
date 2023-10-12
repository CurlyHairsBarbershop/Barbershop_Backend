using Microsoft.AspNetCore.Identity;

namespace Core;

public class Client : IdentityUser
{
    //public int Id { get; set; }
    public string SomeClientData { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
}