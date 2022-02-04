#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using GingerCodeSite.Data.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace GingerCodeSite.Views.Identity.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly IEmailSender _emailSender;
    private readonly IUserEmailStore< GingerCodeSiteUser > _emailStore;
    private readonly ILogger< RegisterModel > _logger;
    private readonly SignInManager< GingerCodeSiteUser > _signInManager;
    private readonly UserManager< GingerCodeSiteUser > _userManager;
    private readonly IUserStore< GingerCodeSiteUser > _userStore;

    public IList< AuthenticationScheme > ExternalLogins { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }

    public RegisterModel(
        UserManager< GingerCodeSiteUser >   userManager ,
        IUserStore< GingerCodeSiteUser >    userStore ,
        SignInManager< GingerCodeSiteUser > signInManager ,
        ILogger< RegisterModel >            logger ,
        IEmailSender                        emailSender
    )
    {
        _userManager   = userManager;
        _userStore     = userStore;
        _emailStore    = GetEmailStore();
        _signInManager = signInManager;
        _logger        = logger;
        _emailSender   = emailSender;
    }

    public async Task OnGetAsync( string returnUrl = null )
    {
        ReturnUrl      = returnUrl;
        ExternalLogins = ( await _signInManager.GetExternalAuthenticationSchemesAsync() ).ToList();
    }

    public async Task< IActionResult > OnPostAsync( string returnUrl = null )
    {
        returnUrl      ??= Url.Content( "~/" );
        ExternalLogins =   ( await _signInManager.GetExternalAuthenticationSchemesAsync() ).ToList();
        if ( ModelState.IsValid )
        {
            var user = CreateUser();

            await _userStore.SetUserNameAsync(
                user ,
                Input.Email ,
                CancellationToken.None
            );
            await _emailStore.SetEmailAsync(
                user ,
                Input.Email ,
                CancellationToken.None
            );
            var result = await _userManager.CreateAsync( user , Input.Password );

            if ( result.Succeeded )
            {
                _logger.LogInformation( "User created a new account with password" );

                var userId = await _userManager.GetUserIdAsync( user );
                var code   = await _userManager.GenerateEmailConfirmationTokenAsync( user );
                code = WebEncoders.Base64UrlEncode( Encoding.UTF8.GetBytes( code ) );
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail" ,
                    null ,
                    new
                    {
                        userId ,
                        code ,
                        returnUrl ,
                    } ,
                    Request.Scheme
                );

                await _emailSender.SendEmailAsync(
                    Input.Email ,
                    "Confirm your email" ,
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode( callbackUrl )}'>clicking here</a>."
                );

                if ( _userManager.Options.SignIn.RequireConfirmedAccount )
                {
                    return RedirectToPage(
                        "./RegisterConfirmation" ,
                        new
                        {
                            email = Input.Email ,
                            returnUrl ,
                        }
                    );
                }

                await _signInManager.SignInAsync( user , false );
                return LocalRedirect( returnUrl );
            }

            foreach ( var error in result.Errors )
                ModelState.AddModelError( string.Empty , error.Description );
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }

    private GingerCodeSiteUser CreateUser()
        => new();

    private IUserEmailStore< GingerCodeSiteUser > GetEmailStore()
    {
        return (IUserEmailStore< GingerCodeSiteUser >) _userStore;
    }

    public class InputModel
    {
        [DataType( DataType.Password )]
        [Display( Name = "Confirm password" )]
        [Compare( "Password" , ErrorMessage = "The password and confirmation password do not match." )]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [Display( Name = "Email" )]
        public string Email { get; set; }

        [Required]
        [StringLength(
            100 ,
            ErrorMessage = "The {0} must be at least {2} and at max {1} characters long." ,
            MinimumLength = 6
        )]
        [DataType( DataType.Password )]
        [Display( Name = "Password" )]
        public string Password { get; set; }
    }
}
