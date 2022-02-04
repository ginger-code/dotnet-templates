#nullable disable

using Microsoft.AspNetCore.Identity;

namespace GingerCodeSite.Data.Identity;

public sealed class GingerCodeSiteUser : IdentityUser< Guid >
{
    public override int AccessFailedCount { get; set; }

    public override string ConcurrencyStamp { get; set; } = Guid.NewGuid()
                                                                .ToString();

    public override string          Email                { get; set; }
    public override bool            EmailConfirmed       { get; set; }
    public override Guid            Id                   { get; set; }
    public override bool            LockoutEnabled       { get; set; }
    public override DateTimeOffset? LockoutEnd           { get; set; }
    public override string          NormalizedEmail      { get; set; }
    public override string          NormalizedUserName   { get; set; }
    public override string          PasswordHash         { get; set; }
    public override string          PhoneNumber          { get; set; }
    public override bool            PhoneNumberConfirmed { get; set; }
    public override string          SecurityStamp        { get; set; }
    public override bool            TwoFactorEnabled     { get; set; }
    public override string          UserName             { get; set; }
}
