using System;
using System.Collections.Generic;
using System.Text;
using ChessInfo.Domain;

namespace ChessInfo.Repository
{
    public class GamesRepository : IGamesRepository
    {
        private readonly ChessInfoContext _context;
        
        public void AddGame(Game newGame)
        {
            throw new NotImplementedException();
        }

        public Game GetById { get; set; }
        public IEnumerable<Game> GetGames(string playerName = null, string openingClassification = null)
        {
            throw new NotImplementedException();
        }

        public bool Update(Game game)
        {
            throw new NotImplementedException();
        }

        public void DeleteGame(int gameId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
