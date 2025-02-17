using Secure.API.Interfaces;
using Secure.API.Models;

namespace Secure.API.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IJwtService _jwtService;

    public AccountService(UserManager<IdentityUser> userManager, IEmailService emailService, IJwtService jwtService)
    {
        _userManager = userManager;
        _emailService = emailService;
        _jwtService = jwtService;
    }

    public async Task<Result> RegisterAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user != null)
            return new Result { IsSuccess = false, Errors = ["Usuário já existe"] };

        var identityUser = new IdentityUser() 
        { 
            UserName = email, 
            Email = email
        };

        var result = await _userManager.CreateAsync(identityUser, password);

        if (!result.Succeeded)
            return new Result { IsSuccess = false, Errors = result.Errors.Select(e => e.Description).ToArray() };

        var emailCode = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
        await _emailService.SendEmailAsync(identityUser.Email, emailCode);

        return new Result { IsSuccess = true, Message = "Registro realizado com sucesso! Confirme seu e-mail antes de realizar o login." };
    }

    public async Task<Result> ConfirmEmailAsync(string email, int code)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return new Result { IsSuccess = false, Errors = new[] { "Usuário não encontrado" } };

        var result = await _userManager.ConfirmEmailAsync(user, code.ToString());

        if (!result.Succeeded)
            return new Result { IsSuccess = false, Errors = result.Errors.Select(e => e.Description).ToArray() };

        return new Result { IsSuccess = true, Message = "E-mail confirmado com sucesso!" };
    }

    public async Task<Result> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            return new Result { IsSuccess = false, Errors = new[] { "E-mail ou senha inválidos" } };

        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

        if (!isEmailConfirmed)
            return new Result { IsSuccess = false, Errors = new[] { "E-mail não confirmado" } };

        var token = _jwtService.GenerateToken(user);

        return new Result { IsSuccess = true, Message = "Login realizado com sucesso!", Data = new { Token = token } };
    }
}
