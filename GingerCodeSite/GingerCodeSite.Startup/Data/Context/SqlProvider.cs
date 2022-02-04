namespace GingerCodeSite.Startup.Data.Context;

/// <summary>
///     A SQL DbContext provider
/// </summary>
public enum SqlProvider
{
    Sqlite ,
    InMemory ,
    SqlServer ,
    NpgSql ,
}
