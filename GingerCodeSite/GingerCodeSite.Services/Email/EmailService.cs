using GingerCodeSite.Data.Internal.Email;
using Microsoft.Extensions.Logging;

namespace GingerCodeSite.Services.Email;

public sealed class EmailService : IEmailService
{
    private readonly ILogger< EmailService > _logger;
    private readonly EmailConfiguration _emailConfiguration;

    public EmailService( ILogger< EmailService > logger , EmailConfiguration emailConfiguration )
    {
        _logger             = logger;
        _emailConfiguration = emailConfiguration;
    }

    public async Task SendEmailAsync(
        string            recipient ,
        string            subject ,
        string            body ,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var result = await FluentEmail.Core.Email.From( _emailConfiguration.Mail , _emailConfiguration.DisplayName )
                                          .To( recipient )
                                          .Subject( subject )
                                          .Body( body )
                                          .SendAsync( cancellationToken )
                                          .ConfigureAwait( false );
            if ( !result.Successful )
            {
                var error = string.Join( "; " , result.ErrorMessages );
                _logger.LogError(
                    "Failed to send email to {EmailAddress} with error(s): {Error}" ,
                    recipient ,
                    error
                );
            }
        }
        catch ( Exception exception )
        {
            const string errorMessage = "Unexpected error encountered while sending email";
            _logger.LogError( exception , errorMessage );
        }
    }

    public async Task SendEmailAsync( string email , string subject , string htmlMessage )
        => await SendEmailAsync(
                   email ,
                   subject ,
                   htmlMessage ,
                   CancellationToken.None
               )
               .ConfigureAwait( false );
}
