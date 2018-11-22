using System.Collections.Generic;

namespace ChessInfo.Business
{
    public interface IPlayersRepository
    {
        void AddPlayer(Player newPlayer);
        Player GetById(int playerId);
        IEnumerable<Player> GetPlayers();
    }
}