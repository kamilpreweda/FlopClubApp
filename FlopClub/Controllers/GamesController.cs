﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlopClub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService, DataContext context)
        {
            _gameService = gameService;
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<ActionResult> CreateGame(CreateGameDto newGame)
        {
            return Ok(await _gameService.CreateGame(newGame));
        }

        [Authorize]
        [HttpPost("Join")]
        public async Task<ActionResult> JoinGameLobby(JoinGameDto game)
        {
            return Ok(await _gameService.JoinGameLobby(game));
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteGame(DeleteGameDto game)
        {
            return Ok(await _gameService.DeleteGame(game));
        }
    }
}
