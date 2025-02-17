using Secure.API.Interfaces;

namespace Secure.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register/{email}/{password}")]
    public async Task<IActionResult> Register(string email, string password)
    {
        var result = await _accountService.RegisterAsync(email, password);
        if (!result.IsSuccess)
            return BadRequest(result.Errors);
        return Ok(result.Message);
    }

    [HttpPost("confirmation/{email}/{code:int}")]
    public async Task<IActionResult> ConfirmEmail(string email, int code)
    {
        var result = await _accountService.ConfirmEmailAsync(email, code);
        if (!result.IsSuccess)
            return BadRequest(result.Errors);
        return Ok(result.Message);
    }

    [HttpPost("login/{email}/{password}")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var result = await _accountService.LoginAsync(email, password);
        if (!result.IsSuccess)
            return BadRequest(result.Errors);
        return Ok(result.Data);
    }
}
