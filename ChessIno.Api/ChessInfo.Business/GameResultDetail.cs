using System.Collections.Generic;

namespace ChessInfo.Domain
{
    public class GameResultDetail
    {
        public GameResultDetail(GameResultTypes resultType)
        {
            ResultType = resultType;
        }

        private readonly Dictionary<GameResultTypes, string> _results = new Dictionary<GameResultTypes, string>
        {
            { GameResultTypes.WhiteWins, "1-0" },
            { GameResultTypes.Draw, "0.5-0.5" },
            { GameResultTypes.BlackWins, "0-1" }
        };
        public GameResultTypes ResultType { get; set; }

        public string Display => ToString();

        public override string ToString()
        {
            return _results[ResultType];
        }
    }

    public enum GameResultTypes
    {
        WhiteWins = 1,
        Draw,
        BlackWins
    }
}
