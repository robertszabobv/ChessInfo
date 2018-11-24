using System;
using System.Collections.Generic;
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
    }
}
