#nullable disable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GingerCodeSite.Views.Identity.Pages.Account;

[AllowAnonymous]
public class ResetPasswordConfirmationModel : PageModel
{
    public void OnGet() { }
}
