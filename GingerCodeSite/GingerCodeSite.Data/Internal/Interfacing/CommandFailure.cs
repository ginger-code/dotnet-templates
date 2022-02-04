namespace GingerCodeSite.Data.Internal.Interfacing;

public sealed record CommandFailure( string Message ) : CommandResult;
