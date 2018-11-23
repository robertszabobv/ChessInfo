using System;
using System.Collections.Generic;

namespace ChessInfo.Business
{
    public interface IPlayersRepository : IDisposable
    {
        void AddPlayer(Player newPlayer);
        Player GetById(int playerId);
        IEnumerable<Player> GetPlayers(string lastName = null);
        void DeletePlayer(int playerId);
    }
}