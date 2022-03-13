using MediatR;
using TestGame.Common.Models;

namespace TestGame.UseCases.CreateLobby
{
    public class CreateLobbyCommand : IRequest<Lobby>
    {
        public int HostId { get; set; }

        public string Name { get; set; }
    }
}
