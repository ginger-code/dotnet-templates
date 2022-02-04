#nullable disable

using System.Text;
using GingerCodeSite.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace GingerCodeSite.Views.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterConfirmationModel : PageModel
{
    private readonly IEmailSender _sender;
    private readonly UserManager< GingerCodeSiteUser > _userManager;

    public bool DisplayConfirmAccountLink { get; set; }

    public string Email { get; set; }

    public string EmailConfirmationUrl { get; set; }

    public RegisterConfirmationModel( UserManager< GingerCodeSiteUser > userManager , IEmailSender sender )
    {
        _userManager = userManager;
        _sender      = sender;
    }

    public async Task< IActionResult > OnGetAsync( string email , string returnUrl = null )
    {
        if ( email == null )
        {
            return RedirectToPage( "/Index" );
        }

        returnUrl = returnUrl ?? Url.Content( "~/" );

        var user = await _userManager.FindByEmailAsync( email );
        if ( user == null )
        {
            return NotFound( $"Unable to load user with email '{email}'." );
        }

        Email = email;
        // Once you add a real email sender, you should remove this code that lets you confirm the account
        DisplayConfirmAccountLink = true;
        if ( DisplayConfirmAccountLink )
        {
            var userId = await _userManager.GetUserIdAsync( user );
            var code   = await _userManager.GenerateEmailConfirmationTokenAsync( user );
            code = WebEncoders.Base64UrlEncode( Encoding.UTF8.GetBytes( code ) );
            EmailConfirmationUrl = Url.Page(
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
        }

        return Page();
    }
}
