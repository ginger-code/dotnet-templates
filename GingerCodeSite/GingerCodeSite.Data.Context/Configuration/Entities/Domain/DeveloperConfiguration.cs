using GingerCodeSite.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GingerCodeSite.Data.Context.Configuration.Entities.Domain;

public class DeveloperConfiguration : IEntityTypeConfiguration< Developer >
{
    public void Configure( EntityTypeBuilder< Developer > builder )
    {
        builder.HasKey( x => x.DeveloperId );
        builder.Property( x => x.FullName )
               .HasMaxLength( 100 )
               .IsUnicode()
               .IsRequired();
        builder.Property( x => x.HairColor )
               .IsRequired();
        builder.Property( x => x.DevelopmentInterests )
               .IsRequired();
    }
}
