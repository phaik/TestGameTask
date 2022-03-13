using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestGame.Common.Models;

namespace TestGame.Common.Interfaces
{
    public interface ILobbyRepository
    {
        Task<Lobby> GetLobbyAsync(int id, CancellationToken cancellationToken = default);

        IQueryable<Lobby> GetLobbies();

        Task<Lobby> SaveLobbyAsync(Lobby lobby, CancellationToken cancellationToken = default);

        Task SaveLobbiesAsync(IEnumerable<Lobby> lobbies, CancellationToken cancellationToken = default);
    }
}
