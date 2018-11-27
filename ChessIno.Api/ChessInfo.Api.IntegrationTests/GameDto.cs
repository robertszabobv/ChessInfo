using System;
using System.Collections.Generic;
using System.Text;

namespace ChessInfo.Api.IntegrationTests
{
    internal class GameDto
    {
        public int GameId { get; set; }
        public string WhitePlayer { get; set; }
        public int WhitePlayerId { get; set; }
        public string BlackPlayer { get; set; }
        public int BlackPlayerId { get; set; }
        public DateTime GameDate { get; set; }
        public string OpeningClassification { get; set; }
        public byte Result { get; set; }
        public string ResultDetail { get; set; }
    }
}
