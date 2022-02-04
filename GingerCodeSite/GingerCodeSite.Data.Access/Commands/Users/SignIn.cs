using GingerCodeSite.Data.Access.Queries.Users;
using GingerCodeSite.Data.Context;
using GingerCodeSite.Data.Identity;
using GingerCodeSite.Data.Internal.Interfacing;
using Microsoft.AspNetCore.Identity;

namespace GingerCodeSite.Data.Access.Commands.Users;

/// <summary>
///     Signs in a user
/// </summary>
public sealed class SignIn : ContextCommand< SignInRequest >
{
    private readonly RetrieveUser _retrieveUser;
    private readonly SignInManager< GingerCodeSiteUser > _signinManager;

    public SignIn( GingerCodeSiteContext context , SignInManager< GingerCodeSiteUser > signinManager , RetrieveUser retrieveUser )
        : base( context )
    {
        _signinManager = signinManager;
        _retrieveUser  = retrieveUser;
    }

    public override async Task< CommandResult > ExecuteAsync( SignInRequest input , CancellationToken cancellationToken = default )
    {
        var user = await _retrieveUser.ExecuteAsync( new RetrieveUserRequest( input.Username ) , cancellationToken );
        var success = await _signinManager.CheckPasswordSignInAsync(
                          user ,
                          input.Password ,
                          true
                      );
        return success.Succeeded ? new CommandSuccess() : new CommandFailure( "Failed to sign in" );
    }
}
