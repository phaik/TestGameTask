using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestGame.Common.Interfaces;
using TestGame.Common.Models;

namespace TestGame.UseCases.CreateLobby
{
    public class CreateLobbyCommandHandler : IRequestHandler<CreateLobbyCommand, Lobby>
    {
        protected readonly IClientRepository _clientRepository;
        protected readonly ILobbyRepository _lobbyRepository;
        protected readonly IMapper _mapper;

        public CreateLobbyCommandHandler(
            IClientRepository clientRepository,
            ILobbyRepository lobbyRepository,
            IMapper mapper)
        {
            _clientRepository = clientRepository;
            _lobbyRepository = lobbyRepository;
            _mapper = mapper;
        }

        public async Task<Lobby> Handle(CreateLobbyCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var host = await _clientRepository.GetClientAsync(request.HostId, cancellationToken);
            if (host == null)
                throw new KeyNotFoundException($"Cannot find client with id={request.HostId}");

            var lobby = _mapper.Map<Lobby>(request);
            lobby = await _lobbyRepository.SaveLobbyAsync(lobby, cancellationToken);
            return lobby;
        }
    }
}
