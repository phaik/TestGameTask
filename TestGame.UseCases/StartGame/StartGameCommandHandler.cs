using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestGame.Common.Interfaces;
using TestGame.Common.Models;
using System.Transactions;
using System.ComponentModel.DataAnnotations;

namespace TestGame.UseCases.StartGame
{
    public class StartGameCommandHandler : IRequestHandler<StartGameCommand, Lobby>
    {
        protected readonly IClientRepository _clientRepository;
        protected readonly ILobbyRepository _lobbyRepository;
        protected readonly INotificationService _notificationService;
        protected readonly IMapper _mapper; 

        public StartGameCommandHandler(
            IClientRepository clientRepository,
            ILobbyRepository lobbyRepository,
            INotificationService notificationService,
            IMapper mapper)
        {
            _clientRepository = clientRepository;
            _lobbyRepository = lobbyRepository;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public async Task<Lobby> Handle(StartGameCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var secondClient = await _clientRepository.GetClientAsync(request.SecondClientId, cancellationToken);
            if (secondClient == null)
                throw new KeyNotFoundException($"Cannot find client with id={request.SecondClientId}");

            Lobby result;
            using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew, TransactionScopeAsyncFlowOption.Enabled))
            {
                var lobby = await _lobbyRepository.GetLobbyAsync(request.LobbyId, cancellationToken);
                if (lobby == null)
                    throw new KeyNotFoundException($"Cannot find lobby with id={request.LobbyId}");

                if (lobby.SecondClientId.HasValue)
                    throw new ValidationException($"Game at lobby {request.LobbyId} already started.");

                if (lobby.HostId == request.SecondClientId)
                    throw new ValidationException($"Client with id={request.LobbyId} already host of lobby.");

                lobby.SecondClientId = request.SecondClientId;
                lobby.StartDate = DateTime.UtcNow;
                result = await _lobbyRepository.SaveLobbyAsync(lobby, cancellationToken);
                transaction.Complete();
            }
            await _notificationService.GameStartedNotification(result);
            return result;
        }
    }
}
