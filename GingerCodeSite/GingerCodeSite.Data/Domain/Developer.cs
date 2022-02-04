using GingerCodeSite.Data.Domain.Enums;

namespace GingerCodeSite.Data.Domain;

//todo: Remove this file! It's just to demonstrate what a domain model might look like.

public class Developer
{
    public Guid                 DeveloperId          { get; set; }
    public DevelopmentInterests DevelopmentInterests { get; set; }
    public string               FullName             { get; set; } = null!;
    public HairColor            HairColor            { get; set; }
}
