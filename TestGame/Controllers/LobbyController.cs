using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestGame.DTOs;
using TestGame.UseCases.CreateLobby;
using TestGame.UseCases.GetActiveLobbies;
using TestGame.UseCases.GetLobby;
using TestGame.UseCases.StartGame;

namespace TestGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LobbyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public LobbyController(
            IMediator mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<LobbyDTO>>> Get()
        {
            try
            {
                var response = await _mediator.Send(new GetActiveLobbiesCommand());
                return Ok(_mapper.Map<IEnumerable<LobbyDTO>>(response));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LobbyDTO>> Post([FromBody] CreateLobbyDTO lobby)
        {
            try
            {
                var command = _mapper.Map<CreateLobbyCommand>(lobby);
                var response = await _mediator.Send(command);
                return Created($"{Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(Request)}/{response.Id}", _mapper.Map<LobbyDTO>(response));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/start")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LobbyDTO>> Put(int id, [FromBody] StartGameDTO start)
        {
            try
            {
                var response = await _mediator.Send(new StartGameCommand { LobbyId = id, SecondClientId = start.SecondClientId });
                return Ok(_mapper.Map<LobbyDTO>(response));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LobbyDTO>> Get(int id)
        {
            try
            {
                var response = await _mediator.Send(new GetLobbyCommand { LobbyId = id });
                return Ok(_mapper.Map<LobbyDTO>(response));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
