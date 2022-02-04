using Microsoft.Extensions.Configuration;

namespace GingerCodeSite.Data.Internal.Email;

public sealed class EmailConfiguration
{
    public string            DisplayName { get; set; } = null!;
    public string            Mail        { get; set; } = null!;
    public SmtpConfiguration Smtp        { get; set; } = new();

    public static EmailConfiguration Bind( IConfigurationRoot configurationRoot )
        => configurationRoot.GetSection( "Email" )
                            .Get< EmailConfiguration >();
}
