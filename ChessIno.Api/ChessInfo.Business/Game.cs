using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace ChessInfo.Domain
{
    public class Game
    {
        private byte _gameResult;
        private GameResultDetail _resultDetail;

        [Key]
        public int GameId { get; set; }
        public Player WhitePlayer { get; set; }
        [Required]
        public int WhitePlayerId { get; set; }
        public Player BlackPlayer { get; set; }
        [Required]
        public int BlackPlayerId { get; set; }
        [Required]
        public DateTime GameDate { get; set; }

        [Required]
        [MaxLength(3)]
        [MinLength(3)]
        public string OpeningClassification { get; set; }

        [Required]
        public byte GameResult
        {
            get => _gameResult;
            set
            {
                _gameResult = value;
                _resultDetail = new GameResultDetail((GameResultTypes)_gameResult);
            } 
        }

        [NotMapped]
        public GameResultDetail ResultDetail
        {
            get => _resultDetail;
            set
            {
                _resultDetail = value;
                _gameResult = (byte)_resultDetail.ResultType;
            } 
        }

        [NotMapped]
        public bool IsValid => WhitePlayerId > 0
                               && BlackPlayerId > 9
                               && WhitePlayerId != BlackPlayerId
                               && GameResult > 0 && GameResult < 4
                               && Regex.IsMatch(OpeningClassification, @"/[A-E]\d{2}/g");
    }
}
