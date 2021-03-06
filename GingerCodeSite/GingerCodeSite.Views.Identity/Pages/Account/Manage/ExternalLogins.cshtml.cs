#nullable disable

using GingerCodeSite.Data.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GingerCodeSite.Views.Identity.Pages.Account.Manage;

public class ExternalLoginsModel : PageModel
{
    private readonly SignInManager< GingerCodeSiteUser > _signInManager;
    private readonly UserManager< GingerCodeSiteUser > _userManager;
    private readonly IUserStore< GingerCodeSiteUser > _userStore;

    public IList< UserLoginInfo > CurrentLogins { get; set; }

    public IList< AuthenticationScheme > OtherLogins { get; set; }

    public bool ShowRemoveButton { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    public ExternalLoginsModel(
        UserManager< GingerCodeSiteUser >   userManager ,
        SignInManager< GingerCodeSiteUser > signInManager ,
        IUserStore< GingerCodeSiteUser >    userStore
    )
    {
        _userManager   = userManager;
        _signInManager = signInManager;
        _userStore     = userStore;
    }

    public async Task< IActionResult > OnGetAsync()
    {
        var user = await _userManager.GetUserAsync( User );
        if ( user == null )
        {
            return NotFound( $"Unable to load user with ID '{_userManager.GetUserId( User )}'." );
        }

        CurrentLogins = await _userManager.GetLoginsAsync( user );
        OtherLogins = ( await _signInManager.GetExternalAuthenticationSchemesAsync() ).Where( auth => CurrentLogins.All( ul => auth.Name != ul.LoginProvider ) )
                                                                                      .ToList();

        string passwordHash = null;
        if ( _userStore is IUserPasswordStore< GingerCodeSiteUser > userPasswordStore )
        {
            passwordHash = await userPasswordStore.GetPasswordHashAsync( user , HttpContext.RequestAborted );
        }

        ShowRemoveButton = passwordHash != null || CurrentLogins.Count > 1;
        return Page();
    }

    public async Task< IActionResult > OnPostRemoveLoginAsync( string loginProvider , string providerKey )
    {
        var user = await _userManager.GetUserAsync( User );
        if ( user == null )
        {
            return NotFound( $"Unable to load user with ID '{_userManager.GetUserId( User )}'." );
        }

        var result = await _userManager.RemoveLoginAsync(
                         user ,
                         loginProvider ,
                         providerKey
                     );
        if ( !result.Succeeded )
        {
            StatusMessage = "The external login was not removed.";
            return RedirectToPage();
        }

        await _signInManager.RefreshSignInAsync( user );
        StatusMessage = "The external login was removed.";
        return RedirectToPage();
    }

    public async Task< IActionResult > OnPostLinkLoginAsync( string provider )
    {
        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync( IdentityConstants.ExternalScheme );

        // Request a redirect to the external login provider to link a login for the current user
        var redirectUrl = Url.Page( "/Account/Manage/ExternalLogins" , "LinkLoginCallback" );
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(
            provider ,
            redirectUrl ,
            _userManager.GetUserId( User )
        );
        return new ChallengeResult( provider , properties );
    }

    public async Task< IActionResult > OnGetLinkLoginCallbackAsync()
    {
        var user = await _userManager.GetUserAsync( User );
        if ( user == null )
        {
            return NotFound( $"Unable to load user with ID '{_userManager.GetUserId( User )}'." );
        }

        var userId = await _userManager.GetUserIdAsync( user );
        var info   = await _signInManager.GetExternalLoginInfoAsync( userId );
        if ( info == null )
        {
            throw new InvalidOperationException( "Unexpected error occurred loading external login info." );
        }

        var result = await _userManager.AddLoginAsync( user , info );
        if ( !result.Succeeded )
        {
            StatusMessage = "The external login was not added. External logins can only be associated with one account.";
            return RedirectToPage();
        }

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync( IdentityConstants.ExternalScheme );

        StatusMessage = "The external login was added.";
        return RedirectToPage();
    }
}
