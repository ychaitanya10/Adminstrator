using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Adminstrator.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }  // Primary key for User

        // Ensure the username is required and has a minimum length of 6
        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Username { get; set; }

        // Ensure the password is required and has a minimum length of 6
        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }

        // The role of the user (e.g., "Participant", "Administrator", "Speaker")
        [Required]
        public string Role { get; set; }

        // Relationship with Administrator
        [JsonIgnore]  // Ignore this property during JSON serialization
        public Administrator? Admin { get; set; }

        // Relationship with Participant
        [JsonIgnore]  // Ignore this property during JSON serialization
        public Participant? Parti { get; set; }

        // Relationship with Speaker
        [JsonIgnore]  // Ignore this property during JSON serialization
        public Speaker? Speak { get; set; }
    }
}
