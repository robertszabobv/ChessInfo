using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ChessInfo.Business
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

        [JsonIgnore]
        public bool IsValid => !string.IsNullOrWhiteSpace(FirstName)
                               && !string.IsNullOrWhiteSpace(LastName)
                               && Rating >= 0;
    }
}