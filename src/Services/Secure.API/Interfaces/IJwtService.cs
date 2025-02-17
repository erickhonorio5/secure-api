namespace Secure.API.Interfaces;

public interface IJwtService
{
    string GenerateToken(IdentityUser user);
}
