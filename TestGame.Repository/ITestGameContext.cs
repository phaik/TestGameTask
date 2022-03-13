using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TestGame.Repository.DAOs;

namespace TestGame.Repository
{
    public interface ITestGameContext
    {
        public DbSet<ClientDAO> Clients { get; set; }

        public DbSet<LobbyDAO> Lobbies { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        public void Migrate();
    }
}
