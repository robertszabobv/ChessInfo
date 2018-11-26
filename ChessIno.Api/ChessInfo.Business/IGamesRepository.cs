using System;
using System.Collections.Generic;

namespace ChessInfo.Domain
{
    public interface IGamesRepository : IDisposable
    {
        void AddGame(Game newGame);
        Game GetById(int gameId);
        IEnumerable<Game> GetGames(string playerLastName = null, string openingClassification = null);
        bool Update(Game game);
        void DeleteGame(int gameId);        
    }
}
