using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        
        public string[] KeySkills { get; set; }

        // Link to User for login
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public ICollection<Event> Events { get; set; }

        public ICollection<Feedback> Feedbacks { get; set; }
    }
}
