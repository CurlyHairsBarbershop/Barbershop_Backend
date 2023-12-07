namespace Infrustructure.ErrorHandling.Exceptions.Favors;

public class FavorNotFoundException : Exception
{
    public FavorNotFoundException(int id)
        : base($"Favor with id {id} was not found") { }
}