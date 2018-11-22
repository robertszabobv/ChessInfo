using System;
using System.Collections.Generic;
using System.Linq;
using ChessInfo.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ChessInfo.Repository
{
    public class PlayersRepository : IPlayersRepository, IDisposable
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
            _context.Database.EnsureCreated();
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

        public IEnumerable<Player> GetPlayers()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
