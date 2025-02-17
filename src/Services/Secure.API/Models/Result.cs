namespace Secure.API.Models;

public class Result
{
    public bool IsSuccess { get; set; }
    public string[]? Errors { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }
}
