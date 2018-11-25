using System;

namespace ChessInfo.Api.Dto
{
    public class GameDto
    {
        public int GameId { get; set; }
        public string WhitePlayerFirstName { get; set; }
        public string WhitPlayerLastName { get; set; }
        public string BlackPlayerFirstName { get; set; }
        public string BlackPlayerLastName { get; set; }
        public DateTime GameDate { get; set; }
        public string OpeningClassification { get; set; }
        public string Result { get; set; }
    }
}
