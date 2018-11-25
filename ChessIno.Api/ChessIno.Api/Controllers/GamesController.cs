using ChessInfo.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ChessInfo.Api.Controllers
{
    [Route("games")]
    public class GamesController : Controller
    {
        private readonly IGamesRepository _gamesRepository;

        public GamesController(IGamesRepository gamesRepository)
        {
            _gamesRepository = gamesRepository;
        }

        [HttpPost]
        public IActionResult CreateGame([FromBody] Game game)
        {
            if (!game.IsValid)
            {
                return BadRequest();
            }

            return null;
        }
    }
}
