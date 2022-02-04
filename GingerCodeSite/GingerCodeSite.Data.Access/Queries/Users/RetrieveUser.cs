using GingerCodeSite.Data.Context;
using GingerCodeSite.Data.Identity;
using Microsoft.EntityFrameworkCore;

namespace GingerCodeSite.Data.Access.Queries.Users;

/// <summary>
///     Retrieves a user, given a username
/// </summary>
public sealed class RetrieveUser : ContextQuery< RetrieveUserRequest , GingerCodeSiteUser >
{
    public RetrieveUser( GingerCodeSiteContext context )
        : base( context ) { }

    public override async Task< GingerCodeSiteUser > ExecuteAsync( RetrieveUserRequest input , CancellationToken cancellationToken = default )
    {
        return await Context.Users.SingleAsync( x => x.UserName == input.Username , cancellationToken )
                            .ConfigureAwait( false );
    }
}
