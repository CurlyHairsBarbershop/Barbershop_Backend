using Microsoft.Extensions.Options;

namespace API.Models.Options;

public class JwtProviderOptions
{
    public string Key { get; set; }
    
    public string ValidIssuer { get; set; }
    
    public string ValidAudience { get; set; }
}