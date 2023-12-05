namespace Infrustructure.Extensions;

public static class GeneralHelperExtensions
{
    public static bool IsValidBase64(this string source, out byte[] bytes)
    {
        try
        {
            bytes = Convert.FromBase64String(source);

            return true;
        }
        catch (FormatException ex)
        {
            bytes = Array.Empty<byte>();
            
            return false;
        }
    }
}