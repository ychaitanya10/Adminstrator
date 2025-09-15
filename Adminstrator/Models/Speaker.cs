using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Adminstrator.Models
{

    public class Speaker
    {
        [Key]
        public int SpeakerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        
        public string KeySkills { get; set; }

        // Link to User for login
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [JsonIgnore]
        public User ?User { get; set; }
        [JsonIgnore]

        public ICollection<Event> Events { get; set; } = new List<Event>();
        [JsonIgnore]

        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
