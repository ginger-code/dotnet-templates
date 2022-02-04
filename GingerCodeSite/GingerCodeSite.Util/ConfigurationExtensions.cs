using Microsoft.Extensions.Configuration;

namespace GingerCodeSite.Util;

/// <summary>
///     Extensions for retrieving configuration information
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    ///     Retrieve the signing secret for JWTs
    /// </summary>
    /// <param name="configuration">The configuration object to read from</param>
    /// <returns>The JWT signing secret</returns>
    public static string GetJwtSecret( this IConfigurationRoot configuration )
        => configuration[ "Authentication:Jwt:Key" ];

    /// <summary>
    ///     Retrieve the issuer for JWTs
    /// </summary>
    /// <param name="configuration">The configuration object to read from</param>
    /// <returns>The JWT issuer</returns>
    public static string GetJwtIssuer( this IConfigurationRoot configuration )
        => configuration[ "Authentication:Jwt:Issuer" ];

    /// <summary>
    ///     Retrieve the audience for JWTs
    /// </summary>
    /// <param name="configuration">The configuration object to read from</param>
    /// <returns>The JWT audience</returns>
    public static string GetJwtAudience( this IConfigurationRoot configuration )
        => configuration[ "Authentication:Jwt:Audience" ];

    /// <summary>
    ///     Retrieves origins for which Cors will be enabled
    /// </summary>
    /// <param name="configuration">The configuration object to read from</param>
    /// <returns>An array </returns>
    public static string[] GetCorsOrigins( this IConfiguration configuration )
        => configuration[ "Cross-Origin:Origins" ]
            .Split( ";" );
}
