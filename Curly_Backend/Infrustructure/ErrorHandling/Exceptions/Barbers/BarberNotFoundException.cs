namespace Infrustructure.ErrorHandling.Exceptions.Barbers;

public class BarberNotFoundException : Exception
{
    public BarberNotFoundException(int id) 
        : base($"barber with id {id} was not found") {}
}