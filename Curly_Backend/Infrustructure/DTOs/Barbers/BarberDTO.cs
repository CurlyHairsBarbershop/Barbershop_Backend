using Core;

namespace Infrustructure.DTOs.Barbers;

public class BarberDTO
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public decimal Earnings { get; set; }
    public decimal Rating { get; set; }
    public string Image { get; set; }
}