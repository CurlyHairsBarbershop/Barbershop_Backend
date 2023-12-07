using Infrustructure.DTOs.Barbers;

namespace Infrustructure.DTOs;

public class AppointmentDTO
{
    public int Id { get; set; }
    
    public DateTime At { get; set; }
    
    public BarberDTO Barber { get; set; }
    
    public List<FavorDTO> Favors { get; set; }
    
    public bool Cancelled { get; set; }

    public double TotalCost { get; set; }
}