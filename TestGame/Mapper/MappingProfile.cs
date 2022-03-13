using AutoMapper;
using TestGame.Common.Models;
using TestGame.DTOs;
using TestGame.Repository.DAOs;
using TestGame.UseCases.CreateLobby;

namespace TestGame.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Client, ClientDAO>().ReverseMap();
            CreateMap<Lobby, LobbyDAO>().ReverseMap();
            CreateMap<Lobby, LobbyDTO>().ReverseMap();
            CreateMap<CreateLobbyDTO, CreateLobbyCommand>();
            CreateMap<CreateLobbyCommand, Lobby>(); 
        }
    }
}
