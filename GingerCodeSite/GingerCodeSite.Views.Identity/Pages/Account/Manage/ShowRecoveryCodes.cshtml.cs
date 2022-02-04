#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GingerCodeSite.Views.Identity.Pages.Account.Manage;

public class ShowRecoveryCodesModel : PageModel
{
    [TempData]
    public string[] RecoveryCodes { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    public IActionResult OnGet()
    {
        if ( RecoveryCodes == null || RecoveryCodes.Length == 0 )
        {
            return RedirectToPage( "./two-factor" );
        }

        return Page();
    }
}
