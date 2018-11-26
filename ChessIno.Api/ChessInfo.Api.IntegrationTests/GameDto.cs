using System;
using System.Collections.Generic;
using System.Text;

namespace ChessInfo.Api.IntegrationTests
{
    internal class GameDto
    {
        public int GameId { get; set; }
        public string WhitePlayer { get; set; }
        public string BlackPlayer { get; set; }
        public DateTime GameDate { get; set; }
        public string OpeningClassification { get; set; }
        public string Result { get; set; }
    }
}
