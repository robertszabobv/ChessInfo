using System;
using System.Collections.Generic;
using System.Linq;
using ChessInfo.Domain;

namespace ChessInfo.Api.Tests.Controllers
{
    internal class FakeGamesRepository : IGamesRepository
    {
        private readonly Dictionary<int, Game> _games = new Dictionary<int, Game>();

        public void Dispose()
        {
            
        }

        public void AddGame(Game newGame)
        {
            newGame.GameId = 99;
            _games.Add(newGame.GameId, newGame);
        }

        public Game GetById(int gameId)
        {
            return _games.Single(g => g.Key == gameId).Value;
        }

        public IEnumerable<Game> GetGames(string playerLastName = null, string openingClassification = null)
        {
            throw new NotImplementedException();
        }

        public bool Update(Game game)
        {
            return true;
        }

        public void DeleteGame(int gameId)
        {
            throw new NotImplementedException();
        }
    }
}
