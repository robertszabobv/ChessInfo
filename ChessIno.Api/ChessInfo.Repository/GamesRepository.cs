using System;
using System.Collections.Generic;
using System.Linq;
using ChessInfo.Domain;

namespace ChessInfo.Repository
{
    public class GamesRepository : BaseRepository, IGamesRepository
    {        
        public void AddGame(Game newGame)
        {
            Context.Games.Add(newGame);
            Context.SaveChanges();
        }

        public Game GetById(int gameId)
        {
            return Context.Games.Single(g => g.GameId == gameId);
        }
        public IEnumerable<Game> GetGames(string playerName = null, string openingClassification = null)
        {
            return Context.Games.ToList();
        }

        public bool Update(Game game)
        {
            throw new NotImplementedException();
        }

        public void DeleteGame(int gameId)
        {
            throw new NotImplementedException();
        }        
    }
}
