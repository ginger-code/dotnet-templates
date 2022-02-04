namespace GingerCodeSite.Data.Internal.Interfacing;

/// <summary>
///     Describes a command with an input. Data target agnostic.
/// </summary>
/// <typeparam name="TInput">Type of the input parameter</typeparam>
public interface ICommand< in TInput >
{
    /// <summary>
    ///     Executes the command, using the supplied parameter
    /// </summary>
    /// <param name="input">The input parameter with which to execute the command</param>
    /// <param name="cancellationToken">A token for task cancellation</param>
    /// <returns>A task without a result</returns>
    public Task< CommandResult > ExecuteAsync( TInput input , CancellationToken cancellationToken = default );
}
