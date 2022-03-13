using AutoMapper;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using TestGame.Common.Interfaces;
using TestGame.Common.Models;
using TestGame.UseCases.StartGame;

namespace TestGame.Tests
{
    // Добавил только тесты для этой команды, для остальных команд надо добавить аналогичные тесты
    [TestClass]
    public class StartGameCommandHandlerTests
    {
        private StartGameCommandHandler _startGameCommandHandler;
        private Mock<IClientRepository> _clientRepository;
        private Mock<ILobbyRepository> _lobbyRepository;
        private Mock<INotificationService> _notificationService;
        private Mock<IMapper> _mapper;

        [TestInitialize]
        public void Initialize()
        {
            _clientRepository = new Mock<IClientRepository>();
            _lobbyRepository = new Mock<ILobbyRepository>();
            _notificationService = new Mock<INotificationService>();
            _mapper = new Mock<IMapper>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task StartGameRequestNull()
        {
            _startGameCommandHandler = new StartGameCommandHandler(_clientRepository.Object, _lobbyRepository.Object, _notificationService.Object, _mapper.Object);

            StartGameCommand command = null;
            await _startGameCommandHandler.Handle(command, CancellationToken.None);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task StartGameSecondClientNotFound()
        {
            StartGameCommand command = new StartGameCommand { SecondClientId = 21 };

            _clientRepository.Setup(s => s.GetClientAsync(It.Is<int>(x => x == command.SecondClientId), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Client>(null));

            _startGameCommandHandler = new StartGameCommandHandler(_clientRepository.Object, _lobbyRepository.Object, _notificationService.Object, _mapper.Object);

            await _startGameCommandHandler.Handle(command, CancellationToken.None);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task StartGameLobbyNotFound()
        {
            StartGameCommand command = new StartGameCommand { SecondClientId = 21, LobbyId = 2121 };

            _clientRepository.Setup(s => s.GetClientAsync(It.Is<int>(x => x == command.SecondClientId), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Client>(new Client()));

            _lobbyRepository.Setup(s => s.GetLobbyAsync(It.Is<int>(x => x == command.LobbyId), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Lobby>(null));

            _startGameCommandHandler = new StartGameCommandHandler(_clientRepository.Object, _lobbyRepository.Object, _notificationService.Object, _mapper.Object);

            await _startGameCommandHandler.Handle(command, CancellationToken.None);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task StartGameAlreadyStarted()
        {
            StartGameCommand command = new StartGameCommand { SecondClientId = 21, LobbyId = 2121 };

            _clientRepository.Setup(s => s.GetClientAsync(It.Is<int>(x => x == command.SecondClientId), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Client>(new Client()));

            _lobbyRepository.Setup(s => s.GetLobbyAsync(It.Is<int>(x => x == command.LobbyId), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Lobby>(new Lobby { SecondClientId = 5 }));

            _startGameCommandHandler = new StartGameCommandHandler(_clientRepository.Object, _lobbyRepository.Object, _notificationService.Object, _mapper.Object);

            await _startGameCommandHandler.Handle(command, CancellationToken.None);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task StartGameSecondClientSameAsHost()
        {
            StartGameCommand command = new StartGameCommand { SecondClientId = 21, LobbyId = 2121 };

            _clientRepository.Setup(s => s.GetClientAsync(It.Is<int>(x => x == command.SecondClientId), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Client>(new Client()));

            _lobbyRepository.Setup(s => s.GetLobbyAsync(It.Is<int>(x => x == command.LobbyId), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Lobby>(new Lobby { HostId = 21 }));

            _startGameCommandHandler = new StartGameCommandHandler(_clientRepository.Object, _lobbyRepository.Object, _notificationService.Object, _mapper.Object);

            await _startGameCommandHandler.Handle(command, CancellationToken.None);
        }

        [TestMethod]
        public async Task StartGameSuccess()
        {
            StartGameCommand command = new StartGameCommand { SecondClientId = 21, LobbyId = 2121 };

            _clientRepository.Setup(s => s.GetClientAsync(It.Is<int>(x => x == command.SecondClientId), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Client>(new Client()));

            _lobbyRepository.Setup(s => s.GetLobbyAsync(It.Is<int>(x => x == command.LobbyId), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Lobby>(new Lobby { HostId = 5 }));

            _startGameCommandHandler = new StartGameCommandHandler(_clientRepository.Object, _lobbyRepository.Object, _notificationService.Object, _mapper.Object);

            await _startGameCommandHandler.Handle(command, CancellationToken.None);
        }
    }
}
