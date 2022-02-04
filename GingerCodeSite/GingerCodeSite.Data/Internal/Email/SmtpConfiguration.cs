namespace GingerCodeSite.Data.Internal.Email;

public sealed class SmtpConfiguration
{
    public string Hostname { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int    Port     { get; set; }
    public string Username { get; set; } = null!;
}
