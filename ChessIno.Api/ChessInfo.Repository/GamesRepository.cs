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
            var game = Context.Games
                .Include(g => g.WhitePlayer)
                .Include(g => g.BlackPlayer).SingleOrDefault(g => g.GameId == gameId);
            if (game != null) game.ResultDetail = new GameResultDetail((GameResultTypes) game.GameResult);

            return game;
        }
        public IEnumerable<Game> GetGames(string playerLastName = null, string openingClassification = null)
        {
            var games = Context.Games
                .Include(g => g.WhitePlayer)
                .Include(g => g.BlackPlayer)
                .Where(GetLastNameClause(playerLastName))
                .Where(GetOpeningClassificationClause(openingClassification))
                .ToList();

            foreach (Game game in games)
            {
                game.ResultDetail = new GameResultDetail((GameResultTypes)game.GameResult);
            }
            return games;
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
            var gameToUpdate = GetById(game.GameId);
            if (gameToUpdate == null)
            {
                return false;   
            }

            gameToUpdate.WhitePlayerId = game.WhitePlayerId;
            gameToUpdate.BlackPlayerId = game.BlackPlayerId;
            gameToUpdate.GameDate = game.GameDate;
            gameToUpdate.GameResult = game.GameResult;
            gameToUpdate.OpeningClassification = game.OpeningClassification;
            int rowsAffected = Context.SaveChanges();

            return rowsAffected > 0;

        }

        public void DeleteGame(int gameId)
        {
            var gameToDelete = GetById(gameId);
            if (gameToDelete != null)
            {
                Context.Games.Remove(gameToDelete);
                Context.SaveChanges();
            }
        }        
    }
}
