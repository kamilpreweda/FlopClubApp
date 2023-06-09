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
        [HttpDelete("{gameId}/leave")]
        public async Task<IActionResult> LeaveGameLobby(int gameId)
        {
            var response = await _gameService.LeaveGameLobby(gameId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteGame(DeleteGameDto game)
        {
            return Ok(await _gameService.DeleteGame(game));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAllGames()
        {
            return Ok(await _gameService.GetAllGames());
        }
    }   
}
