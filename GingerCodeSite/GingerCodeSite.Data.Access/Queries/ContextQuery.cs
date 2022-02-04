using GingerCodeSite.Data.Context;
using GingerCodeSite.Data.Internal.Interfacing;

namespace GingerCodeSite.Data.Access.Queries;

/// <summary>
///     Base class for queries which utilize access to a database context
/// </summary>
/// <typeparam name="TInput">The type of the input parameter for the query</typeparam>
/// <typeparam name="TResult">The type returned by the query</typeparam>
public abstract class ContextQuery< TInput , TResult > : IQuery< TInput , TResult >
{
    protected readonly GingerCodeSiteContext Context;

    /// <summary>
    ///     Instantiates a new query
    /// </summary>
    /// <param name="context">The database context to use</param>
    protected ContextQuery( GingerCodeSiteContext context )
    {
        Context = context;
    }

    public abstract Task< TResult > ExecuteAsync( TInput input , CancellationToken cancellationToken = default );
}
