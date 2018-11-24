using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ChessInfo.Business
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        public short Rating { get; set; }

        [JsonIgnore]
        [NotMapped]
        public bool IsValid => !string.IsNullOrWhiteSpace(FirstName)
                               && !string.IsNullOrWhiteSpace(LastName)
                               && Rating >= 0;
    }
}