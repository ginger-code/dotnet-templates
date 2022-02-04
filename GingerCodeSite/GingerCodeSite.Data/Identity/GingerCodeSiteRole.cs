#nullable disable
using Microsoft.AspNetCore.Identity;

namespace GingerCodeSite.Data.Identity;

public sealed class GingerCodeSiteRole : IdentityRole< Guid >
{
    public override string ConcurrencyStamp { get; set; } = Guid.NewGuid()
                                                                .ToString();

    public override Guid   Id             { get; set; }
    public override string Name           { get; set; }
    public override string NormalizedName { get; set; }
}
