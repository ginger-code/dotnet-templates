namespace GingerCodeSite.Data.Access.Queries.Users;

public class GenerateTokenResponse
{
    public string Token { get; init; }

    public GenerateTokenResponse( string token )
    {
        Token = token;
    }
}
