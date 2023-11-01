using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Core;

public class Client : ApplicationUser
{
    [JsonIgnore]
    public ICollection<Appointment>? Appointments { get; set; }
    
    [JsonIgnore]
    public ICollection<Review>? Reviews { get; set; }
}