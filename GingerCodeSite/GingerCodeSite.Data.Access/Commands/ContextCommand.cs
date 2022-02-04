using GingerCodeSite.Data.Context;
using GingerCodeSite.Data.Internal.Interfacing;

namespace GingerCodeSite.Data.Access.Commands;

/// <summary>
///     Base class for queries which utilize access to a database context
/// </summary>
/// <typeparam name="TInput">The type of the input parameter for the command</typeparam>
public abstract class ContextCommand< TInput > : ICommand< TInput >
{
    protected readonly GingerCodeSiteContext Context;

    /// <summary>
    ///     Instantiates a new command
    /// </summary>
    /// <param name="context">The database context to use</param>
    protected ContextCommand( GingerCodeSiteContext context )
    {
        Context = context;
    }

    public abstract Task< CommandResult > ExecuteAsync( TInput input , CancellationToken cancellationToken = default );
}
