using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessInfo.Business;
using ChessIno.Api;

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

        public IEnumerable<Player> GetPlayers()
        {
            throw new NotImplementedException();
        }
    }
}
