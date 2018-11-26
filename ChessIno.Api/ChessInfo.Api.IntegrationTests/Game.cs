using System;

namespace ChessInfo.Api.IntegrationTests
{
    internal class Game
    {
        public int GameId { get; set; }
        public int WhitePlayerId { get; set; }
        public int BlackPlayerId { get; set; }
        public DateTime GameDate { get; set; }
        public string OpeningClassification { get; set; }
        public byte GameResult { get; set; }
    }
}
