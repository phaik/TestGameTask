using MediatR;
using TestGame.Common.Models;

namespace TestGame.UseCases.StartGame
{
    public class StartGameCommand : IRequest<Lobby>
    {
        public int LobbyId { get; set; }

        public int SecondClientId { get; set; }
    }
}
