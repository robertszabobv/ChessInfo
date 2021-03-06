﻿using System.Collections.Generic;
using ChessInfo.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

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
            using (_playersRepository)
            {
                if (player == null || !player.IsValid)
                {
                    return BadRequest();
                }
                _playersRepository.AddPlayer(player);
                return CreatedAtRoute(GetPlayerRouteName, new { playerId = player.PlayerId }, player);
            }                
        }

        [HttpGet("{playerId}", Name = GetPlayerRouteName)]
        public IActionResult GetPlayer(int playerId)
        {
            using (_playersRepository)
            {
                var player = _playersRepository.GetById(playerId);
                if (player == null)
                {
                    return NotFound();
                }
                return Ok(player);
            }                   
       }

        [HttpGet]
        public IActionResult GetPlayers([FromQuery]string lastName = null)
        {
            using (_playersRepository)
            {
                IEnumerable<Player> players = _playersRepository.GetPlayers(lastName);
                if (players == null || !players.Any())
                {
                    return NotFound();
                }
                return Ok(players);
            }
            
        }

        [HttpDelete("{playerId}")]
        public IActionResult Delete(int playerId)
        {
            using (_playersRepository)
            {
                if (!_playersRepository.DeletePlayer(playerId))
                {
                    return BadRequest();
                }
                return NoContent();
            }                
        }

        [HttpPut]
        public IActionResult Update([FromBody]Player player)
        {
            if (player == null || !player.IsValid)
            {
                return BadRequest();
            }
            using (_playersRepository)
            {                
                if (_playersRepository.Update(player))
                {
                    return NoContent();
                }                
            }
            return NotFound();
        }
    }
}
