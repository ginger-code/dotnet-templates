using System.ComponentModel.DataAnnotations;

namespace GingerCodeSite.Data.Access.Commands.Users;

public sealed class SignInRequest
{
    [Required( ErrorMessage = "Password must be specified to login" )]
    public string Password { get; set; } = null!;

    [Required( ErrorMessage = "Username must be specified to login" )]
    public string Username { get; set; } = null!;
}
