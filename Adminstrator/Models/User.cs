using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Adminstrator.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        [JsonIgnore]
        public Administrator? Admin { get; set; }

        [JsonIgnore]
        public Participant? Parti { get; set; }

        [JsonIgnore]
        public Speaker? Speak { get; set; }
    }
}
