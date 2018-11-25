using System;
using System.Collections.Generic;
using System.Linq;
using ChessInfo.Domain;

namespace ChessInfo.Repository
{
    public class PlayersRepository : BaseRepository, IPlayersRepository
    {        
        public void AddPlayer(Player newPlayer)
        {            
            Context.Players.Add(newPlayer);
            Context.SaveChanges();            
        }

        public Player GetById(int playerId)
        {
            return Context.Players.SingleOrDefault(p => p.PlayerId == playerId);
        }

        public IEnumerable<Player> GetPlayers(string lastName = null)
        {
            return Context.Players.Where(Matching(lastName)).ToList();
        }

        public bool DeletePlayer(int playerId)
        {
            if (HasAnyGame(playerId))
            {
                return false;
            }
            Player playerToDelete = Context.Players.Single(p => p.PlayerId == playerId);
            Context.Remove(playerToDelete);
            Context.SaveChanges();
            return true;
        }

        private bool HasAnyGame(int playerId)
        {
            return Context.Games.Any(g => g.WhitePlayerId == playerId || g.BlackPlayerId == playerId);
        }
       
        public bool Update(Player player)
        {
            var playerToUpdate = GetById(player.PlayerId);
            if (playerToUpdate == null)
            {
                return false;
            }

            playerToUpdate.FirstName = player.FirstName;
            playerToUpdate.LastName = player.LastName;
            playerToUpdate.Rating = player.Rating;
            int noOfRowsAffected = Context.SaveChanges();

            return noOfRowsAffected > 0;
        }

        private Func<Player, bool> Matching(string lastName)
        {
            return string.IsNullOrWhiteSpace(lastName)
                ? p => true
                : new Func<Player, bool>(p => p.LastName.StartsWith(lastName, StringComparison.OrdinalIgnoreCase));
        }        
    }
}
