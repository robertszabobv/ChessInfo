using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChessInfo.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ChessInfo.Api.Controllers
{
    [Route("games")]
    public class GamesController
    {
        private readonly IGamesRepository _gamesRepository;

        public GamesController(IGamesRepository gamesRepository)
        {
            _gamesRepository = gamesRepository;
        }
    }
}
