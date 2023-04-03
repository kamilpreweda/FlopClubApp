using Microsoft.AspNetCore.Http;
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
        [HttpPost]
        public async Task<ActionResult> CreateGame(CreateGameDto newGame)
        {
            return Ok(await _gameService.CreateGame(newGame));
        }
    }
}
