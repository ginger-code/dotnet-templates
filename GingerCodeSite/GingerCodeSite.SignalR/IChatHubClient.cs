namespace GingerCodeSite.SignalR;

public interface IChatHubClient
{
    Task ReceiveMessage( string? user , string message );
}
