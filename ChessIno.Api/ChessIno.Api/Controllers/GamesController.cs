using System.Collections.Generic;
using System.Linq;
using ChessInfo.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ChessInfo.Api.Controllers
{
    [Route("games")]
    public class GamesController : Controller
    {
        private const string GetGameRouteName = "GetGame";
        private readonly IGamesRepository _gamesRepository;

        public GamesController(IGamesRepository gamesRepository)
        {
            _gamesRepository = gamesRepository;
        }

        [HttpPost]
        public IActionResult CreateGame([FromBody] Game game)
        {
            if (game== null || !game.IsValid)
            {
                return BadRequest();
            }

            using (_gamesRepository)
            {
                _gamesRepository.AddGame(game);
            }
            return CreatedAtRoute(GetGameRouteName, new { gameId = game.GameId }, game);
        }

        [HttpGet("{gameId}", Name = GetGameRouteName)]
        public IActionResult GetGame(int gameId)
        {
            using (_gamesRepository)
            {
                var game = _gamesRepository.GetById(gameId);
                if (game == null)
                {
                    return NotFound();
                }
                return Ok(game);
            }
        }

        [HttpGet]
        public IActionResult GetGames([FromQuery]string playerLastName = null, string openingClassification = null)
        {
            using (_gamesRepository)
            {
                IEnumerable<Game> games = _gamesRepository.GetGames(playerLastName, openingClassification);
                if (games == null || !games.Any())
                {
                    return NotFound();
                }
                return Ok(games);
            }
        }

        [HttpPut]
        public IActionResult UpdateGame([FromBody]Game game)
        {
            if (game == null || !game.IsValid)
            {
                return BadRequest();
            }

            using (_gamesRepository)
            {
                if (_gamesRepository.Update(game))
                {
                    return NoContent();
                }
            }
            return NotFound();
        }

        [HttpDelete("{gameId}")]
        public IActionResult Delete(int gameId)
        {
            using (_gamesRepository)
            {
                _gamesRepository.DeleteGame(gameId);
                return NoContent();
            }
        }

    }
}
