using GingerCodeSite.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GingerCodeSite.Startup.Data.Context;

/// <summary>
///     Extensions for configuring the GingerCodeSite.Data.Context assembly
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    ///     Adds the GingerCodeSite DbContext to the service collection, using the specified provider (Defaults to In-Memory persistence)
    /// </summary>
    /// <param name="services">The service collection to which the DbContext should be added</param>
    /// <param name="connectionString">The connection string to use for the DbContext</param>
    /// <param name="provider">The type of SQL provider that should be used</param>
    /// <returns>The same service collection for chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IServiceCollection AddDataContext( this IServiceCollection services , string connectionString , SqlProvider provider = SqlProvider.InMemory )
    {
        return provider switch
               {
                   SqlProvider.Sqlite    => services.AddSqlite( connectionString ) ,
                   SqlProvider.InMemory  => services.AddInMemory( connectionString ) ,
                   SqlProvider.SqlServer => services.AddSqlServer( connectionString ) ,
                   SqlProvider.NpgSql    => services.AddNpgsql( connectionString ) ,
                   _ => throw new ArgumentOutOfRangeException(
                            nameof( provider ) ,
                            provider ,
                            "Specified SQL provider could not be configured"
                        ) ,
               };
    }

    private static IServiceCollection AddSqlite( this IServiceCollection services , string connectionString )
        => services.AddSqlite< GingerCodeSiteContext >( connectionString );

    private static IServiceCollection AddSqlServer( this IServiceCollection services , string connectionString )
        => services.AddSqlServer< GingerCodeSiteContext >( connectionString );

    private static IServiceCollection AddNpgsql( this IServiceCollection services , string connectionString )
    {
        return services.AddDbContext< GingerCodeSiteContext >( opt => opt.UseNpgsql( connectionString ) );
    }

    private static IServiceCollection AddInMemory( this IServiceCollection services , string connectionString )
    {
        return services.AddDbContext< GingerCodeSiteContext >( opt => opt.UseInMemoryDatabase( connectionString ) );
    }
}
