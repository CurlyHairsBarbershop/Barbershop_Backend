namespace Infrustructure.ErrorHandling.Exceptions.Media;

public class MediaNotFoundException : Exception
{
    public MediaNotFoundException(string path) 
        : base($"media {path} was not found") { }
}