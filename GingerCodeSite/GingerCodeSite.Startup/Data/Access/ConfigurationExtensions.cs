using GingerCodeSite.Data.Access.Commands;
using GingerCodeSite.Data.Access.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace GingerCodeSite.Startup.Data.Access;

/// <summary>
///     Extensions for configuring the GingerCodeSite.Data.Access assembly
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    ///     Adds all queries utilizing a DbContext to the service collection
    /// </summary>
    /// <param name="services">The service collection to which queries should be added</param>
    /// <returns>The same service collection for chaining</returns>
    public static IServiceCollection AddContextQueries( this IServiceCollection services )
    {
        return services.Scan(
            scan => scan.FromAssemblyDependencies( typeof( ConfigurationExtensions ).Assembly )
                        .AddClasses( type => type.AssignableTo( typeof( ContextQuery< , > ) ) )
                        .AsSelfWithInterfaces()
                        .WithTransientLifetime()
        );
    }

    /// <summary>
    ///     Adds all commands utilizing a DbContext to the service collection
    /// </summary>
    /// <param name="services">The service collection to which commands should be added</param>
    /// <returns>The same service collection for chaining</returns>
    public static IServiceCollection AddContextCommands( this IServiceCollection services )
    {
        return services.Scan(
            scan => scan.FromAssemblyDependencies( typeof( ConfigurationExtensions ).Assembly )
                        .AddClasses( type => type.AssignableTo( typeof( ContextCommand<> ) ) )
                        .AsSelfWithInterfaces()
                        .WithTransientLifetime()
        );
    }
}
