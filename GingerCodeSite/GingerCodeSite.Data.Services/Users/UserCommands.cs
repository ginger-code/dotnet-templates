using GingerCodeSite.Data.Access.Commands.Users;
using GingerCodeSite.Data.Internal.Interfacing;
using Microsoft.Extensions.Logging;

namespace GingerCodeSite.Data.Services.Users;

public class UserCommands : IDataService
{
    private readonly ILogger< UserCommands > _logger;
    private readonly SignIn _signIn;

    public UserCommands( ILogger< UserCommands > logger , SignIn signIn )
    {
        _logger = logger;
        _signIn = signIn;
    }

    public async Task< CommandResult > SignIn( SignInRequest request , CancellationToken cancellationToken = default )
        => await _signIn.ExecuteAsync( request , cancellationToken );
}
