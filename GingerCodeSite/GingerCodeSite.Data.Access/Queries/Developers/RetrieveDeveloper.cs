using GingerCodeSite.Data.Context;
using GingerCodeSite.Data.Domain;

namespace GingerCodeSite.Data.Access.Queries.Developers;

//todo: Remove this file! It's just to demonstrate what a context query might look like.

public sealed class RetrieveDeveloper : ContextQuery< Guid , Developer? >
{
    public RetrieveDeveloper( GingerCodeSiteContext context )
        : base( context ) { }

    public override async Task< Developer? > ExecuteAsync( Guid developerId , CancellationToken cancellationToken = default )
    {
        return await Context.Developers.FindAsync( new object?[] { developerId , } , cancellationToken )
                            .ConfigureAwait( false );
    }
}
