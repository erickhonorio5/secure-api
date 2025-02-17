namespace Secure.API.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string email, string code);
}
