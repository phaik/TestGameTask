using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TestGame.Common.Interfaces;
using TestGame.Common.Models;

namespace TestGame.Repository.Repositories
{
    public class ClientRepository : IClientRepository
    {
        protected readonly ITestGameContext _context;
        protected readonly IMapper _mapper;

        public ClientRepository(
            ITestGameContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Client> GetClientAsync(int id, CancellationToken cancellationToken = default)
        {
            var dbClient = await _context.Clients.FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);

            return _mapper.Map<Client>(dbClient);
        }
    }
}
