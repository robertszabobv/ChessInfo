﻿using ChessInfo.Business;
using Microsoft.AspNetCore.Mvc;

namespace ChessInfo.Api.Controllers
{
    [Route("players")]
    public class PlayersController : Controller
    {
        private readonly IPlayersRepository _playersRepository;
        private const string GetPlayerRouteName = "GetPlayer";

        public PlayersController(IPlayersRepository playersRepository)
        {
            _playersRepository = playersRepository;
        }

        [HttpPost]
        public IActionResult CreatePlayer([FromBody]Player player)
        {
            if (player == null || !player.IsValid)
            {
                return BadRequest();
            }
            _playersRepository.AddPlayer(player);
            return CreatedAtRoute(GetPlayerRouteName, new { playerId = player.PlayerId }, player);
        }

        [HttpGet("{playerId}", Name = GetPlayerRouteName)]
        public IActionResult GetPlayer(int playerId)
        {
            var player = _playersRepository.GetById(playerId);
            if (player == null)
            {
                return NotFound();
            }
            return Ok(player);          
       }

        [HttpGet]
        public IActionResult GetPlayers()
        {
            return NotFound();
        }
    }
}
