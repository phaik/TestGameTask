using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestGame.Common.Interfaces;
using TestGame.Common.Models;

namespace TestGame.UseCases.GetLobby
{
    public class GetLobbyCommandHandler : IRequestHandler<GetLobbyCommand, Lobby>
    {
        protected readonly ILobbyRepository _lobbyRepository;

        public GetLobbyCommandHandler(ILobbyRepository lobbyRepository)
        {
            _lobbyRepository = lobbyRepository;
        }

        public async Task<Lobby> Handle(GetLobbyCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var lobby = await _lobbyRepository.GetLobbyAsync(request.LobbyId, cancellationToken);
            if (lobby == null)
                throw new KeyNotFoundException($"Cannot find lobby with id={request.LobbyId}");
            return lobby;
        }
    }
}
