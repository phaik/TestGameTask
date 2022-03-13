using System.Threading;
using System.Threading.Tasks;
using TestGame.Common.Models;

namespace TestGame.Common.Interfaces
{
    public interface IClientRepository
    {
        Task<Client> GetClientAsync(int id, CancellationToken cancellationToken = default);
    }
}
