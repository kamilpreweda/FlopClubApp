using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FlopClub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private readonly ITableService _tableService;

        public TablesController(ITableService tableService)
        {
            _tableService = tableService;
        }
        [Authorize]
        [HttpPost("BuyIn")]
        public async Task<ActionResult> BuyIn(int gameId)
        {
            return Ok(await _tableService.BuyIn(gameId));
        }

        [Authorize]
        [HttpPost("LeaveTable")]
        public async Task<ActionResult> LeaveTable(int gameId)
        {
            return Ok(await _tableService.LeaveTable(gameId));
        }

        [Authorize]
        [HttpPost("SetPositions")]
        public async Task<ActionResult> SetPositions(int gameId)
        {
            return Ok(await _tableService.SetPositions(gameId));
        }
    }
}
