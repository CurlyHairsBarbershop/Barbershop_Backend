namespace Presentation.Models.Appointments;

public class CreateAppointmentRequest
{
    public DateTime At { get; set; }
    
    public int BarberId { get; set; }
    
    public List<int> ServiceIds { get; set; }
}