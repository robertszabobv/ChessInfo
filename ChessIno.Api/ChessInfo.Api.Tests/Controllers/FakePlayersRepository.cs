using System;
using System.Collections.Generic;
using System.Linq;
using ChessInfo.Business;

namespace ChessInfo.Api.Tests.Controllers
{
    internal class FakePlayersRepository: IPlayersRepository
    {
        private readonly Dictionary<int, Player> _players = new Dictionary<int, Player>();
        public void AddPlayer(Player newPlayer)
        {
            newPlayer.PlayerId = 99;
            _players.Add(newPlayer.PlayerId, newPlayer);
        }

        public Player GetById(int playerId)
        {
            return _players.Single(p => p.Key == playerId).Value;
        }

        public IEnumerable<Player> GetPlayers(string lastName = null)
        {
            throw new NotImplementedException();
        }

        public void DeletePlayer(int playerId)
        {
            throw new NotImplementedException();
        }

        public bool Update(Player player)
        {
            return true;
        }

        public void Dispose()
        {
        }
    }
}
