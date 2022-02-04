using Functional;

namespace GingerCodeSite.Util;

public static class ResultExtensions
{
    public static string GetErrorMessageOrEmptyString( this Result< Unit , string > result )
        => result.Match( _ => "" , err => err );
}
