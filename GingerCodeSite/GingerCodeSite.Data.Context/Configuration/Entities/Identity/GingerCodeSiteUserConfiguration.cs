using GingerCodeSite.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GingerCodeSite.Data.Context.Configuration.Entities.Identity;

public class GingerCodeSiteUserConfiguration : IEntityTypeConfiguration< GingerCodeSiteUser >
{
    public void Configure( EntityTypeBuilder< GingerCodeSiteUser > builder )
    {
        builder.Property( x => x.PhoneNumber )
               .IsRequired( false );
    }
}
