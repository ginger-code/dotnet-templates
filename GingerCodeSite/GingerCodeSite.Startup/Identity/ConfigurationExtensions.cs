using GingerCodeSite.Data.Context;
using GingerCodeSite.Data.Identity;
using GingerCodeSite.Util;
using JWT.Algorithms;
using JWT.Extensions.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GingerCodeSite.Startup.Identity;

/// <summary>
///     Extensions for configuring/integrating Microsoft Identity
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    ///     Configures and adds Microsoft Identity to the service collection
    /// </summary>
    /// <param name="services">The service collection to which identity services should be added</param>
    /// <param name="configuration">Configuration to read</param>
    /// <returns>And <see cref="IdentityBuilder" /> for chaining</returns>
    public static IdentityBuilder UseIdentity( this IServiceCollection services , IConfigurationRoot configuration )
    {
        services.ConfigureJwtAuthentication( configuration );
        return services.AddIdentity< GingerCodeSiteUser , GingerCodeSiteRole >( Configure )
                       .AddDefaultTokenProviders()
                       .AddEntityFrameworkStores< GingerCodeSiteContext >();
    }

    /// <summary>
    ///     Configures an application to use authentication and authorization
    /// </summary>
    /// <param name="app">The application builder to configure</param>
    /// <returns>The same application builder for chaining</returns>
    public static IApplicationBuilder UseAuth( this IApplicationBuilder app )
        => app.UseAuthentication()
              .UseAuthorization();

    #region Configuration Helpers

    private static void ConfigureJwtAuthentication( this IServiceCollection services , IConfigurationRoot configuration )
    {
        services.AddAuthentication(
                    options =>
                    {
                        options.DefaultAuthenticateScheme = JwtAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme    = JwtAuthenticationDefaults.AuthenticationScheme;
                    }
                )
                .UseGoogleAuth( configuration )
                .UseMicrosoftAuth( configuration )
                .AddJwt(
                    options =>
                    {
                        options.ClaimsIssuer    = configuration.GetJwtIssuer();
                        options.Keys            = new[] { configuration.GetJwtSecret() , };
                        options.VerifySignature = true;
                    }
                );
        services.AddSingleton< IAlgorithmFactory >( new DelegateAlgorithmFactory( new HMACSHA256Algorithm() ) );
    }

    private static AuthenticationBuilder UseGoogleAuth( this AuthenticationBuilder auth , IConfigurationRoot configuration )
    {
        var googleConfig = configuration.GetSection( "Authentication:Google" );
        var enableGoogle = configuration.GetValue< bool >( "Enable" );
        if ( !enableGoogle )
        {
            return auth;
        }

        return auth.AddGoogle(
            options =>
            {
                options.ClientId     = googleConfig[ "ClientId" ];
                options.ClientSecret = googleConfig[ "ClientSecret" ];
                options.CallbackPath = "/signin-google";
            }
        );
    }

    private static AuthenticationBuilder UseMicrosoftAuth( this AuthenticationBuilder auth , IConfigurationRoot configuration )
    {
        var microsoftConfig = configuration.GetSection( "Authentication:Microsoft" );
        var enableMicrosoft = configuration.GetValue< bool >( "Enable" );
        if ( !enableMicrosoft )
        {
            return auth;
        }

        return auth.AddMicrosoftAccount(
            options =>
            {
                options.ClientId     = microsoftConfig[ "ClientId" ];
                options.ClientSecret = microsoftConfig[ "ClientSecret" ];
                options.CallbackPath = "/signin-microsoft";
            }
        );
    }

    private static void Configure( this IdentityOptions options )
    {
        options.Password.Configure();
        options.Stores.Configure();
        options.SignIn.Configure();
        options.Lockout.Configure();
        options.Tokens.Configure();
        options.ClaimsIdentity.Configure();
        options.User.Configure();
    }

    private static void Configure( this PasswordOptions options )
    {
        options.RequireLowercase       = true;
        options.RequireUppercase       = true;
        options.RequiredLength         = 12;
        options.RequiredUniqueChars    = 7;
        options.RequireNonAlphanumeric = false;
        options.RequireDigit           = false;
    }

    private static void Configure( this StoreOptions options )
    {
        options.MaxLengthForKeys    = 128;
        options.ProtectPersonalData = false;
    }

    private static void Configure( this SignInOptions options )
    {
        options.RequireConfirmedAccount = true;
    }

    private static void Configure( this LockoutOptions options )
    {
        options.MaxFailedAccessAttempts = 5;
    }

    private static void Configure( this TokenOptions options ) { }

    private static void Configure( this ClaimsIdentityOptions options ) { }

    private static void Configure( this UserOptions options )
    {
        options.RequireUniqueEmail = true;
    }

    #endregion
}
