using System.ComponentModel.DataAnnotations;

namespace GingerCodeSite.Data.Access.Queries.Users;

public class GenerateTokenRequest
{
    [Required( ErrorMessage = "Username must be specified to generate a JWT" )]
    public string Username { get; init; }

    public GenerateTokenRequest( string username )
    {
        Username = username;
    }
}
