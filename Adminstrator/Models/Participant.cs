using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Adminstrator.Models
{
    public class Participant
    {
        [Key]
        public int ParticipantId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }

        // Link to User for login
        public int UserId { get; set; }
        [ForeignKey("UserId")]

        [JsonIgnore]
        public User? User { get; set; }

        // Many-to-many: Events attended by this participant
        [JsonIgnore]
        public ICollection<Event> Events { get; set; }=new List<Event>();
    }
}
