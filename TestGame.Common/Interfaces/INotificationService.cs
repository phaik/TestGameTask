using System.Collections.Generic;
using System.Threading.Tasks;
using TestGame.Common.Models;

namespace TestGame.Common.Interfaces
{
    public interface INotificationService
    {
        Task GameStartedNotification(Lobby lobby);

        Task MoveMakedNotification(IEnumerable<Lobby> lobbies);
    }
}
