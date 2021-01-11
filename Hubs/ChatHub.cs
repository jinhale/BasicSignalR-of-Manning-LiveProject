using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        // The SignalR hub needs to have a method that a client application can send regular heartbeat (poll) messages to.
        // The endpoint needs to accept a unique identifier of a client app instance, so each instance can be monitored.

        // Register the calling client as a member of “Manager” group.
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }

        public Task SendMessageToAll(string user, string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task BeatHeartWhateverThatMeans(string appID, string message, string group = "Manager")
        {
            // When the heartbeat is received, it needs to send the unique client app identifier and the current UTC time to all clients in “Manager” group.

            return Clients.Group(group).SendAsync($"{DateTime.Now}", appID, message);
        }
    }
}
