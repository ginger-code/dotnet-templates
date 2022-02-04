using Microsoft.Extensions.Logging;

namespace GingerCodeSite.Startup.Logging;

/// <summary>
///     Extensions for Microsoft.Extensions.Logging
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    ///     Configures logging options
    /// </summary>
    /// <param name="logging">The logging builder to configure</param>
    /// <returns>The same logging builder for chaining</returns>
    public static ILoggingBuilder Configure( this ILoggingBuilder logging )
    {
        logging.ClearProviders();
        logging.AddSimpleConsole();
        return logging;
    }
}
