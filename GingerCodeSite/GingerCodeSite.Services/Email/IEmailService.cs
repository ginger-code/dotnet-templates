using Microsoft.AspNetCore.Identity.UI.Services;

namespace GingerCodeSite.Services.Email;

public interface IEmailService : ITransientService , IEmailSender
{
    Task SendEmailAsync(
        string            recipient ,
        string            subject ,
        string            body ,
        CancellationToken cancellationToken
    );
}
