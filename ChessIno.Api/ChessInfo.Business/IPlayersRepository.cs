using System;
using System.Collections.Generic;

namespace ChessInfo.Domain
{
    public interface IPlayersRepository : IDisposable
    {
        void AddPlayer(Player newPlayer);
        Player GetById(int playerId);
        IEnumerable<Player> GetPlayers(string lastName = null);
        void DeletePlayer(int playerId);
        bool Update(Player player);

    }
}