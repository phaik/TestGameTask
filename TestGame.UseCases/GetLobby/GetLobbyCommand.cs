using MediatR;
using TestGame.Common.Models;

namespace TestGame.UseCases.GetLobby
{
    public class GetLobbyCommand : IRequest<Lobby>
    {
        public int LobbyId { get; set; }
    }
}
