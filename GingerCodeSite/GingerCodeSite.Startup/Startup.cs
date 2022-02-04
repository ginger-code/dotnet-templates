using GingerCodeSite.Data.Identity;
using GingerCodeSite.Services;
using GingerCodeSite.Startup.Data.Access;
using GingerCodeSite.Startup.Data.Context;
using GingerCodeSite.Startup.Data.Services;
using GingerCodeSite.Startup.Email;
using GingerCodeSite.Startup.Identity;
using GingerCodeSite.Startup.Logging;
using GingerCodeSite.Startup.Services;
using GingerCodeSite.Startup.Services.Hosted;
using GingerCodeSite.Startup.SignalR;
using GingerCodeSite.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace GingerCodeSite.Startup;

/// <summary>
///     A simple collection of methods for building and configuring a GingerCodeSite instance
/// </summary>
public static class Startup
{
    /// <summary>
    ///     Creates and configures a web application using the methods defined within Startup
    /// </summary>
    /// <param name="args">An array of command-line args passed to the program</param>
    /// <returns>A configured web application for execution</returns>
    public static WebApplication CreateWebApplication( string[] args )
        => WebApplication.CreateBuilder( args )
                         .ConfigureBuilder()
                         .Build()
                         .ConfigureApp();

    /// <summary>
    ///     Configures services and logging before building the web application
    /// </summary>
    /// <param name="builder">The web application builder to configure</param>
    /// <returns>The same web application builder for chaining</returns>
    private static WebApplicationBuilder ConfigureBuilder( this WebApplicationBuilder builder )
    {
        var (services , environment , configuration , logging) = ( builder.Services , builder.Environment , builder.Configuration , builder.Logging );
        configuration.AddJsonFile( "appsettings.json" , false )
                     .AddJsonFile( "appsettings.json" );
        var connectionString = configuration.GetConnectionString( "GingerCodeSite" );
        services.AddTransient< IConfigurationRoot , ConfigurationManager >( _ => configuration );
        services.AddDataContext( connectionString )
                .AddDatabaseDeveloperPageExceptionFilter()
                .AddContextQueries()
                .AddContextCommands()
                .AddDataServices()
                .AddServices()
                .AddHostedServices()
                .ConfigureEmail( configuration )
                .AddCors( options => ConfigureCors( configuration , options ) )
                .AddSignalR();
        services.AddControllers();
        services.ConfigureSwagger( environment );
        services.AddRazorPages();
        services.AddHttpClient();
        services.AddScoped< AuthenticationStateProvider , RevalidatingIdentityAuthenticationStateProvider< GingerCodeSiteUser > >();
        services.AddServerSideBlazor();
        services.UseIdentity( configuration );
        logging.Configure();

        return builder;
    }

    /// <summary>
    ///     Configures a web application prior to execution
    /// </summary>
    /// <param name="app">The web application to configure</param>
    /// <returns>The same web application for execution</returns>
    private static WebApplication ConfigureApp( this WebApplication app )
    {
        if ( app.Environment.IsDevelopment() )
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI( options => { options.SwaggerEndpoint( "/swagger/v1/swagger.json" , "GingerCodeSite API" ); } );
        }
        else
        {
            app.UseExceptionHandler( "/Error" );
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        if ( app.Configuration.GetCorsOrigins() is { } origins )
        {
            app.UseCors( builder => builder.WithOrigins( origins ) );
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuth();
        app.UseEndpoints(
            endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapHubs();
                endpoints.MapBlazorHub();
            }
        );
        app.MapFallbackToPage( "/_Host" );

        return app;
    }

    private static void ConfigureCors( IConfiguration configuration , CorsOptions options )
    {
        options.AddPolicy( "Enable CORS origins" , builder => { builder.SetIsOriginAllowed( origin => IsOriginAllowed( configuration , origin ) ); } );

        //Helper method for checking origins against those defined in configuration
        static bool IsOriginAllowed( IConfiguration configuration , string origin )
            => configuration.GetCorsOrigins()
                            .Any( origin.StartsWith );
    }

    /// <summary>
    ///     Configures swagger documentation generation
    /// </summary>
    /// <param name="services">The service collection to modify</param>
    /// <param name="environment">The host environment being deployed to</param>
    private static void ConfigureSwagger( this IServiceCollection services , IHostEnvironment environment )
    {
        if ( !environment.IsDevelopment() )
        {
            return;
        }

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc(
                    "v1" ,
                    new OpenApiInfo
                    {
                        Version     = "v1" ,
                        Title       = "GingerCodeSite API" ,
                        Description = "API documentation for GingerCodeSite" ,
                    }
                );
                options.AddSecurityDefinition(
                    "Bearer" ,
                    new OpenApiSecurityScheme
                    {
                        Name         = "Authorization" ,
                        Type         = SecuritySchemeType.ApiKey ,
                        Scheme       = "Bearer" ,
                        BearerFormat = "JWT" ,
                        In           = ParameterLocation.Header ,
                        Description  = "JWT Authorization header using the Bearer scheme." ,
                    }
                );
                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                            {
                                                Type
                                                    = ReferenceType.SecurityScheme ,
                                                Id = "Bearer" ,
                                            } ,
                            } ,
                            Array.Empty< string >()
                        } ,
                    }
                );
            }
        );
    }
}
