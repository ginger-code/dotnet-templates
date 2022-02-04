namespace GingerCodeSite.Data.Access;

/// <summary>
///     An empty type, useful for imitating the unit/() as would be found in F#, Rust, etc.
/// </summary>
public readonly ref struct Unit
{
    //Provides a default instance of the empty argument
    public static Unit Arg
        => new();
}
