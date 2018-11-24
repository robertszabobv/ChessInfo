using System;
using System.Collections.Generic;
using System.Linq;
using ChessInfo.Business;
using ChessInfo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ChessInfo.Repository
{
    public class PlayersRepository : IPlayersRepository
    {
        private readonly ChessInfoContext _context;

        public PlayersRepository()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<ChessInfoContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ChessInfoContext"));
            _context = new ChessInfoContext(optionsBuilder.Options);

            _context.Database.Migrate();
        }
        
        public void AddPlayer(Player newPlayer)
        {            
            _context.Players.Add(newPlayer);
            _context.SaveChanges();            
        }

        public Player GetById(int playerId)
        {
            return _context.Players.SingleOrDefault(p => p.PlayerId == playerId);
        }

        public IEnumerable<Player> GetPlayers(string lastName = null)
        {
            return _context.Players.Where(Matching(lastName)).ToList();
        }

        public void DeletePlayer(int playerId)
        {
            Player playerToDelete = _context.Players.Single(p => p.PlayerId == playerId);
            _context.Remove(playerToDelete);
            _context.SaveChanges();
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
            int noOfRowsAffected = _context.SaveChanges();

            return noOfRowsAffected > 0;
        }

        private Func<Player, bool> Matching(string lastName)
        {
            return string.IsNullOrWhiteSpace(lastName)
                ? p => true
                : new Func<Player, bool>(p => p.LastName.StartsWith(lastName, StringComparison.OrdinalIgnoreCase));
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
