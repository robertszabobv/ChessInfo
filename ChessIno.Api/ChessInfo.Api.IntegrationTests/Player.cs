using System.ComponentModel.DataAnnotations;

namespace ChessInfo.Api.IntegrationTests
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public short Rating { get; set; }       
    }
}