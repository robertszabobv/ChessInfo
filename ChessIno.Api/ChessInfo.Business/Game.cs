using System;
using System.ComponentModel.DataAnnotations;

namespace ChessInfo.Domain
{
    public class Game
    {
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
        public byte GameResult { get; set; }
    }
}
