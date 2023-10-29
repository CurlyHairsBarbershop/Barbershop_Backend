using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core;
using Microsoft.IdentityModel.Tokens;
using Presentation.Models.Options;

namespace Presentation.Services.JwtProvider;

public class JwtProvider<TUser> : IJwtProvider<TUser> 
    where TUser : ApplicationUser
{
    private readonly string _audience;
    private readonly string _issuer;
    private readonly string _signingKey;
    
    public JwtProvider(JwtProviderOptions options)
    {
        _audience = options.ValidAudience;
        _issuer = options.ValidIssuer;
        _signingKey = options.Key;
    }
    
    public string GenerateJwtTokenString(TUser user, params string[] roles)
    {
        var handler = new JwtSecurityTokenHandler();

        var tokenString = handler.WriteToken(GenerateMemberToken(user, roles));

        return tokenString;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var symmetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_signingKey));
        var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

        return signingCredentials;
    }

    private JwtSecurityToken GenerateToken(List<Claim> claims)
    {
        var credentials = GetSigningCredentials();
        
        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            issuer: _issuer,
            audience: _audience,
            signingCredentials: credentials);

        return securityToken;
    }
    
    private JwtSecurityToken GenerateMemberToken(TUser member, string[] roles)
    {
        List<Claim> claims = new()
        {
            new("Email", member.Email),
            new("FirstName", member.FirstName),
            new("LastName", member.LastName)
        };
        
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        
        var token = GenerateToken(claims);

        return token;
    }
}