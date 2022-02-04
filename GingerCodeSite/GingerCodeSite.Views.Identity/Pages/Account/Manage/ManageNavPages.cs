#nullable disable

using Microsoft.AspNetCore.Mvc.Rendering;

namespace GingerCodeSite.Views.Identity.Pages.Account.Manage;

public static class ManageNavPages
{
    public static string ChangePassword
        => "change-password";

    public static string DeletePersonalData
        => "delete-data";

    public static string DownloadPersonalData
        => "doanload-data";

    public static string Email
        => "email";

    public static string ExternalLogins
        => "external-logins";

    public static string Index
        => "/";

    public static string PersonalData
        => "personal-data";

    public static string TwoFactorAuthentication
        => "two-factor";

    public static string IndexNavClass( ViewContext viewContext )
        => PageNavClass( viewContext , Index );

    public static string EmailNavClass( ViewContext viewContext )
        => PageNavClass( viewContext , Email );

    public static string ChangePasswordNavClass( ViewContext viewContext )
        => PageNavClass( viewContext , ChangePassword );

    public static string DownloadPersonalDataNavClass( ViewContext viewContext )
        => PageNavClass( viewContext , DownloadPersonalData );

    public static string DeletePersonalDataNavClass( ViewContext viewContext )
        => PageNavClass( viewContext , DeletePersonalData );

    public static string ExternalLoginsNavClass( ViewContext viewContext )
        => PageNavClass( viewContext , ExternalLogins );

    public static string PersonalDataNavClass( ViewContext viewContext )
        => PageNavClass( viewContext , PersonalData );

    public static string TwoFactorAuthenticationNavClass( ViewContext viewContext )
        => PageNavClass( viewContext , TwoFactorAuthentication );

    public static string PageNavClass( ViewContext viewContext , string page )
    {
        var activePage = viewContext.ViewData[ "ActivePage" ] as string ?? Path.GetFileNameWithoutExtension( viewContext.ActionDescriptor.DisplayName );
        return string.Equals(
                   activePage ,
                   page ,
                   StringComparison.OrdinalIgnoreCase
               )
                   ? "active"
                   : null;
    }
}
