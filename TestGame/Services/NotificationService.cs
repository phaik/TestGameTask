using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TestGame.Common.Interfaces;
using TestGame.Common.Models;
using TestGame.Hubs;

namespace TestGame.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _notificationHubContext;

        public NotificationService(IHubContext<NotificationHub> notificationHubContext)
        {
            _notificationHubContext = notificationHubContext;
        }

        public async Task GameStartedNotification(Lobby lobby)
        {
            await _notificationHubContext.Clients.User(lobby.HostId.ToString()).SendAsync("GameStarted", JsonSerializer.Serialize(lobby));
        }

        public async Task MoveMakedNotification(IEnumerable<Lobby> lobbies)
        {
            foreach (var lobby in lobbies)
            {
                await _notificationHubContext.Clients.Users(new string[] { lobby.HostId.ToString(), lobby.SecondClientId.ToString() }).SendAsync("MoveMaked", JsonSerializer.Serialize(lobby));
            }
        }
    }
}
