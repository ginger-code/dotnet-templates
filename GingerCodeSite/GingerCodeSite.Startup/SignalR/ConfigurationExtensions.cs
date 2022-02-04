using GingerCodeSite.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace GingerCodeSite.Startup.SignalR;

/// <summary>
///     Extensions for configuring the GingerCodeSite.SignalR assembly
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    ///     Maps all defined SignalR hubs
    /// </summary>
    /// <param name="endpoints">The endpoint route builder to use for mapping</param>
    /// <returns>The same endpoint route builder for chaining</returns>
    public static IEndpointRouteBuilder MapHubs( this IEndpointRouteBuilder endpoints )
    {
        endpoints.MapHub< ChatHub >( "/hub/chat" );
        return endpoints;
    }
}
