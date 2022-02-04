using Microsoft.AspNetCore.SignalR;

namespace GingerCodeSite.SignalR;

public class ChatHub : Hub< IChatHubClient >
{
    public async Task SendMessage( string user , string message )
    {
        await Clients.User( user )
                     .ReceiveMessage( Context.User?.Identity?.Name , message );
    }

    public Task SendMessageToCaller( string message )
        => Clients.Caller.ReceiveMessage( Context.User?.Identity?.Name , message );
}
