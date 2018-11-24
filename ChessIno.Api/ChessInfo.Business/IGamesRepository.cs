using System;
using System.Collections.Generic;
using System.Text;

namespace ChessInfo.Domain
{
    public interface IGamesRepository : IDisposable
    {
        void AddGame(Game newGame);
        Game GetById { get; set; }
        IEnumerable<Game> GetGames(string playerName = null, string openingClassification = null);
        bool Update(Game game);
        void DeleteGame(int gameId);        
    }
}
