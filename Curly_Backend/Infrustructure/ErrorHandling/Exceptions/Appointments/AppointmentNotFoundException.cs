namespace Infrustructure.ErrorHandling.Exceptions.Appointments;

public class AppointmentNotFoundException : Exception
{
    public AppointmentNotFoundException(int id) 
        : base($"appointment with id {id} was not found") { }
}