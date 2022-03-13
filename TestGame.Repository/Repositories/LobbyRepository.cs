using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestGame.Common.Interfaces;
using TestGame.Common.Models;
using TestGame.Repository.DAOs;

namespace TestGame.Repository.Repositories
{
    public class LobbyRepository : ILobbyRepository
    {
        protected readonly ITestGameContext _context;
        protected readonly IMapper _mapper;

        public LobbyRepository(
            ITestGameContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Lobby> GetLobbyAsync(int id, CancellationToken cancellationToken = default)
        {
            var dbLobby = await _context.Lobbies.FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);

            return _mapper.Map<Lobby>(dbLobby);
        }

        public IQueryable<Lobby> GetLobbies()
        {
            return _context.Lobbies.AsNoTracking().ProjectTo<Lobby>(_mapper.ConfigurationProvider);
        }

        public Task<Lobby> SaveLobbyAsync(Lobby lobby, CancellationToken cancellationToken = default)
        {
            return SaveLobbyAsync(lobby, true, cancellationToken);
        }

        public async Task SaveLobbiesAsync(IEnumerable<Lobby> lobbies, CancellationToken cancellationToken = default)
        {
            foreach (var lobby in lobbies)
                await SaveLobbyAsync(lobby, false, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        private async Task<Lobby> SaveLobbyAsync(Lobby lobby, bool saveChanges, CancellationToken cancellationToken = default)
        {
            if (lobby == null)
                throw new ArgumentNullException(nameof(lobby));

            var changed = _mapper.Map<LobbyDAO>(lobby);
            var dbLobby = await _context.Lobbies.FirstOrDefaultAsync(x => x.Id == lobby.Id, cancellationToken).ConfigureAwait(false);
            if (dbLobby == null)
            {
                dbLobby = changed;
                dbLobby.CreateDate = DateTime.UtcNow;
                await _context.Lobbies.AddAsync(dbLobby, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                _mapper.Map(lobby, dbLobby);
            }
            if (saveChanges)
                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return _mapper.Map<Lobby>(dbLobby);
        }
    }
}
