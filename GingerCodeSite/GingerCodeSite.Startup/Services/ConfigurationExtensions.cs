using GingerCodeSite.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GingerCodeSite.Startup.Services;

/// <summary>
///     Extensions for configuring the GingerCodeSite.Service assembly
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    ///     Adds all GingerCodeSite services to the service collection
    /// </summary>
    /// <param name="services">The service collection to which services should be added</param>
    /// <returns>The same service collection for chaining</returns>
    public static IServiceCollection AddServices( this IServiceCollection services )
    {
        return services.Scan(
            scan => scan.FromAssemblyDependencies( typeof( ConfigurationExtensions ).Assembly )
                        .AddClasses( type => type.AssignableTo< ITransientService >() )
                        .AsSelfWithInterfaces()
                        .WithTransientLifetime()
                        .AddClasses( type => type.AssignableTo< IScopedService >() )
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime()
                        .AddClasses( type => type.AssignableTo< ISingletonService >() )
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime()
        );
    }
}
