using System.Collections.Generic;
using System.Linq;
using ChessInfo.Api.Dto;
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

        private GameDto CreateDtoFrom(Game game)
        {
            return new GameDto
            {
                WhitePlayer = $"{game.WhitePlayer.FirstName} {game.WhitePlayer.LastName}",
                BlackPlayer = $"{game.BlackPlayer.FirstName} {game.BlackPlayer.LastName}",
                GameId = game.GameId,
                OpeningClassification = game.OpeningClassification,
                GameDate = game.GameDate,
                Result = game.ResultDetail.ToString()
            };
        }

        private IEnumerable<GameDto> CreateDtoListFrom(IEnumerable<Game> games)
        {
            var gamesList = games.ToList();
            var dtos = new List<GameDto>(gamesList.Count);
            foreach (Game game in gamesList)
            {
                dtos.Add(CreateDtoFrom(game));
            }
            return dtos;
        }

        [HttpGet]
        public IActionResult GetGames([FromQuery]string playerLastName = null, string openingClassification = null)
        {
            using (_gamesRepository)
            {
                IEnumerable<Game> games = _gamesRepository.GetGames(playerLastName, openingClassification);
                IEnumerable<Game> gamesList = games.ToList();
                if (!gamesList.Any())
                {
                    return NotFound();
                }
                return Ok(CreateDtoListFrom(gamesList));
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
