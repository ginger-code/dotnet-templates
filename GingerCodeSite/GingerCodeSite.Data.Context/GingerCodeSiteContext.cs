using GingerCodeSite.Data.Domain;
using GingerCodeSite.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GingerCodeSite.Data.Context;

public class GingerCodeSiteContext : IdentityDbContext< GingerCodeSiteUser , GingerCodeSiteRole , Guid >
{
    public DbSet< Developer > Developers { get; set; } = null!;

    public GingerCodeSiteContext( DbContextOptions options )
        : base( options ) { }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        base.OnModelCreating( modelBuilder );
        modelBuilder.ApplyConfigurationsFromAssembly(
            GetType()
                .Assembly
        );
    }
}
