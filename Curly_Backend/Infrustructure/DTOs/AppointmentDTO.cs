using Core;

namespace Infrustructure.DTOs.Barbers;

public class AppointmentDTO
{
    public int Id { get; set; }
    
    public DateTime At { get; set; }
    
    public BarberDTO Barber { get; set; }
    
    public List<FavorDTO> Favors { get; set; }

    public double TotalCost { get; set; }
}