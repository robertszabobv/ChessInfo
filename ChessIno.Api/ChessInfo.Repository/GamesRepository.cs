using System;
using System.Collections.Generic;
using System.Linq;
using ChessInfo.Domain;
using Microsoft.EntityFrameworkCore;

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
        public IEnumerable<Game> GetGames(string playerLastName = null, string openingClassification = null)
        {
            return Context.Games
                .Include(g => g.WhitePlayer)
                .Include(g => g.BlackPlayer)
                .Where(GetLastNameClause(playerLastName))
                .Where(GetOpeningClassificationClause(openingClassification))
                .ToList();            
        }

        private Func<Game, bool> GetOpeningClassificationClause(string openingClassification)
        {
            if (string.IsNullOrWhiteSpace(openingClassification))
            {
                return g => true;
            }
            return g => g.OpeningClassification.StartsWith(openingClassification, StringComparison.Ordinal);
        }

        private Func<Game, bool> GetLastNameClause(string playerLastName)
        {
            if (string.IsNullOrWhiteSpace(playerLastName))
            {
                return g => true;
            }
            return g => g.WhitePlayer.LastName.StartsWith(playerLastName)
                        || g.BlackPlayer.LastName.StartsWith(playerLastName);
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
