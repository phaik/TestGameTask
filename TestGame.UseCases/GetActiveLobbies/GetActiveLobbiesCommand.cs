using MediatR;
using System.Collections.Generic;
using TestGame.Common.Models;

namespace TestGame.UseCases.GetActiveLobbies
{
    public class GetActiveLobbiesCommand : IRequest<IEnumerable<Lobby>>
    {
    }
}
