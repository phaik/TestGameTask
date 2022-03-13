using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestGame.Common.Interfaces;
using TestGame.Common.Models;

namespace TestGame.UseCases.MakeMoves
{
    public class MakeMovesCommandHandler : AsyncRequestHandler<MakeMovesCommand>
    {
        protected readonly ILobbyRepository _lobbyRepository;
        protected readonly INotificationService _notificationService;
        protected readonly IMapper _mapper;

        public MakeMovesCommandHandler(
            ILobbyRepository lobbyRepository,
            INotificationService notificationService,
            IMapper mapper)
        {
            _lobbyRepository = lobbyRepository;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        protected override async Task Handle(MakeMovesCommand request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var rand = new Random();
            var lobbiestToMakeMove = await _lobbyRepository.GetLobbies()
                .Where(x => x.WinnerId == null && x.StartDate != null)
                .ToListAsync(cancellationToken);
            lobbiestToMakeMove = lobbiestToMakeMove.Where(x => (now - x.StartDate.Value).TotalSeconds > x.MovesCount).ToList();
            if (!lobbiestToMakeMove.Any())
                return;

            foreach (var lobbyToMakeMove in lobbiestToMakeMove)
            {
                lobbyToMakeMove.MovesCount++;
                lobbyToMakeMove.HostHealth -= rand.Next(0, 3);
                if (lobbyToMakeMove.HostHealth <= 0)
                { 
                    lobbyToMakeMove.WinnerId = lobbyToMakeMove.SecondClientId;
                    continue;
                }

                lobbyToMakeMove.SecondClientHealth -= rand.Next(0, 3);
                if (lobbyToMakeMove.SecondClientHealth <= 0)
                    lobbyToMakeMove.WinnerId = lobbyToMakeMove.HostId;
            }
            await _lobbyRepository.SaveLobbiesAsync(lobbiestToMakeMove, cancellationToken);
            await _notificationService.MoveMakedNotification(lobbiestToMakeMove);
        }
    }
}
