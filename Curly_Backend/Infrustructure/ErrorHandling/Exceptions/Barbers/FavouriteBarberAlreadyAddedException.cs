namespace Infrustructure.ErrorHandling.Exceptions.Barbers;

public class FavouriteBarberAlreadyAddedException : Exception
{
    public FavouriteBarberAlreadyAddedException(int id) 
        : base($"Barber with id {id} has already been added to favourites") { }
}