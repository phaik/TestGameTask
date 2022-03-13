using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestGame.Common.Interfaces;
using TestGame.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace TestGame.UseCases.GetActiveLobbies
{
    public class GetActiveLobbiesCommandHandler : IRequestHandler<GetActiveLobbiesCommand, IEnumerable<Lobby>>
    {
        protected readonly ILobbyRepository _lobbyRepository;
        protected readonly IMapper _mapper;

        public GetActiveLobbiesCommandHandler(
            ILobbyRepository lobbyRepository,
            IMapper mapper)
        {
            _lobbyRepository = lobbyRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Lobby>> Handle(GetActiveLobbiesCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var activeLobbies = await _lobbyRepository.GetLobbies().Where(x => x.StartDate == null).ToListAsync(cancellationToken);
            return activeLobbies;
        }
    }
}
