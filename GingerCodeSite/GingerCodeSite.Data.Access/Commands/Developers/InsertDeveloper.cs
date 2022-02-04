using GingerCodeSite.Data.Context;
using GingerCodeSite.Data.Domain;
using GingerCodeSite.Data.Internal.Interfacing;

namespace GingerCodeSite.Data.Access.Commands.Developers;

//todo: Remove this file! It's just to demonstrate what a context command might look like.

public sealed class InsertDeveloper : ContextCommand< Developer >
{
    public InsertDeveloper( GingerCodeSiteContext context )
        : base( context ) { }

    public override async Task< CommandResult > ExecuteAsync( Developer input , CancellationToken cancellationToken = default )
    {
        Context.Update( input );
        await Context.SaveChangesAsync( cancellationToken )
                     .ConfigureAwait( false );
        return new CommandSuccess();
    }
}
