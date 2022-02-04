using GingerCodeSite.Data.Access.Queries.Users;
using GingerCodeSite.Data.Identity;
using Microsoft.Extensions.Logging;

namespace GingerCodeSite.Data.Services.Users;

/// <summary>
///     A service for querying against site users
/// </summary>
public class UserQueries : IDataService
{
    private readonly GenerateToken _generateToken;
    private readonly ILogger< UserQueries > _logger;
    private readonly RetrieveUser _retrieveUser;

    public UserQueries( ILogger< UserQueries > logger , RetrieveUser retrieveUser , GenerateToken generateToken )
    {
        _logger        = logger;
        _retrieveUser  = retrieveUser;
        _generateToken = generateToken;
    }

    /// <summary>
    ///     Retrieves a user by their username, throwing an exception if the user does not exist
    /// </summary>
    /// <param name="request">The request, containing the username to locate</param>
    /// <returns>A task containing a user</returns>
    public async Task< GingerCodeSiteUser > RetrieveUserAsync( RetrieveUserRequest request )
        => await _retrieveUser.ExecuteAsync( request );

    /// <summary>
    ///     Generates a new JWT for a user, given their username
    /// </summary>
    /// <param name="request">The request, containing the username</param>
    /// <returns>A task containing the generated token</returns>
    public async Task< GenerateTokenResponse > GenerateToken( GenerateTokenRequest request )
        => await _generateToken.ExecuteAsync( request );
}
