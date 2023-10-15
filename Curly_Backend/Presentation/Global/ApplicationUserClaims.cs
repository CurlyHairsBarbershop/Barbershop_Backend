using System.Security.Claims;

namespace Presentation.Global;

public class ApplicationUserClaims
{
    public static List<Claim> BarberClaims = new()
    {
        new("MemberType", "Barber")
    };

    public static List<Claim> ClientClaims = new()
    {
        new Claim("MemberType", "Client")
    };
}