using GingerCodeSite.Data.Internal.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GingerCodeSite.Startup.Email;

public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigureEmail( this IServiceCollection services , IConfigurationRoot configuration )
    {
        var email = EmailConfiguration.Bind( configuration );
        services.AddSingleton( provider => EmailConfiguration.Bind( provider.GetService< IConfigurationRoot >()! ) );
        services.AddFluentEmail( email.Mail , email.DisplayName )
                .AddSmtpSender(
                    email.Smtp.Hostname ,
                    email.Smtp.Port ,
                    email.Smtp.Username ,
                    email.Smtp.Password
                );
        return services;
    }
}
