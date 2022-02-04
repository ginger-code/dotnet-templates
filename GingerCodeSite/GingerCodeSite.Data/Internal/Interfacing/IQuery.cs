namespace GingerCodeSite.Data.Internal.Interfacing;

/// <summary>
///     Describes a query with an input and output. Data source agnostic.
/// </summary>
/// <typeparam name="TInput">Type of the input parameter</typeparam>
/// <typeparam name="TResult">Type of the result</typeparam>
public interface IQuery< in TInput , TResult >
{
    /// <summary>
    ///     Executes the query using the supplied parameter, and returns the result
    /// </summary>
    /// <param name="input">The input parameter with which to execute the query</param>
    /// <param name="cancellationToken">A token for task cancellation</param>
    /// <returns>A task with a result</returns>
    public Task< TResult > ExecuteAsync( TInput input , CancellationToken cancellationToken = default );
}
