using GingerCodeSite.Data.Domain.Enums;

namespace GingerCodeSite.Data.ViewModels;

//todo: Remove this file! It's just to demonstrate what a view model might look like.

public class DeveloperHairColor
{
    public string    DeveloperName { get; set; } = null!;
    public HairColor HairColor     { get; set; }
}
