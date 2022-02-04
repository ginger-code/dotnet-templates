using GingerCodeSite.Views.Identity.Pages;
using Microsoft.AspNetCore.Hosting;

[assembly : HostingStartup( typeof( IdentityHostingStartup ) )]

namespace GingerCodeSite.Views.Identity.Pages;

public class IdentityHostingStartup : IHostingStartup
{
    public void Configure( IWebHostBuilder builder )
    {
        builder.ConfigureServices( ( context , services ) => { } );
    }
}
