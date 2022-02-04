using GingerCodeSite.Data.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GingerCodeSite.Startup.Data.Services;

/// <summary>
///     Extensions for configuring the GingerCodeSite.Data.Service assembly
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    ///     Adds all data services to the service collection
    /// </summary>
    /// <param name="services">The service collection to which data services should be added</param>
    /// <returns>The same service collection for chaining</returns>
    public static IServiceCollection AddDataServices( this IServiceCollection services )
    {
        return services.Scan(
            scan => scan.FromAssemblyDependencies( typeof( ConfigurationExtensions ).Assembly )
                        .AddClasses( type => type.AssignableTo< IDataService >() )
                        .AsSelfWithInterfaces()
                        .WithTransientLifetime()
        );
    }
}
