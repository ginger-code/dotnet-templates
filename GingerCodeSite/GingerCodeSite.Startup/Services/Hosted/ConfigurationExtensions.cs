using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GingerCodeSite.Startup.Services.Hosted;

/// <summary>
///     Extensions for configuring the GingerCodeSite.Service.Hosted assembly
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    ///     Adds all hosted/background services to the service collection
    /// </summary>
    /// <param name="services">The service collection to which services should be added</param>
    /// <returns>The same service collection for chaining</returns>
    public static IServiceCollection AddHostedServices( this IServiceCollection services )
    {
        return services.Scan(
            scan => scan.FromAssemblyDependencies( typeof( ConfigurationExtensions ).Assembly )
                        .AddClasses( type => type.AssignableTo< IHostedService >() )
                        .AsSelfWithInterfaces()
                        .WithSingletonLifetime()
        );
    }
}
