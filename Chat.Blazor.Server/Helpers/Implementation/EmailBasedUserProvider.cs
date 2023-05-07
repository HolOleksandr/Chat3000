using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Chat.Blazor.Server.Helpers.Implementation
{
    public class EmailBasedUserProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.Email)?.Value!;
        }
    }
}
