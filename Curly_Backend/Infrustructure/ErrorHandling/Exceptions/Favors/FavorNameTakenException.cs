namespace Infrustructure.ErrorHandling.Exceptions.Favors;

public class FavorNameTakenException : Exception
{
    public FavorNameTakenException(string name) 
        : base($"Favor with name {name} already exists") { }
}