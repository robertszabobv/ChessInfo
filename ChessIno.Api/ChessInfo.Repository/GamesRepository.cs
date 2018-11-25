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
            if (!string.IsNullOrWhiteSpace(playerLastName))
            {
                return Context.Games
                    .Include(g => g.WhitePlayer)
                    .Include(g => g.BlackPlayer)
                    .Where(g => g.WhitePlayer.LastName.StartsWith(playerLastName) || g.BlackPlayer.LastName.StartsWith(playerLastName))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(openingClassification))
            {
                return Context.Games
                    .Include(g => g.WhitePlayer)
                    .Include(g => g.BlackPlayer)
                    .Where(g => g.OpeningClassification.StartsWith(openingClassification, StringComparison.Ordinal))
                    .ToList();
            }
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
