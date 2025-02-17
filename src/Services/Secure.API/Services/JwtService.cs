using Secure.API.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace Secure.API.Services;

public class JwtService : IJwtService
{
    private const string SecretKey = "A1B2C3D4E5F67890AABBCCDDEEFF1122"; 
    private const int ExpirationMinutes = 30;

    public string GenerateToken(IdentityUser user)
    {
        var key = Encoding.ASCII.GetBytes(SecretKey);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user!.Id),
            new Claim(JwtRegisteredClaimNames.Email, user!.Email!)
        };

        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.Now.AddMinutes(ExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
