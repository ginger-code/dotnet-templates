#nullable disable

using System.ComponentModel.DataAnnotations;
using GingerCodeSite.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GingerCodeSite.Views.Identity.Pages.Account.Manage;

public class SetPasswordModel : PageModel
{
    private readonly SignInManager< GingerCodeSiteUser > _signInManager;
    private readonly UserManager< GingerCodeSiteUser > _userManager;

    [BindProperty]
    public InputModel Input { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    public SetPasswordModel( UserManager< GingerCodeSiteUser > userManager , SignInManager< GingerCodeSiteUser > signInManager )
    {
        _userManager   = userManager;
        _signInManager = signInManager;
    }

    public async Task< IActionResult > OnGetAsync()
    {
        var user = await _userManager.GetUserAsync( User );
        if ( user == null )
        {
            return NotFound( $"Unable to load user with ID '{_userManager.GetUserId( User )}'." );
        }

        var hasPassword = await _userManager.HasPasswordAsync( user );

        if ( hasPassword )
        {
            return RedirectToPage( "./change-password" );
        }

        return Page();
    }

    public async Task< IActionResult > OnPostAsync()
    {
        if ( !ModelState.IsValid )
        {
            return Page();
        }

        var user = await _userManager.GetUserAsync( User );
        if ( user == null )
        {
            return NotFound( $"Unable to load user with ID '{_userManager.GetUserId( User )}'." );
        }

        var addPasswordResult = await _userManager.AddPasswordAsync( user , Input.NewPassword );
        if ( !addPasswordResult.Succeeded )
        {
            foreach ( var error in addPasswordResult.Errors )
                ModelState.AddModelError( string.Empty , error.Description );

            return Page();
        }

        await _signInManager.RefreshSignInAsync( user );
        StatusMessage = "Your password has been set.";

        return RedirectToPage();
    }

    public class InputModel
    {
        [DataType( DataType.Password )]
        [Display( Name = "Confirm new password" )]
        [Compare( "NewPassword" , ErrorMessage = "The new password and confirmation password do not match." )]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(
            100 ,
            ErrorMessage = "The {0} must be at least {2} and at max {1} characters long." ,
            MinimumLength = 6
        )]
        [DataType( DataType.Password )]
        [Display( Name = "New password" )]
        public string NewPassword { get; set; }
    }
}
