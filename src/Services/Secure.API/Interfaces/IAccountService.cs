using Secure.API.Models;

namespace Secure.API.Interfaces;

public interface IAccountService
{
    Task<Result> RegisterAsync(string email, string password);
    Task<Result> ConfirmEmailAsync(string email, int code);
    Task<Result> LoginAsync(string email, string password);
}
