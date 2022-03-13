using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using TestGame.UseCases.MakeMoves;

namespace TestGame.HostedServices
{
    public class MakeMovesHostedService : IHostedService, IDisposable
    {
        private Timer _timer = null;
        private readonly IServiceScopeFactory _scopeFactory;

        public MakeMovesHostedService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(new MakeMovesCommand());
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
